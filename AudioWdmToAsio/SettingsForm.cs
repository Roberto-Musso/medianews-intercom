using NAudio.Wave;
using NAudio.Wave.Asio;
using NAudio.CoreAudioApi;

namespace AudioWdmToAsio;

public partial class SettingsForm : Form
{
    private IntercomSettings settings;
    private int asioInputChannelCount = 0;
    private int asioOutputChannelCount = 0;

    // Controlli dinamici per il mapping dei canali
    private List<Label> channelLabels = new List<Label>();
    private List<NumericUpDown> asioInputChannelControls = new List<NumericUpDown>();
    private List<NumericUpDown> asioOutputChannelControls = new List<NumericUpDown>();

    public SettingsForm(IntercomSettings settings)
    {
        InitializeComponent();
        this.settings = settings;
    }

    private void SettingsForm_Load(object sender, EventArgs e)
    {
        // Carica dispositivi
        LoadWdmInputDevices();
        LoadWdmOutputDevices();
        LoadAsioDevices();

        // Carica impostazioni correnti (questo seleziona il device ASIO)
        LoadCurrentSettings();

        // I controlli di mapping vengono creati in comboBoxAsioDevice_SelectedIndexChanged
        // quando viene selezionato il device ASIO
    }

    private void LoadWdmInputDevices()
    {
        comboBoxWdmInput.Items.Clear();

        try
        {
            var device = new WasapiCapture();
            comboBoxWdmInput.Items.Add("Default WASAPI");
        }
        catch { }

        var enumerator = new MMDeviceEnumerator();
        var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);

        foreach (var device in devices)
        {
            comboBoxWdmInput.Items.Add(device.FriendlyName);
        }

        if (comboBoxWdmInput.Items.Count > 0)
            comboBoxWdmInput.SelectedIndex = 0;
    }

    private void LoadWdmOutputDevices()
    {
        comboBoxWdmOutput.Items.Clear();

        var enumerator = new MMDeviceEnumerator();
        var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);

        foreach (var device in devices)
        {
            comboBoxWdmOutput.Items.Add(device.FriendlyName);
        }

        if (comboBoxWdmOutput.Items.Count > 0)
            comboBoxWdmOutput.SelectedIndex = 0;
    }

    private void LoadAsioDevices()
    {
        comboBoxAsioDevice.Items.Clear();

        var asioDriverNames = AsioOut.GetDriverNames();

        if (asioDriverNames.Length == 0)
        {
            MessageBox.Show("No ASIO drivers found! Please install an ASIO driver.",
                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        foreach (var driverName in asioDriverNames)
        {
            comboBoxAsioDevice.Items.Add(driverName);
        }

        if (comboBoxAsioDevice.Items.Count > 0)
            comboBoxAsioDevice.SelectedIndex = 0;
    }

    private void LoadCurrentSettings()
    {
        // Seleziona i dispositivi dalle impostazioni
        if (!string.IsNullOrEmpty(settings.WdmInputDevice))
        {
            int index = comboBoxWdmInput.FindStringExact(settings.WdmInputDevice);
            if (index >= 0)
                comboBoxWdmInput.SelectedIndex = index;
        }

        if (!string.IsNullOrEmpty(settings.WdmOutputDevice))
        {
            int index = comboBoxWdmOutput.FindStringExact(settings.WdmOutputDevice);
            if (index >= 0)
                comboBoxWdmOutput.SelectedIndex = index;
        }

        if (!string.IsNullOrEmpty(settings.AsioDevice))
        {
            int index = comboBoxAsioDevice.FindStringExact(settings.AsioDevice);
            if (index >= 0)
                comboBoxAsioDevice.SelectedIndex = index;
        }

        // Imposta buffer
        trackBarBuffer.Value = settings.BufferMs;
        UpdateBufferLabel();

        // Imposta debug mode
        checkBoxEnableDebug.Checked = settings.EnableDebug;

        // Imposta visualizzazione log
        checkBoxShowLog.Checked = settings.ShowLog;

        // Imposta noise gate
        checkBoxGateEnabled.Checked = settings.GateEnabled;
        trackBarGateThreshold.Value = settings.GateThresholdDb;
        trackBarGateAttack.Value = settings.GateAttackMs;
        trackBarGateRelease.Value = settings.GateReleaseMs;
        UpdateGateLabels();
    }

    private void CreateChannelMappingControls()
    {
        panelChannelMappings.Controls.Clear();
        channelLabels.Clear();
        asioInputChannelControls.Clear();
        asioOutputChannelControls.Clear();

        // Header
        Label headerChannel = new Label
        {
            Text = "Channel",
            Location = new Point(10, 5),
            Width = 120,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = Color.White
        };
        Label headerInput = new Label
        {
            Text = "ASIO Input Ch",
            Location = new Point(140, 5),
            Width = 120,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = Color.White
        };
        Label headerOutput = new Label
        {
            Text = "ASIO Output Ch",
            Location = new Point(270, 5),
            Width = 120,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = Color.White
        };

        panelChannelMappings.Controls.Add(headerChannel);
        panelChannelMappings.Controls.Add(headerInput);
        panelChannelMappings.Controls.Add(headerOutput);

        // Crea controlli per ogni canale (1-8)
        for (int i = 0; i < 8; i++)
        {
            int yPos = 35 + (i * 35);
            var channel = settings.Channels[i];

            // Label canale - use custom channel name
            Label lblChannel = new Label
            {
                Text = channel.ChannelName,
                Location = new Point(10, yPos + 5),
                Width = 120,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                ForeColor = Color.LightGray
            };

            // NumericUpDown per ASIO Input Channel (mostrato come 1-based)
            NumericUpDown numInput = new NumericUpDown
            {
                Location = new Point(140, yPos),
                Width = 100,
                Height = 25,
                Minimum = 1,
                Maximum = Math.Max(1, asioInputChannelCount),
                Value = Math.Min(channel.AsioInputChannel + 1, Math.Max(1, asioInputChannelCount)),
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.White
            };

            // NumericUpDown per ASIO Output Channel (mostrato come 1-based)
            NumericUpDown numOutput = new NumericUpDown
            {
                Location = new Point(270, yPos),
                Width = 100,
                Height = 25,
                Minimum = 1,
                Maximum = Math.Max(1, asioOutputChannelCount),
                Value = Math.Min(channel.AsioOutputChannel + 1, Math.Max(1, asioOutputChannelCount)),
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.White
            };

            panelChannelMappings.Controls.Add(lblChannel);
            panelChannelMappings.Controls.Add(numInput);
            panelChannelMappings.Controls.Add(numOutput);

            channelLabels.Add(lblChannel);
            asioInputChannelControls.Add(numInput);
            asioOutputChannelControls.Add(numOutput);
        }
    }

    private void comboBoxAsioDevice_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (comboBoxAsioDevice.SelectedIndex < 0) return;

        try
        {
            string driver = comboBoxAsioDevice.SelectedItem?.ToString() ?? "";
            using (var tempAsio = new AsioOut(driver))
            {
                asioInputChannelCount = tempAsio.DriverInputChannelCount;
                asioOutputChannelCount = tempAsio.DriverOutputChannelCount;

                // Ricrea i controlli di mapping con i contatori aggiornati
                CreateChannelMappingControls();
            }
        }
        catch { }
    }

    private void buttonAsioInfo_Click(object sender, EventArgs e)
    {
        if (comboBoxAsioDevice.SelectedIndex < 0) return;

        try
        {
            string driver = comboBoxAsioDevice.SelectedItem?.ToString() ?? "";
            using (var tempAsio = new AsioOut(driver))
            {
                int inputCh = tempAsio.DriverInputChannelCount;
                int outputCh = tempAsio.DriverOutputChannelCount;
                MessageBox.Show($"Driver: {driver}\n\nInput channels: {inputCh}\nOutput channels: {outputCh}",
                    "ASIO Driver Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void trackBarBuffer_Scroll(object sender, EventArgs e)
    {
        UpdateBufferLabel();
    }

    private void UpdateBufferLabel()
    {
        int bufferMs = trackBarBuffer.Value;
        labelBufferValue.Text = $"{bufferMs} ms";

        // Cambia colore in base alla dimensione del buffer
        if (bufferMs < 30)
            labelBufferValue.ForeColor = System.Drawing.Color.Red;
        else if (bufferMs < 100)
            labelBufferValue.ForeColor = System.Drawing.Color.Orange;
        else if (bufferMs < 200)
            labelBufferValue.ForeColor = System.Drawing.Color.Green;
        else
            labelBufferValue.ForeColor = System.Drawing.Color.Blue;
    }

    private void UpdateGateLabels()
    {
        labelGateThresholdValue.Text = $"{trackBarGateThreshold.Value} dB";
        labelGateAttackValue.Text = $"{trackBarGateAttack.Value} ms";
        labelGateReleaseValue.Text = $"{trackBarGateRelease.Value} ms";
    }

    private void trackBarGateThreshold_Scroll(object sender, EventArgs e)
    {
        UpdateGateLabels();
    }

    private void trackBarGateAttack_Scroll(object sender, EventArgs e)
    {
        UpdateGateLabels();
    }

    private void trackBarGateRelease_Scroll(object sender, EventArgs e)
    {
        UpdateGateLabels();
    }

    private void buttonOK_Click(object sender, EventArgs e)
    {
        // Salva le impostazioni
        settings.WdmInputDevice = comboBoxWdmInput.SelectedItem?.ToString() ?? "";
        settings.WdmOutputDevice = comboBoxWdmOutput.SelectedItem?.ToString() ?? "";
        settings.AsioDevice = comboBoxAsioDevice.SelectedItem?.ToString() ?? "";
        settings.BufferMs = trackBarBuffer.Value;
        settings.EnableDebug = checkBoxEnableDebug.Checked;
        settings.ShowLog = checkBoxShowLog.Checked;

        // Salva parametri noise gate
        settings.GateEnabled = checkBoxGateEnabled.Checked;
        settings.GateThresholdDb = trackBarGateThreshold.Value;
        settings.GateAttackMs = trackBarGateAttack.Value;
        settings.GateReleaseMs = trackBarGateRelease.Value;

        // Salva i mapping dei canali (converte da 1-based UI a 0-based interno)
        for (int i = 0; i < 8; i++)
        {
            settings.Channels[i].AsioInputChannel = (int)asioInputChannelControls[i].Value - 1;
            settings.Channels[i].AsioOutputChannel = (int)asioOutputChannelControls[i].Value - 1;
        }

        // Salva immediatamente su disco
        try
        {
            settings.Save();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving settings: {ex.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        this.DialogResult = DialogResult.OK;
        this.Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
        this.DialogResult = DialogResult.Cancel;
        this.Close();
    }
}
