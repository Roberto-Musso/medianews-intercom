using System;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AudioWdmToAsio;

/// <summary>
/// HTTP server per controllare l'intercom via web
/// </summary>
public class WebServer
{
    private HttpListener? listener;
    private readonly IntercomSettings settings;
    private readonly Action<int> toggleTalkCallback;
    private readonly Action<int> toggleListenCallback;
    private readonly Action<int, int> setInputLevelCallback;
    private readonly Action<int, int> setOutputLevelCallback;
    private readonly Func<float> getVuLevelCallback;
    private bool isRunning = false;
    private readonly int port;

    public WebServer(
        int port,
        IntercomSettings settings,
        Action<int> toggleTalkCallback,
        Action<int> toggleListenCallback,
        Action<int, int> setInputLevelCallback,
        Action<int, int> setOutputLevelCallback,
        Func<float> getVuLevelCallback)
    {
        this.port = port;
        this.settings = settings;
        this.toggleTalkCallback = toggleTalkCallback;
        this.toggleListenCallback = toggleListenCallback;
        this.setInputLevelCallback = setInputLevelCallback;
        this.setOutputLevelCallback = setOutputLevelCallback;
        this.getVuLevelCallback = getVuLevelCallback;
    }

    public void Start()
    {
        if (isRunning) return;

        // Try to add firewall rule first
        TryAddFirewallRule();

        listener = new HttpListener();

        try
        {
            // Try to listen on all interfaces (requires admin privileges or netsh url reservation)
            listener.Prefixes.Add($"http://+:{port}/");
            listener.Start();

            // Get local IP address for logging
            string localIP = GetLocalIPAddress();
            Console.WriteLine($"======================================");
            Console.WriteLine($"WEB SERVER STARTED SUCCESSFULLY");
            Console.WriteLine($"======================================");
            Console.WriteLine($"Local access:   http://localhost:{port}/");
            Console.WriteLine($"Network access: http://{localIP}:{port}/");
            Console.WriteLine($"======================================");
            Console.WriteLine($"To access from other devices:");
            Console.WriteLine($"  1. Make sure this PC allows connections on port {port}");
            Console.WriteLine($"  2. Connect to: http://{localIP}:{port}/");
            Console.WriteLine($"======================================");
        }
        catch (HttpListenerException ex)
        {
            // If failed (likely due to permissions), fall back to localhost only
            Console.WriteLine($"======================================");
            Console.WriteLine($"WARNING: Limited Network Access");
            Console.WriteLine($"======================================");
            Console.WriteLine($"Could not start server on all interfaces.");
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine();
            Console.WriteLine("TO ENABLE NETWORK ACCESS, run these commands as Administrator:");
            Console.WriteLine($"  1. netsh http add urlacl url=http://+:{port}/ user=Everyone");
            Console.WriteLine($"  2. netsh advfirewall firewall add rule name=\"Intercom Web {port}\" dir=in action=allow protocol=TCP localport={port}");
            Console.WriteLine();
            Console.WriteLine("OR restart this application as Administrator.");
            Console.WriteLine($"======================================");

            listener.Prefixes.Clear();
            listener.Prefixes.Add($"http://localhost:{port}/");
            listener.Prefixes.Add($"http://127.0.0.1:{port}/");

            try
            {
                listener.Start();
                Console.WriteLine($"Web server started on http://localhost:{port}/ (local only)");
            }
            catch (Exception ex2)
            {
                Console.WriteLine($"Error starting web server: {ex2.Message}");
                listener = null;
                return;
            }
        }

        isRunning = true;
        Task.Run(async () => await HandleRequests());
    }

    private void TryAddFirewallRule()
    {
        try
        {
            // Try to add firewall rule using netsh
            var processInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "netsh",
                Arguments = $"advfirewall firewall add rule name=\"MediaNews Intercom Web {port}\" dir=in action=allow protocol=TCP localport={port}",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                Verb = "runas" // Request admin privileges
            };

            using (var process = System.Diagnostics.Process.Start(processInfo))
            {
                if (process != null)
                {
                    process.WaitForExit(3000); // Wait max 3 seconds
                    if (process.ExitCode == 0)
                    {
                        Console.WriteLine($"Firewall rule added successfully for port {port}");
                    }
                }
            }
        }
        catch
        {
            // Silently fail - firewall rule is optional
            // User can add it manually if needed
        }
    }

    private string GetLocalIPAddress()
    {
        try
        {
            var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
        }
        catch
        {
            // Ignore errors
        }
        return "localhost";
    }

    public void Stop()
    {
        if (!isRunning) return;

        isRunning = false;
        listener?.Stop();
        listener?.Close();
    }

    private async Task HandleRequests()
    {
        while (isRunning && listener != null)
        {
            try
            {
                var context = await listener.GetContextAsync();
                _ = Task.Run(() => ProcessRequest(context));
            }
            catch (HttpListenerException)
            {
                // Server stopped
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WebServer error: {ex.Message}");
            }
        }
    }

    private void ProcessRequest(HttpListenerContext context)
    {
        try
        {
            var request = context.Request;
            var response = context.Response;

            // Enable CORS
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");

            if (request.HttpMethod == "OPTIONS")
            {
                response.StatusCode = 200;
                response.Close();
                return;
            }

            var path = request.Url?.AbsolutePath ?? "/";

            // Serve HTML page
            if (path == "/" || path == "/index.html")
            {
                ServeHtmlPage(response);
                return;
            }

            // API endpoints
            if (path.StartsWith("/api/"))
            {
                HandleApiRequest(path, request, response);
                return;
            }

            // 404
            response.StatusCode = 404;
            response.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Request processing error: {ex.Message}");
            try
            {
                context.Response.StatusCode = 500;
                context.Response.Close();
            }
            catch { }
        }
    }

    private void HandleApiRequest(string path, HttpListenerRequest request, HttpListenerResponse response)
    {
        try
        {
            // GET /api/status - Get current state
            if (path == "/api/status" && request.HttpMethod == "GET")
            {
                var status = new
                {
                    channels = settings.Channels.Select(ch => new
                    {
                        number = ch.ChannelNumber,
                        name = ch.ChannelName,
                        isTalkActive = ch.IsTalkActive,
                        isListenActive = ch.IsListenActive,
                        inputLevel = ch.InputLevel,
                        outputLevel = ch.OutputLevel
                    }).ToList(),
                    vuLevel = getVuLevelCallback()
                };

                SendJsonResponse(response, status);
                return;
            }

            // POST /api/channel/{id}/talk - Toggle talk
            if (path.StartsWith("/api/channel/") && path.EndsWith("/talk") && request.HttpMethod == "POST")
            {
                var channelId = ExtractChannelId(path);
                if (channelId >= 0 && channelId < 8)
                {
                    toggleTalkCallback(channelId);
                    SendJsonResponse(response, new { success = true });
                    return;
                }
            }

            // POST /api/channel/{id}/listen - Toggle listen
            if (path.StartsWith("/api/channel/") && path.EndsWith("/listen") && request.HttpMethod == "POST")
            {
                var channelId = ExtractChannelId(path);
                if (channelId >= 0 && channelId < 8)
                {
                    toggleListenCallback(channelId);
                    SendJsonResponse(response, new { success = true });
                    return;
                }
            }

            // POST /api/channel/{id}/input-level - Set input level
            if (path.StartsWith("/api/channel/") && path.EndsWith("/input-level") && request.HttpMethod == "POST")
            {
                var channelId = ExtractChannelId(path);
                var body = ReadRequestBody(request);
                var data = JsonSerializer.Deserialize<JsonElement>(body);

                if (channelId >= 0 && channelId < 8 && data.TryGetProperty("value", out var valueElement))
                {
                    var value = valueElement.GetInt32();
                    setInputLevelCallback(channelId, value);
                    SendJsonResponse(response, new { success = true });
                    return;
                }
            }

            // POST /api/channel/{id}/output-level - Set output level
            if (path.StartsWith("/api/channel/") && path.EndsWith("/output-level") && request.HttpMethod == "POST")
            {
                var channelId = ExtractChannelId(path);
                var body = ReadRequestBody(request);
                var data = JsonSerializer.Deserialize<JsonElement>(body);

                if (channelId >= 0 && channelId < 8 && data.TryGetProperty("value", out var valueElement))
                {
                    var value = valueElement.GetInt32();
                    setOutputLevelCallback(channelId, value);
                    SendJsonResponse(response, new { success = true });
                    return;
                }
            }

            response.StatusCode = 404;
            response.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"API error: {ex.Message}");
            response.StatusCode = 500;
            SendJsonResponse(response, new { error = ex.Message });
        }
    }

    private int ExtractChannelId(string path)
    {
        // Extract channel ID from path like "/api/channel/0/talk"
        var parts = path.Split('/');
        if (parts.Length >= 4 && int.TryParse(parts[3], out var id))
        {
            return id;
        }
        return -1;
    }

    private string ReadRequestBody(HttpListenerRequest request)
    {
        using var reader = new System.IO.StreamReader(request.InputStream, request.ContentEncoding);
        return reader.ReadToEnd();
    }

    private void SendJsonResponse(HttpListenerResponse response, object data)
    {
        response.ContentType = "application/json";
        var json = JsonSerializer.Serialize(data);
        var buffer = Encoding.UTF8.GetBytes(json);
        response.ContentLength64 = buffer.Length;
        response.OutputStream.Write(buffer, 0, buffer.Length);
        response.Close();
    }

    private void ServeHtmlPage(HttpListenerResponse response)
    {
        var html = GetHtmlContent();
        var buffer = Encoding.UTF8.GetBytes(html);
        response.ContentType = "text/html; charset=utf-8";
        response.ContentLength64 = buffer.Length;
        response.OutputStream.Write(buffer, 0, buffer.Length);
        response.Close();
    }

    private string GetHtmlContent()
    {
        return @"<!DOCTYPE html>
<html lang=""it"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>MediaNews Intercom - Remote Control</title>
    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: #1e1e1e;
            color: #fff;
            padding: 20px;
        }

        header {
            background: #141414;
            padding: 20px;
            border-radius: 8px;
            margin-bottom: 20px;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        h1 {
            font-size: 24px;
            color: #fff;
        }

        .vu-meter-container {
            flex: 1;
            max-width: 400px;
            margin: 0 30px;
        }

        .vu-label {
            font-size: 12px;
            color: #aaa;
            margin-bottom: 5px;
        }

        .vu-meter {
            height: 30px;
            background: #1e1e1e;
            border: 1px solid #505050;
            border-radius: 4px;
            position: relative;
            overflow: hidden;
        }

        .vu-meter-fill {
            height: 100%;
            background: linear-gradient(to right, #00c800 0%, #64ff00 40%, #ffff00 70%, #ff9600 85%, #ff0000 100%);
            width: 0%;
            transition: width 0.1s ease-out;
        }

        .channels-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
            gap: 15px;
            max-width: 1920px;
            margin: 0 auto;
        }

        .channel {
            background: #2d2d30;
            border: 1px solid #3e3e42;
            border-radius: 8px;
            padding: 15px;
        }

        .channel-name {
            text-align: center;
            font-size: 14px;
            font-weight: bold;
            margin-bottom: 12px;
            padding: 8px;
            background: #1e2228;
            border-radius: 4px;
        }

        .button-group {
            display: flex;
            gap: 10px;
            margin-bottom: 15px;
        }

        button {
            flex: 1;
            padding: 15px;
            border: 2px solid #505050;
            border-radius: 6px;
            font-size: 12px;
            font-weight: bold;
            cursor: pointer;
            transition: all 0.2s;
            background: #3c3c3c;
            color: #fff;
        }

        button:hover {
            transform: scale(1.05);
        }

        button.talk-active {
            background: #ff4444;
            border-color: #ff6666;
            color: #fff;
        }

        button.listen-active {
            background: #00aa00;
            border-color: #00cc00;
            color: #fff;
        }

        .knob-container {
            margin-bottom: 12px;
        }

        .knob-label {
            text-align: center;
            font-size: 11px;
            color: #aaa;
            margin-bottom: 8px;
        }

        .knob-wrapper {
            display: flex;
            align-items: center;
            gap: 10px;
        }

        .knob {
            width: 60px;
            height: 60px;
            margin: 0 auto;
        }

        .knob-value {
            font-size: 14px;
            font-weight: bold;
            color: #00d4ff;
            min-width: 35px;
            text-align: center;
        }

        .knob-value.output {
            color: #ff9600;
        }

        input[type=""range""] {
            flex: 1;
            -webkit-appearance: none;
            appearance: none;
            height: 4px;
            border-radius: 2px;
            background: #3c3c3c;
            outline: none;
        }

        input[type=""range""]::-webkit-slider-thumb {
            -webkit-appearance: none;
            appearance: none;
            width: 16px;
            height: 16px;
            border-radius: 50%;
            background: #00d4ff;
            cursor: pointer;
        }

        input[type=""range""].output::-webkit-slider-thumb {
            background: #ff9600;
        }

        input[type=""range""]::-moz-range-thumb {
            width: 16px;
            height: 16px;
            border-radius: 50%;
            background: #00d4ff;
            cursor: pointer;
            border: none;
        }

        .status {
            position: fixed;
            bottom: 20px;
            right: 20px;
            background: #2d2d30;
            padding: 10px 20px;
            border-radius: 6px;
            font-size: 12px;
        }

        .status.connected {
            border-left: 4px solid #00aa00;
        }

        .status.disconnected {
            border-left: 4px solid #ff4444;
        }

        @media (max-width: 768px) {
            .channels-grid {
                grid-template-columns: 1fr;
            }

            header {
                flex-direction: column;
                gap: 15px;
            }

            .vu-meter-container {
                width: 100%;
                max-width: none;
                margin: 0;
            }
        }
    </style>
</head>
<body>
    <header>
        <h1>MediaNews Intercom - Remote Control</h1>
        <div class=""vu-meter-container"">
            <div class=""vu-label"">Input Level</div>
            <div class=""vu-meter"">
                <div class=""vu-meter-fill"" id=""vuMeter""></div>
            </div>
        </div>
        <div class=""status disconnected"" id=""status"">Connecting...</div>
    </header>

    <div class=""channels-grid"" id=""channelsGrid""></div>

    <script>
        const API_BASE = window.location.origin;
        let channels = [];

        async function fetchStatus() {
            try {
                const response = await fetch(`${API_BASE}/api/status`);
                const data = await response.json();

                channels = data.channels;
                updateUI();
                updateVUMeter(data.vuLevel);
                updateStatus(true);
            } catch (error) {
                console.error('Error fetching status:', error);
                updateStatus(false);
            }
        }

        function updateUI() {
            const grid = document.getElementById('channelsGrid');

            if (grid.children.length === 0) {
                // Create channels on first load
                channels.forEach(channel => {
                    const channelDiv = createChannelElement(channel);
                    grid.appendChild(channelDiv);
                });
            } else {
                // Update existing channels
                channels.forEach((channel, index) => {
                    updateChannelElement(index, channel);
                });
            }
        }

        function createChannelElement(channel) {
            const div = document.createElement('div');
            div.className = 'channel';
            div.id = `channel-${channel.number - 1}`;

            div.innerHTML = `
                <div class=""channel-name"">${channel.name}</div>
                <div class=""button-group"">
                    <button class=""talk-btn"" onclick=""toggleTalk(${channel.number - 1})"">TALK</button>
                    <button class=""listen-btn"" onclick=""toggleListen(${channel.number - 1})"">LISTEN</button>
                </div>
                <div class=""knob-container"">
                    <div class=""knob-label"">Input Level</div>
                    <div class=""knob-wrapper"">
                        <input type=""range"" min=""0"" max=""130"" value=""${channel.inputLevel}""
                            class=""input-level"" onchange=""setInputLevel(${channel.number - 1}, this.value)"">
                        <span class=""knob-value input-value"">${channel.inputLevel}</span>
                    </div>
                </div>
                <div class=""knob-container"">
                    <div class=""knob-label"">Output Level</div>
                    <div class=""knob-wrapper"">
                        <input type=""range"" min=""0"" max=""130"" value=""${channel.outputLevel}""
                            class=""output-level output"" onchange=""setOutputLevel(${channel.number - 1}, this.value)"">
                        <span class=""knob-value output-value output"">${channel.outputLevel}</span>
                    </div>
                </div>
            `;

            return div;
        }

        function updateChannelElement(index, channel) {
            const channelDiv = document.getElementById(`channel-${index}`);
            if (!channelDiv) return;

            // Update name
            const nameEl = channelDiv.querySelector('.channel-name');
            if (nameEl) nameEl.textContent = channel.name;

            // Update talk button
            const talkBtn = channelDiv.querySelector('.talk-btn');
            if (talkBtn) {
                if (channel.isTalkActive) {
                    talkBtn.classList.add('talk-active');
                } else {
                    talkBtn.classList.remove('talk-active');
                }
            }

            // Update listen button
            const listenBtn = channelDiv.querySelector('.listen-btn');
            if (listenBtn) {
                if (channel.isListenActive) {
                    listenBtn.classList.add('listen-active');
                } else {
                    listenBtn.classList.remove('listen-active');
                }
            }

            // Update input level
            const inputSlider = channelDiv.querySelector('.input-level');
            const inputValue = channelDiv.querySelector('.input-value');
            if (inputSlider && inputSlider.value != channel.inputLevel) {
                inputSlider.value = channel.inputLevel;
            }
            if (inputValue) inputValue.textContent = channel.inputLevel;

            // Update output level
            const outputSlider = channelDiv.querySelector('.output-level');
            const outputValue = channelDiv.querySelector('.output-value');
            if (outputSlider && outputSlider.value != channel.outputLevel) {
                outputSlider.value = channel.outputLevel;
            }
            if (outputValue) outputValue.textContent = channel.outputLevel;
        }

        function updateVUMeter(level) {
            const vuMeter = document.getElementById('vuMeter');
            if (vuMeter) {
                const percentage = Math.min(100, level * 100);
                vuMeter.style.width = `${percentage}%`;
            }
        }

        function updateStatus(connected) {
            const statusEl = document.getElementById('status');
            if (connected) {
                statusEl.className = 'status connected';
                statusEl.textContent = 'Connected';
            } else {
                statusEl.className = 'status disconnected';
                statusEl.textContent = 'Disconnected';
            }
        }

        async function toggleTalk(channelId) {
            try {
                await fetch(`${API_BASE}/api/channel/${channelId}/talk`, { method: 'POST' });
                fetchStatus();
            } catch (error) {
                console.error('Error toggling talk:', error);
            }
        }

        async function toggleListen(channelId) {
            try {
                await fetch(`${API_BASE}/api/channel/${channelId}/listen`, { method: 'POST' });
                fetchStatus();
            } catch (error) {
                console.error('Error toggling listen:', error);
            }
        }

        async function setInputLevel(channelId, value) {
            try {
                await fetch(`${API_BASE}/api/channel/${channelId}/input-level`, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ value: parseInt(value) })
                });

                // Update display immediately for smooth feedback
                const channelDiv = document.getElementById(`channel-${channelId}`);
                const inputValue = channelDiv?.querySelector('.input-value');
                if (inputValue) inputValue.textContent = value;
            } catch (error) {
                console.error('Error setting input level:', error);
            }
        }

        async function setOutputLevel(channelId, value) {
            try {
                await fetch(`${API_BASE}/api/channel/${channelId}/output-level`, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ value: parseInt(value) })
                });

                // Update display immediately for smooth feedback
                const channelDiv = document.getElementById(`channel-${channelId}`);
                const outputValue = channelDiv?.querySelector('.output-value');
                if (outputValue) outputValue.textContent = value;
            } catch (error) {
                console.error('Error setting output level:', error);
            }
        }

        // Poll for updates
        fetchStatus();
        setInterval(fetchStatus, 200); // Update every 200ms
    </script>
</body>
</html>";
    }
}
