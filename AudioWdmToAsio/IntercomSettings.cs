using System.Text.Json;

namespace AudioWdmToAsio;

/// <summary>
/// Configurazione globale dell'applicazione intercom
/// </summary>
public class IntercomSettings
{
    /// <summary>
    /// Nome del dispositivo WDM di input (microfono)
    /// </summary>
    public string WdmInputDevice { get; set; } = "";

    /// <summary>
    /// Nome del dispositivo WDM di output (altoparlante)
    /// </summary>
    public string WdmOutputDevice { get; set; } = "";

    /// <summary>
    /// Nome del driver ASIO selezionato
    /// </summary>
    public string AsioDevice { get; set; } = "";

    /// <summary>
    /// Dimensione del buffer in millisecondi (10-500)
    /// </summary>
    public int BufferMs { get; set; } = 100;

    /// <summary>
    /// Gli 8 canali dell'intercom
    /// </summary>
    public List<IntercomChannel> Channels { get; set; }

    /// <summary>
    /// Indica se il sistema è attualmente in streaming
    /// </summary>
    public bool IsStreaming { get; set; }

    /// <summary>
    /// Abilita la visualizzazione di informazioni di debug nella status bar
    /// </summary>
    public bool EnableDebug { get; set; } = false;

    /// <summary>
    /// Mostra la finestra di log nella parte inferiore dell'applicazione
    /// </summary>
    public bool ShowLog { get; set; } = true;

    /// <summary>
    /// Abilita il noise gate sull'ingresso del microfono
    /// </summary>
    public bool GateEnabled { get; set; } = false;

    /// <summary>
    /// Soglia del gate in dB (-60 a 0)
    /// </summary>
    public int GateThresholdDb { get; set; } = -40;

    /// <summary>
    /// Tempo di attacco del gate in millisecondi (1-100)
    /// </summary>
    public int GateAttackMs { get; set; } = 10;

    /// <summary>
    /// Tempo di rilascio del gate in millisecondi (10-1000)
    /// </summary>
    public int GateReleaseMs { get; set; } = 200;

    public IntercomSettings()
    {
        Channels = new List<IntercomChannel>();

        // Inizializza gli 8 canali
        for (int i = 1; i <= 8; i++)
        {
            Channels.Add(new IntercomChannel(i));
        }

        IsStreaming = false;
    }

    /// <summary>
    /// Ottiene un canale specifico per numero (1-8)
    /// </summary>
    public IntercomChannel? GetChannel(int channelNumber)
    {
        return Channels.FirstOrDefault(c => c.ChannelNumber == channelNumber);
    }

    /// <summary>
    /// Ottiene tutti i canali con Talk attivo
    /// </summary>
    public List<IntercomChannel> GetActiveTalkChannels()
    {
        return Channels.Where(c => c.IsTalkActive).ToList();
    }

    /// <summary>
    /// Ottiene tutti i canali con Listen attivo
    /// </summary>
    public List<IntercomChannel> GetActiveListenChannels()
    {
        return Channels.Where(c => c.IsListenActive).ToList();
    }

    /// <summary>
    /// Salva le impostazioni su file JSON
    /// </summary>
    public void Save(string filePath = "intercom_settings.json")
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(this, options);
            File.WriteAllText(filePath, json);
        }
        catch (Exception ex)
        {
            throw new Exception($"Errore nel salvataggio delle impostazioni: {ex.Message}");
        }
    }

    /// <summary>
    /// Carica le impostazioni da file JSON
    /// </summary>
    public static IntercomSettings? Load(string filePath = "intercom_settings.json")
    {
        try
        {
            if (!File.Exists(filePath))
                return null;

            string json = File.ReadAllText(filePath);
            var settings = JsonSerializer.Deserialize<IntercomSettings>(json);

            // Assicurati che IsStreaming e gli stati Talk/Listen siano false al caricamento
            if (settings != null)
            {
                settings.IsStreaming = false;

                // Reset degli stati Talk/Listen
                foreach (var channel in settings.Channels)
                {
                    channel.IsTalkActive = false;
                    channel.IsListenActive = false;
                }
            }

            return settings;
        }
        catch (Exception ex)
        {
            throw new Exception($"Errore nel caricamento delle impostazioni: {ex.Message}");
        }
    }
}
