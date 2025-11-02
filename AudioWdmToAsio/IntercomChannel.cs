namespace AudioWdmToAsio;

/// <summary>
/// Rappresenta un singolo canale dell'intercom (1-8)
/// </summary>
public class IntercomChannel
{
    /// <summary>
    /// Numero del canale (1-8)
    /// </summary>
    public int ChannelNumber { get; set; }

    /// <summary>
    /// Nome personalizzato del canale
    /// </summary>
    public string ChannelName { get; set; } = "";

    /// <summary>
    /// Indica se il pulsante Talk è attivo per questo canale
    /// </summary>
    public bool IsTalkActive { get; set; }

    /// <summary>
    /// Indica se il pulsante Listen è attivo per questo canale
    /// </summary>
    public bool IsListenActive { get; set; }

    /// <summary>
    /// Livello di input (0-130, default 100)
    /// Controlla il volume dell'audio in arrivo da ASIO input
    /// </summary>
    public int InputLevel { get; set; } = 100;

    /// <summary>
    /// Livello di output (0-130, default 100)
    /// Controlla il volume dell'audio in uscita verso ASIO output quando Talk è attivo
    /// </summary>
    public int OutputLevel { get; set; } = 100;

    /// <summary>
    /// Canale fisico ASIO da usare per l'input (0-based)
    /// </summary>
    public int AsioInputChannel { get; set; }

    /// <summary>
    /// Canale fisico ASIO da usare per l'output (0-based)
    /// </summary>
    public int AsioOutputChannel { get; set; }

    /// <summary>
    /// Converte il livello input (0-130) in gain lineare (0.0-1.3)
    /// </summary>
    public float GetInputGain()
    {
        return InputLevel / 100.0f;
    }

    /// <summary>
    /// Converte il livello output (0-130) in gain lineare (0.0-1.3)
    /// </summary>
    public float GetOutputGain()
    {
        return OutputLevel / 100.0f;
    }

    public IntercomChannel(int channelNumber)
    {
        ChannelNumber = channelNumber;
        ChannelName = $"Channel {channelNumber}";
        IsTalkActive = false;
        IsListenActive = false;
        InputLevel = 100;
        OutputLevel = 100;

        // Default: canale software corrisponde al canale fisico ASIO (0-based)
        AsioInputChannel = channelNumber - 1;
        AsioOutputChannel = channelNumber - 1;
    }
}
