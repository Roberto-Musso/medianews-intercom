using NAudio.Wave;
using NAudio.Wave.Asio;
using NAudio.CoreAudioApi;

namespace AudioWdmToAsio;

/// <summary>
/// Provider personalizzato per WDM speaker che legge dal listenBuffer thread-safe
/// </summary>
public class IntercomListenProvider : IWaveProvider
{
    public WaveFormat WaveFormat { get; }
    private readonly object lockObject = new object();
    private readonly Queue<float> audioQueue = new Queue<float>();
    private readonly int maxSamples = 48000 * 2 * 2; // 2 secondi di buffer stereo

    public IntercomListenProvider(WaveFormat waveFormat)
    {
        WaveFormat = waveFormat;
    }

    public void AddSamples(float[] samples)
    {
        lock (lockObject)
        {
            foreach (var sample in samples)
            {
                if (audioQueue.Count < maxSamples)
                {
                    audioQueue.Enqueue(sample);
                }
                else
                {
                    // Buffer pieno - scarta il sample più vecchio
                    audioQueue.Dequeue();
                    audioQueue.Enqueue(sample);
                }
            }

            totalSamplesWritten += samples.Length;
        }
    }

    private long totalSamplesRead = 0;
    private long totalSamplesWritten = 0;

    public int Read(byte[] buffer, int offset, int count)
    {
        int samplesNeeded = count / sizeof(float);
        float[] samples = new float[samplesNeeded];
        int samplesAvailable;

        // Minimizza il tempo di lock: copia solo i dati
        lock (lockObject)
        {
            samplesAvailable = Math.Min(samplesNeeded, audioQueue.Count);

            for (int i = 0; i < samplesAvailable; i++)
            {
                samples[i] = audioQueue.Dequeue();
            }

            totalSamplesRead += samplesNeeded;
        }

        // Riempi il resto con silenzio fuori dal lock
        for (int i = samplesAvailable; i < samplesNeeded; i++)
        {
            samples[i] = 0.0f;
        }

        Buffer.BlockCopy(samples, 0, buffer, offset, count);
        return count;
    }

    public long TotalSamplesRead => totalSamplesRead;
    public long TotalSamplesWritten => totalSamplesWritten;

    public int BufferedSamples
    {
        get
        {
            lock (lockObject)
            {
                return audioQueue.Count;
            }
        }
    }

    public void Clear()
    {
        lock (lockObject)
        {
            audioQueue.Clear();
        }
    }
}

/// <summary>
/// Provider personalizzato per ASIO che distribuisce l'audio dal talkBuffer ai canali corretti
/// </summary>
public class IntercomAsioProvider : IWaveProvider
{
    public WaveFormat WaveFormat { get; }
    private readonly Func<List<IntercomChannel>> getActiveTalkChannels;
    private readonly Func<BufferedWaveProvider?> getTalkBuffer;
    private readonly int outputChannels;

    public IntercomAsioProvider(WaveFormat waveFormat, Func<List<IntercomChannel>> getActiveTalkChannels, Func<BufferedWaveProvider?> getTalkBuffer)
    {
        WaveFormat = waveFormat;
        this.getActiveTalkChannels = getActiveTalkChannels;
        this.getTalkBuffer = getTalkBuffer;
        this.outputChannels = waveFormat.Channels;
    }

    public int Read(byte[] buffer, int offset, int count)
    {
        // Azzera tutto il buffer (silenzio di default)
        Array.Clear(buffer, offset, count);

        var talkBuffer = getTalkBuffer();
        var activeTalkChannels = getActiveTalkChannels();

        if (talkBuffer == null || activeTalkChannels.Count == 0)
        {
            return count; // Restituisci silenzio
        }

        // Calcola quanti sample servono per canale
        int bytesPerSample = sizeof(float);
        int samplesPerChannel = count / (outputChannels * bytesPerSample);
        int bytesNeededFromTalk = samplesPerChannel * bytesPerSample;

        if (talkBuffer.BufferedBytes < bytesNeededFromTalk)
        {
            return count; // Non abbastanza dati, restituisci silenzio
        }

        // Leggi dal talkBuffer (MONO)
        byte[] talkData = new byte[bytesNeededFromTalk];
        int bytesRead = talkBuffer.Read(talkData, 0, bytesNeededFromTalk);

        if (bytesRead != bytesNeededFromTalk)
        {
            return count; // Lettura incompleta, restituisci silenzio
        }

        // Converti in float
        float[] talkSamples = new float[bytesRead / sizeof(float)];
        Buffer.BlockCopy(talkData, 0, talkSamples, 0, bytesRead);

        // Distribuisci ai canali con Talk attivo
        float[] outputSamples = new float[samplesPerChannel * outputChannels];

        foreach (var channel in activeTalkChannels)
        {
            int asioOutputCh = channel.AsioOutputChannel;
            if (asioOutputCh >= outputChannels)
                continue;

            float gain = channel.GetOutputGain();

            // Copia i sample mono su questo canale dell'interleaved output
            for (int i = 0; i < samplesPerChannel && i < talkSamples.Length; i++)
            {
                float sample = talkSamples[i] * gain;
                sample = Math.Max(-1.0f, Math.Min(1.0f, sample));
                outputSamples[i * outputChannels + asioOutputCh] = sample;
            }
        }

        // Converti in bytes
        Buffer.BlockCopy(outputSamples, 0, buffer, offset, count);
        return count;
    }
}

public partial class Form1 : Form
{
    private IntercomSettings settings;

    // Sample rate usato da ASIO (determinato all'inizializzazione)
    private int asioSampleRate = 48000;

    // Audio components
    private WasapiCapture? microphoneInput;  // Cattura dal microfono
    private AsioOut? asioDriver;             // Driver ASIO per I/O
    private WasapiOut? speakerOutput;        // Output agli altoparlanti

    // Buffers per il Talk path (microfono → ASIO output)
    private BufferedWaveProvider? talkBuffer;     // Buffer per il microfono convertito

    // Provider per il Listen path (ASIO input → speaker)
    private IntercomListenProvider? listenProvider;   // Provider custom per Listen path

    // Noise gate state
    private float gateGain = 1.0f;  // Guadagno corrente del gate (0.0 = chiuso, 1.0 = aperto)

    private bool isStreaming = false;

    // Web server per controllo remoto
    private WebServer? webServer;
    private float currentVuLevel = 0.0f;  // Livello VU corrente (0.0 - 1.0)

    public Form1()
    {
        InitializeComponent();

        // Carica le impostazioni salvate, o crea nuove impostazioni di default
        settings = IntercomSettings.Load() ?? new IntercomSettings();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        try
        {
            Log("Audio Intercom System started - 8 Channel");
            Log("Click Settings to configure devices and channels");
            Log("Click START to begin streaming");
            Log("Use TALK buttons to send microphone to individual ASIO outputs");
            Log("Use LISTEN buttons to hear from individual ASIO inputs");

            // Inizializza i livelli e i nomi dei canali dalle impostazioni
            for (int i = 0; i < 8; i++)
            {
                var channel = settings.Channels[i];
                channelNameTextBoxes[i].Text = channel.ChannelName;
                inputLevelKnobs[i].Value = channel.InputLevel;
                outputLevelKnobs[i].Value = channel.OutputLevel;
                inputValueLabels[i].Text = channel.InputLevel.ToString();
                outputValueLabels[i].Text = channel.OutputLevel.ToString();
            }

            // Imposta visibilità del log
            UpdateLogVisibility();

            // Avvia il web server per controllo remoto
            try
            {
                webServer = new WebServer(
                    port: 8080,
                    settings: settings,
                    toggleTalkCallback: WebToggleTalk,
                    toggleListenCallback: WebToggleListen,
                    setInputLevelCallback: WebSetInputLevel,
                    setOutputLevelCallback: WebSetOutputLevel,
                    getVuLevelCallback: () => currentVuLevel
                );
                webServer.Start();
                Log("Web interface started on http://localhost:8080");
                Log("Remote control available from other computers");
            }
            catch (Exception webEx)
            {
                Log($"Warning: Could not start web server: {webEx.Message}");
            }
        }
        catch (Exception ex)
        {
            Log($"Error during initialization: {ex.Message}");
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }


    private void UpdateLogVisibility()
    {
        // Mostra/nascondi il log senza modificare la dimensione della finestra
        textBoxLog.Visible = settings.ShowLog;
    }

    private void buttonSettings_Click(object sender, EventArgs e)
    {
        if (isStreaming)
        {
            MessageBox.Show("Please stop streaming before changing settings.",
                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using (var settingsForm = new SettingsForm(settings))
        {
            if (settingsForm.ShowDialog() == DialogResult.OK)
            {
                Log("Settings updated");
                Log($"Microphone: {settings.WdmInputDevice}");
                Log($"Speaker: {settings.WdmOutputDevice}");
                Log($"ASIO Device: {settings.AsioDevice}");
                Log($"Buffer: {settings.BufferMs}ms");

                // Aggiorna la visibilità del log in base alle nuove impostazioni
                UpdateLogVisibility();
            }
        }
    }

    private void TalkButton_Click(object? sender, EventArgs e)
    {
        if (!isStreaming)
        {
            MessageBox.Show("Please start streaming first.",
                "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (sender is Button btn && btn.Tag is int channelIndex)
        {
            var channel = settings.Channels[channelIndex];
            channel.IsTalkActive = !channel.IsTalkActive;

            UpdateTalkButton(channelIndex);

            if (channel.IsTalkActive)
            {
                Log($"Channel {channelIndex + 1} TALK activated - Sending microphone");
            }
            else
            {
                Log($"Channel {channelIndex + 1} TALK deactivated");
            }
        }
    }

    private void UpdateTalkButton(int channelIndex)
    {
        var channel = settings.Channels[channelIndex];
        var btn = talkButtons[channelIndex];

        if (channel.IsTalkActive)
        {
            btn.BackColor = Color.FromArgb(200, 0, 0); // Rosso
            btn.Text = "TALK (ON)";
        }
        else
        {
            btn.BackColor = Color.FromArgb(70, 70, 70); // Grigio scuro
            btn.Text = "TALK";
        }
    }

    private void ListenButton_Click(object? sender, EventArgs e)
    {
        if (!isStreaming)
        {
            MessageBox.Show("Please start streaming first.",
                "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (sender is Button btn && btn.Tag is int channelIndex)
        {
            var channel = settings.Channels[channelIndex];
            channel.IsListenActive = !channel.IsListenActive;

            UpdateListenButton(channelIndex);

            if (channel.IsListenActive)
            {
                Log($"Channel {channelIndex + 1} LISTEN activated");
            }
            else
            {
                Log($"Channel {channelIndex + 1} LISTEN deactivated");
            }
        }
    }

    private void UpdateListenButton(int channelIndex)
    {
        var channel = settings.Channels[channelIndex];
        var btn = listenButtons[channelIndex];

        if (channel.IsListenActive)
        {
            btn.BackColor = Color.FromArgb(0, 150, 200); // Azzurro
            btn.Text = "LISTEN (ON)";
        }
        else
        {
            btn.BackColor = Color.FromArgb(70, 70, 70); // Grigio scuro
            btn.Text = "LISTEN";
        }
    }


    /// <summary>
    /// Salva le impostazioni senza mostrare errori all'utente (logging solo)
    /// </summary>
    private void SaveSettingsQuietly()
    {
        try
        {
            settings.Save();
        }
        catch (Exception ex)
        {
            // Log l'errore ma non disturbare l'utente
            Log($"Warning: Could not save settings - {ex.Message}");
        }
    }

    private void buttonStartStop_Click(object sender, EventArgs e)
    {
        if (!isStreaming)
        {
            StartStreaming();
        }
        else
        {
            StopStreaming();
        }
    }

    private void StartStreaming()
    {
        try
        {
            // Verifica che le impostazioni siano configurate
            if (string.IsNullOrEmpty(settings.WdmInputDevice) ||
                string.IsNullOrEmpty(settings.WdmOutputDevice) ||
                string.IsNullOrEmpty(settings.AsioDevice))
            {
                MessageBox.Show("Please configure devices in Settings first.",
                    "Configuration Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Log("=== Starting Intercom System ===");

            // 1. Crea driver ASIO per determinare le capacità
            InitializeAsioDriver();

            // 2. Inizializza microfono WDM input (usa il sample rate di ASIO)
            InitializeMicrophone();

            // 3. Inizializza altoparlante WDM output (usa il sample rate di ASIO)
            InitializeSpeaker();

            // 4. Inizializza ASIO per playback/record (ora che i buffer sono creati)
            InitializeAsioPlayback();

            // 4. Avvia tutto
            microphoneInput?.StartRecording();
            asioDriver?.Play();
            speakerOutput?.Play();

            isStreaming = true;
            settings.IsStreaming = true;
            buttonStartStop.Text = "STOP";
            buttonStartStop.BackColor = Color.FromArgb(200, 0, 0);

            Log("=== Intercom System Started ===");
            Log("Ready to Talk and Listen");
        }
        catch (Exception ex)
        {
            Log($"ERROR: {ex.Message}");
            MessageBox.Show($"Error starting streaming:\n{ex.Message}",
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            StopStreaming();
        }
    }

    private void InitializeMicrophone()
    {
        Log($"Initializing microphone: {settings.WdmInputDevice}");

        // Trova il device WDM input
        if (settings.WdmInputDevice.StartsWith("Default WASAPI"))
        {
            microphoneInput = new WasapiCapture();
        }
        else
        {
            var enumerator = new MMDeviceEnumerator();
            var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);

            var selectedDevice = devices.FirstOrDefault(d => d.FriendlyName == settings.WdmInputDevice);
            if (selectedDevice != null)
            {
                microphoneInput = new WasapiCapture(selectedDevice);
            }
            else
            {
                microphoneInput = new WasapiCapture();
            }
        }

        var micFormat = microphoneInput.WaveFormat;
        Log($"Microphone format: {micFormat.SampleRate}Hz, {micFormat.Channels}ch, {micFormat.BitsPerSample}bit");
        Log($"Microphone encoding: {micFormat.Encoding}");
        Log($"Microphone block align: {micFormat.BlockAlign} bytes");
        Log($"Microphone average bytes/sec: {micFormat.AverageBytesPerSecond}");

        // Usa direttamente il formato ASIO: MONO float32
        // IMPORTANTE: MONO perché ogni canale intercom usa una singola uscita ASIO
        var targetFormat = WaveFormat.CreateIeeeFloatWaveFormat(asioSampleRate, 1);
        Log($"Target ASIO format: {targetFormat.SampleRate}Hz, {targetFormat.Channels}ch (MONO)");

        // Crea buffer per il Talk path
        talkBuffer = new BufferedWaveProvider(targetFormat)
        {
            BufferDuration = TimeSpan.FromMilliseconds(500), // Buffer più grande per stabilità
            DiscardOnBufferOverflow = true
        };

        // Handler per i dati del microfono
        microphoneInput.DataAvailable += MicrophoneDataAvailable;
    }

    private int micCallbackCount = 0;
    private int micCallbackWithTalkCount = 0;

    // Test: genera un tono di 440Hz invece dell'audio del microfono
    // IMPORTANTE: Imposta a true per testare se la pipeline funziona
    private bool useTestTone = false;  // <-- FALSE: usa il microfono reale
    private double testTonePhase = 0;

    // Test: genera un tono di 880Hz per il Listen path
    private bool useListenTestTone = false;  // <-- FALSE: usa l'audio ASIO reale
    private double listenTestTonePhase = 0;

    private void MicrophoneDataAvailable(object? sender, WaveInEventArgs e)
    {
        if (talkBuffer == null || microphoneInput == null || e.BytesRecorded == 0)
            return;

        // Controlla se almeno un canale ha Talk attivo
        var activeTalkChannels = settings.GetActiveTalkChannels();
        bool anyTalkActive = activeTalkChannels.Count > 0;

        micCallbackCount++;

        if (!anyTalkActive)
            return;

        micCallbackWithTalkCount++;

        try
        {
            var micFormat = microphoneInput.WaveFormat;

            // Debug: log i primi 10 bytes raw solo per il primo callback
            if (micCallbackWithTalkCount == 1)
            {
                string hexBytes = "";
                for (int i = 0; i < Math.Min(16, e.BytesRecorded); i++)
                {
                    hexBytes += e.Buffer[i].ToString("X2") + " ";
                }
                Log($"First 16 bytes (hex): {hexBytes}");
            }

            // Converti i dati del microfono in float32 mono
            int sampleCount = e.BytesRecorded / micFormat.BlockAlign;
            float[] monoSamples = new float[sampleCount];

            // Controlla il tipo di encoding
            if (micFormat.Encoding == WaveFormatEncoding.IeeeFloat)
            {
                // Il microfono è già in float (Float32)
                float[] sourceSamples = new float[e.BytesRecorded / sizeof(float)];
                Buffer.BlockCopy(e.Buffer, 0, sourceSamples, 0, e.BytesRecorded);

                // Copia sempre i sample (mai assegnazione per riferimento)
                if (micFormat.Channels == 1)
                {
                    // Mono: copia diretta
                    Array.Copy(sourceSamples, monoSamples, sampleCount);
                }
                else
                {
                    // Stereo → mono: prendi solo canale sinistro
                    for (int i = 0; i < sampleCount; i++)
                    {
                        monoSamples[i] = sourceSamples[i * micFormat.Channels];
                    }
                }
            }
            else if (micFormat.Encoding == WaveFormatEncoding.Pcm && micFormat.BitsPerSample == 16)
            {
                // Converti da PCM Int16 a float
                for (int i = 0; i < sampleCount; i++)
                {
                    short sample = BitConverter.ToInt16(e.Buffer, i * micFormat.BlockAlign);
                    monoSamples[i] = sample / 32768f; // Normalizza a -1.0 ... 1.0
                }
            }
            else if (micFormat.Encoding == WaveFormatEncoding.Pcm && micFormat.BitsPerSample == 32)
            {
                // Converti da PCM Int32 a float
                for (int i = 0; i < sampleCount; i++)
                {
                    int sample = BitConverter.ToInt32(e.Buffer, i * micFormat.BlockAlign);
                    monoSamples[i] = sample / 2147483648f; // Normalizza a -1.0 ... 1.0
                }
            }
            else
            {
                // Formato non supportato - log solo la prima volta
                if (micCallbackCount == 1)
                {
                    Log($"WARNING: Unsupported audio format - Encoding: {micFormat.Encoding}, BitsPerSample: {micFormat.BitsPerSample}");
                }
                return;
            }

            // Calculate audio level for VU meter (before gate)
            UpdateVUMeter(monoSamples);

            // Applica noise gate se abilitato
            if (settings.GateEnabled)
            {
                ApplyNoiseGate(monoSamples);
            }

            // TEST: Sovrascrivi con tono di test se abilitato
            if (useTestTone)
            {
                // Genera un tono di 440Hz per test
                double frequency = 440.0;
                double amplitude = 0.3;
                for (int i = 0; i < monoSamples.Length; i++)
                {
                    monoSamples[i] = (float)(amplitude * Math.Sin(testTonePhase));
                    testTonePhase += 2.0 * Math.PI * frequency / 48000.0;
                    if (testTonePhase > 2.0 * Math.PI)
                        testTonePhase -= 2.0 * Math.PI;
                }
            }

            // Debug: log i primi sample convertiti
            if (micCallbackWithTalkCount == 1)
            {
                string samples = "";
                for (int i = 0; i < Math.Min(10, monoSamples.Length); i++)
                {
                    samples += monoSamples[i].ToString("F6") + " ";
                }
                Log($"First 10 samples (float): {samples}");
                Log($"Test tone enabled: {useTestTone}");
            }

            // Converti in byte array e aggiungi al buffer
            byte[] outputData = new byte[monoSamples.Length * sizeof(float)];
            Buffer.BlockCopy(monoSamples, 0, outputData, 0, outputData.Length);

            talkBuffer.AddSamples(outputData, 0, outputData.Length);

            // Debug: verifica buffer
            if (micCallbackWithTalkCount == 1)
            {
                Log($"Added {outputData.Length} bytes to talkBuffer. Sample count: {monoSamples.Length}");
            }
        }
        catch
        {
            // Non logghiamo nel callback per evitare deadlock - ignoriamo l'errore silenziosamente
        }
    }

    private void UpdateVUMeter(float[] samples)
    {
        // Calculate RMS level
        double sumSquares = 0;
        for (int i = 0; i < samples.Length; i++)
        {
            sumSquares += samples[i] * samples[i];
        }
        double rms = Math.Sqrt(sumSquares / samples.Length);

        // Convert to dB (-60 to 0 dB range)
        double db = rms > 0.0 ? 20.0 * Math.Log10(rms) : -60.0;
        db = Math.Max(-60, Math.Min(0, db));

        // Store for web interface (normalized 0.0 - 1.0)
        currentVuLevel = (float)((db + 60.0) / 60.0);

        // Update VU meter on UI thread
        if (vuMeter != null && !vuMeter.IsDisposed)
        {
            try
            {
                if (vuMeter.InvokeRequired)
                {
                    vuMeter.BeginInvoke(new Action(() =>
                    {
                        vuMeter.SetLevelDb((float)db);
                    }));
                }
                else
                {
                    vuMeter.SetLevelDb((float)db);
                }
            }
            catch
            {
                // Ignore errors during shutdown
            }
        }
    }

    private void ApplyNoiseGate(float[] samples)
    {
        // Calcola RMS del buffer corrente
        float rms = 0.0f;
        for (int i = 0; i < samples.Length; i++)
        {
            rms += samples[i] * samples[i];
        }
        rms = (float)Math.Sqrt(rms / samples.Length);

        // Converti threshold da dB a lineare
        float thresholdLinear = (float)Math.Pow(10.0, settings.GateThresholdDb / 20.0);

        // Determina se il gate dovrebbe essere aperto o chiuso
        bool shouldOpen = rms > thresholdLinear;

        // Calcola i coefficienti di attack/release
        // Coefficiente = 1 - e^(-1/(sampleRate * timeInSeconds))
        float attackCoeff = 1.0f - (float)Math.Exp(-1.0 / (asioSampleRate * (settings.GateAttackMs / 1000.0)));
        float releaseCoeff = 1.0f - (float)Math.Exp(-1.0 / (asioSampleRate * (settings.GateReleaseMs / 1000.0)));

        // Target gain: 1.0 se il gate è aperto, 0.0 se chiuso
        float targetGain = shouldOpen ? 1.0f : 0.0f;

        // Applica envelope follower con attack/release
        for (int i = 0; i < samples.Length; i++)
        {
            // Smooth transition verso il target
            if (targetGain > gateGain)
            {
                // Attack: il gate si apre
                gateGain += (targetGain - gateGain) * attackCoeff;
            }
            else
            {
                // Release: il gate si chiude
                gateGain += (targetGain - gateGain) * releaseCoeff;
            }

            // Applica il guadagno del gate
            samples[i] *= gateGain;
        }
    }

    private void InitializeSpeaker()
    {
        Log($"Initializing speaker: {settings.WdmOutputDevice}");

        var enumerator = new MMDeviceEnumerator();
        var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);

        var selectedDevice = devices.FirstOrDefault(d => d.FriendlyName == settings.WdmOutputDevice);

        if (selectedDevice != null)
        {
            // Latency ridotta a 50ms per richieste più frequenti e buffer più stabili
            speakerOutput = new WasapiOut(selectedDevice, AudioClientShareMode.Shared, false, 50);
        }
        else
        {
            speakerOutput = new WasapiOut();
        }

        // Crea il provider personalizzato per il Listen path (stereo Float32 @ asioSampleRate)
        var listenFormat = WaveFormat.CreateIeeeFloatWaveFormat(asioSampleRate, 2);
        listenProvider = new IntercomListenProvider(listenFormat);

        Log($"Listen provider created: {listenFormat.SampleRate}Hz, {listenFormat.Channels}ch");

        // Inizializza WasapiOut con il provider personalizzato
        speakerOutput.Init(listenProvider);

        // Pre-riempie con minimo silenzio necessario (100ms per partenza stabile)
        int silenceSamples = (listenFormat.SampleRate * listenFormat.Channels) / 10; // 100ms
        float[] silence = new float[silenceSamples];
        Array.Clear(silence, 0, silence.Length);
        listenProvider.AddSamples(silence);

        Log($"Speaker output format: {speakerOutput.OutputWaveFormat.SampleRate}Hz, {speakerOutput.OutputWaveFormat.Channels}ch");
        Log($"Pre-filled with 100ms of silence ({silenceSamples} samples)");
        Log($"Speaker initialized successfully with IntercomListenProvider");
    }

    private void InitializeAsioDriver()
    {
        Log($"Initializing ASIO driver: {settings.AsioDevice}");

        asioDriver = new AsioOut(settings.AsioDevice);

        // Per ora usa 48kHz come default (potrebbe essere configurabile in futuro)
        asioSampleRate = 48000;

        Log($"ASIO driver created - will use {asioSampleRate}Hz sample rate");
        Log($"ASIO capabilities: {asioDriver.DriverInputChannelCount} inputs, {asioDriver.DriverOutputChannelCount} outputs");
    }

    private void InitializeAsioPlayback()
    {
        if (asioDriver == null)
        {
            throw new InvalidOperationException("ASIO driver not initialized");
        }

        Log($"Initializing ASIO at {asioSampleRate}Hz");

        // Handler per processare audio da ASIO inputs e gestire gli outputs
        asioDriver.AudioAvailable += AsioAudioAvailable;

        // Usa solo gli 8 canali che ci servono, non tutti i 16
        int channelsToUse = Math.Min(8, asioDriver.DriverOutputChannelCount);

        var asioFormat = WaveFormat.CreateIeeeFloatWaveFormat(asioSampleRate, channelsToUse);

        // Usa il nostro provider personalizzato che legge dal talkBuffer
        var intercomProvider = new IntercomAsioProvider(
            asioFormat,
            () => settings.GetActiveTalkChannels(),
            () => talkBuffer
        );

        // Inizializza con il nostro provider
        asioDriver.InitRecordAndPlayback(intercomProvider, asioDriver.DriverInputChannelCount, asioSampleRate);

        Log($"ASIO initialized: {asioDriver.DriverInputChannelCount} inputs, {asioDriver.DriverOutputChannelCount} outputs");
        Log($"Using {channelsToUse} output channels for playback");
        Log($"Audio routing handled by IntercomAsioProvider");
    }

    private int asioCallbackCount = 0;

    // Debug: track gain values being applied in Listen path (volatile for thread safety)
    private volatile float[] debugListenGains = new float[8];
    private volatile int listenBytesWritten = 0;
    private volatile int listenCallbacksWithData = 0;

    // Debug: monitor ASIO input levels (RMS per channel)
    private volatile float[] asioInputRMS = new float[16]; // Max 16 canali ASIO
    private volatile int invalidSamplesCount = 0; // Contatore sample NaN/Infinity filtrati

    // Buffer riutilizzabile per GetAsInterleavedSamples (per performance)
    private float[]? interleavedBuffer = null;

    private void AsioAudioAvailable(object? sender, AsioAudioAvailableEventArgs e)
    {
        if (listenProvider == null || e.InputBuffers == null)
            return;

        asioCallbackCount++;

        try
        {
            // DEBUG: Calcola RMS per i primi 8 canali ASIO input per monitorare livelli
            if (asioCallbackCount % 10 == 0) // Ogni 10 callback per non rallentare troppo
            {
                int channelsToMonitor = Math.Min(8, e.InputBuffers.Length);
                for (int ch = 0; ch < channelsToMonitor; ch++)
                {
                    IntPtr inputBuffer = e.InputBuffers[ch];
                    float[] samples = new float[e.SamplesPerBuffer];
                    System.Runtime.InteropServices.Marshal.Copy(inputBuffer, samples, 0, e.SamplesPerBuffer);

                    // Calcola RMS (filtrando valori invalidi)
                    double sum = 0;
                    int validSamples = 0;
                    for (int i = 0; i < samples.Length; i++)
                    {
                        float s = samples[i];
                        if (!float.IsNaN(s) && !float.IsInfinity(s))
                        {
                            sum += s * s;
                            validSamples++;
                        }
                    }
                    float rms = validSamples > 0 ? (float)Math.Sqrt(sum / validSamples) : 0.0f;
                    asioInputRMS[ch] = rms;
                }
            }

            // LISTEN PATH: Processa gli input ASIO e mixa i canali attivi

            int samplesPerBuffer = e.SamplesPerBuffer;
            float[] mixedOutput = new float[samplesPerBuffer * 2]; // Stereo

            // Per ogni canale con Listen attivo
            var activeChannels = settings.GetActiveListenChannels();

            // IMPORTANTE: scrivi nel buffer SOLO se ci sono canali Listen attivi
            if (activeChannels.Count > 0)
            {
                if (useListenTestTone)
                {
                    // TEST: Genera un tono di 880Hz invece di leggere da ASIO
                    double frequency = 880.0;
                    double amplitude = 0.3;
                    for (int i = 0; i < samplesPerBuffer; i++)
                    {
                        float sample = (float)(amplitude * Math.Sin(listenTestTonePhase));
                        listenTestTonePhase += 2.0 * Math.PI * frequency / 48000.0;
                        if (listenTestTonePhase > 2.0 * Math.PI)
                            listenTestTonePhase -= 2.0 * Math.PI;

                        // Stereo - stesso tono su entrambi i canali
                        mixedOutput[i * 2] = sample;
                        mixedOutput[i * 2 + 1] = sample;
                    }

                    debugListenGains[0] = 1.0f; // Debug
                }
                else
                {
                    // Usa il metodo NAudio per ottenere tutti i sample interleaved e convertiti
                    int totalChannels = e.InputBuffers.Length;
                    int totalSamples = samplesPerBuffer * totalChannels;

                    // Crea o ridimensiona il buffer se necessario
                    if (interleavedBuffer == null || interleavedBuffer.Length < totalSamples)
                    {
                        interleavedBuffer = new float[totalSamples];
                    }

                    // Riempi il buffer con i sample convertiti da NAudio
                    e.GetAsInterleavedSamples(interleavedBuffer);

                    // interleavedBuffer contiene: [Ch0_S0, Ch1_S0, ..., Ch15_S0, Ch0_S1, Ch1_S1, ...]

                    // Normale: leggi da ASIO e de-interleave i canali necessari
                    foreach (var channel in activeChannels)
                    {
                        int asioInputCh = channel.AsioInputChannel;

                        // Verifica che il canale esista
                        if (asioInputCh >= totalChannels)
                            continue;

                        float gain = channel.GetInputGain();

                        // Debug: store gain being applied
                        debugListenGains[channel.ChannelNumber - 1] = gain;

                        // De-interleave: estrai i sample per questo canale specifico
                        for (int i = 0; i < samplesPerBuffer; i++)
                        {
                            int sampleIndex = (i * totalChannels) + asioInputCh;
                            float sample = interleavedBuffer[sampleIndex];

                            // IMPORTANTE: Filtra valori invalidi (NaN, Infinity)
                            if (float.IsNaN(sample) || float.IsInfinity(sample))
                            {
                                invalidSamplesCount++;
                                sample = 0.0f;
                            }

                            // Applica gain
                            sample *= gain;

                            // Aggiungi a entrambi i canali stereo del mix output
                            mixedOutput[i * 2] += sample;
                            mixedOutput[i * 2 + 1] += sample;
                        }
                    }

                    // Normalizza SOLO dopo il mix finale (evita clipping del mixer)
                    for (int i = 0; i < mixedOutput.Length; i++)
                    {
                        float sample = mixedOutput[i];
                        mixedOutput[i] = Math.Max(-1.0f, Math.Min(1.0f, sample));
                    }
                }

                // Aggiungi i sample al provider personalizzato
                listenProvider.AddSamples(mixedOutput);

                // Debug: traccia quanti byte stiamo scrivendo
                listenCallbacksWithData++;
                listenBytesWritten += mixedOutput.Length * sizeof(float);
            }

            // TALK PATH: Gestito automaticamente dall'IntercomAsioProvider
            // NAudio leggerà dal nostro provider personalizzato, non c'è bisogno di scrivere manualmente
        }
        catch
        {
            // Non logghiamo nel callback per evitare deadlock
            // L'eccezione viene ignorata silenziosamente
        }
    }

    private void StopStreaming()
    {
        try
        {
            Log("=== Stopping Intercom System ===");

            // Stop microphone
            if (microphoneInput != null)
            {
                try
                {
                    microphoneInput.StopRecording();
                    microphoneInput.Dispose();
                    microphoneInput = null;
                    Log("Microphone stopped");
                }
                catch (Exception ex)
                {
                    Log($"Error stopping microphone: {ex.Message}");
                }
            }

            // Stop ASIO
            if (asioDriver != null)
            {
                try
                {
                    asioDriver.Stop();
                    asioDriver.Dispose();
                    asioDriver = null;
                    Log("ASIO stopped");
                }
                catch (Exception ex)
                {
                    Log($"Error stopping ASIO: {ex.Message}");
                }
            }

            // Stop speaker
            if (speakerOutput != null)
            {
                try
                {
                    speakerOutput.Stop();
                    speakerOutput.Dispose();
                    speakerOutput = null;
                    Log("Speaker stopped");
                }
                catch (Exception ex)
                {
                    Log($"Error stopping speaker: {ex.Message}");
                }
            }

            // Cleanup buffers and providers
            talkBuffer = null;

            if (listenProvider != null)
            {
                listenProvider.Clear();
                listenProvider = null;
            }

            // Reset state
            isStreaming = false;
            settings.IsStreaming = false;

            // Reset counters
            micCallbackCount = 0;
            micCallbackWithTalkCount = 0;
            asioCallbackCount = 0;

            // Reset debug gains and listen stats
            Array.Clear(debugListenGains, 0, debugListenGains.Length);
            Array.Clear(asioInputRMS, 0, asioInputRMS.Length);
            listenBytesWritten = 0;
            listenCallbacksWithData = 0;
            invalidSamplesCount = 0;

            // Reset test tone phases
            testTonePhase = 0;
            listenTestTonePhase = 0;

            // Reset all Talk and Listen buttons
            for (int i = 0; i < 8; i++)
            {
                settings.Channels[i].IsTalkActive = false;
                settings.Channels[i].IsListenActive = false;
                UpdateTalkButton(i);
                UpdateListenButton(i);
            }

            buttonStartStop.Text = "START";
            buttonStartStop.BackColor = Color.FromArgb(0, 150, 0);

            Log("=== Intercom System Stopped ===");
        }
        catch (Exception ex)
        {
            Log($"CRITICAL ERROR during stop: {ex.Message}");
        }
    }

    private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
    {
        StopStreaming();

        // Ferma il web server
        webServer?.Stop();

        // Salva le impostazioni come safety net (già salvate a ogni modifica)
        SaveSettingsQuietly();
    }

    private void debugTimer_Tick(object? sender, EventArgs e)
    {
        // Update input value labels to show actual gain being applied in Listen path
        for (int i = 0; i < 8; i++)
        {
            var channel = settings.Channels[i];
            float appliedGain = debugListenGains[i];

            if (channel.IsListenActive && appliedGain > 0)
            {
                // Show both slider value and actual applied gain
                inputValueLabels[i].Text = $"{channel.InputLevel} (x{appliedGain:F2})";
            }
            else
            {
                // Just show slider value
                inputValueLabels[i].Text = channel.InputLevel.ToString();
            }
        }

        // Mostra statistiche in base alla modalità debug
        if (isStreaming)
        {
            if (settings.EnableDebug && listenProvider != null)
            {
                // Modalità DEBUG: mostra statistiche dettagliate
                int bufferedSamples = listenProvider.BufferedSamples;
                int maxSamples = 48000 * 2 * 2; // 2 secondi stereo
                float bufferPercent = (bufferedSamples * 100.0f) / maxSamples;

                long written = listenProvider.TotalSamplesWritten;
                long read = listenProvider.TotalSamplesRead;
                long diff = written - read;

                // Mostra livelli RMS degli input ASIO (primi 8 canali)
                string rmsLevels = "";
                for (int i = 0; i < 8; i++)
                {
                    float rms = asioInputRMS[i];
                    if (rms > 0.001f) // Mostra solo se c'è segnale significativo
                    {
                        rmsLevels += $"Ch{i + 1}:{rms:F4} ";
                    }
                }

                // Debug info available in log
            }
            else
            {
                // Status shown in button color
            }
        }
        else
        {
            // Status shown in button color
        }
    }

    private void Log(string message)
    {
        if (InvokeRequired)
        {
            Invoke(new Action(() => Log(message)));
            return;
        }

        string timestamp = DateTime.Now.ToString("HH:mm:ss");
        textBoxLog.AppendText($"[{timestamp}] {message}\r\n");
        textBoxLog.SelectionStart = textBoxLog.Text.Length;
        textBoxLog.ScrollToCaret();
    }

    #region Web Server Callbacks

    // Questi metodi vengono chiamati dal web server e devono invocare le azioni sul thread UI

    private void WebToggleTalk(int channelIndex)
    {
        if (InvokeRequired)
        {
            Invoke(new Action(() => WebToggleTalk(channelIndex)));
            return;
        }

        if (channelIndex < 0 || channelIndex >= 8) return;

        var button = talkButtons[channelIndex];
        TalkButton_Click(button, EventArgs.Empty);
    }

    private void WebToggleListen(int channelIndex)
    {
        if (InvokeRequired)
        {
            Invoke(new Action(() => WebToggleListen(channelIndex)));
            return;
        }

        if (channelIndex < 0 || channelIndex >= 8) return;

        var button = listenButtons[channelIndex];
        ListenButton_Click(button, EventArgs.Empty);
    }

    private void WebSetInputLevel(int channelIndex, int value)
    {
        if (InvokeRequired)
        {
            Invoke(new Action(() => WebSetInputLevel(channelIndex, value)));
            return;
        }

        if (channelIndex < 0 || channelIndex >= 8) return;
        if (value < 0 || value > 130) return;

        var knob = inputLevelKnobs[channelIndex];
        knob.Value = value;
        // Il ValueChanged event verrà chiamato automaticamente
    }

    private void WebSetOutputLevel(int channelIndex, int value)
    {
        if (InvokeRequired)
        {
            Invoke(new Action(() => WebSetOutputLevel(channelIndex, value)));
            return;
        }

        if (channelIndex < 0 || channelIndex >= 8) return;
        if (value < 0 || value > 130) return;

        var knob = outputLevelKnobs[channelIndex];
        knob.Value = value;
        // Il ValueChanged event verrà chiamato automaticamente
    }

    #endregion
}
