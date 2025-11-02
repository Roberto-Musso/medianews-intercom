# MediaNews Intercom - Software-Based 8 Channel Professional Audio System

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/)
[![Platform](https://img.shields.io/badge/Platform-Windows-0078D4)](https://www.microsoft.com/windows)

8-channel audio intercom system with bidirectional communication between standard WDM microphone/speaker and 8 channels of a professional ASIO audio interface, featuring remote web control.

![Version](https://img.shields.io/badge/version-1.1.0-blue)

## Key Features

### Complete Intercom System
- **8 Independent Channels**: Each channel has complete routing, input/output levels, individual Talk and Listen
- **Individual Talk Buttons**: Each channel can send microphone to its own ASIO output independently
- **Individual Listen Buttons**: Each channel can be monitored independently
- **Integrated Audio Mixer**: Automatically combines active Listen channels
- **Granular Control**: Activate/deactivate Talk and Listen per specific channel

### Audio Paths

**TALK PATH** (Microphone → ASIO):
```
[WDM Microphone] → [Channel N Talk Button] → [Channel N Output Level Control] → [ASIO Output Channel N]
```
Each channel independently decides whether to receive the microphone

**LISTEN PATH** (ASIO → Speaker):
```
[ASIO Input Channel N] → [Channel N Input Level Control] → [Channel N Listen Button] → [Audio Mixer] → [WDM Speaker]
```
Multiple Listen channels can be active simultaneously (automatic mixing)

### Audio Controls
- **Input Level per channel**: 0-130 (0 = silence, 100 = unity, 130 = +30% amplification)
- **Output Level per channel**: 0-130 (same range)
- **Automatic mixing**: Active Listen channels are mixed together without clipping

### User Interface
- **8 Talk Buttons**: One per channel, turn RED when active
- **8 Listen Buttons**: One per channel, turn CYAN when active
- **Channel Strips**: 2x4 layout with all controls visible per channel
- **16 Level Controls**: Input Level and Output Level for each channel (0-130)
- **Log Console**: Real-time monitoring of all operations
- **Visual Feedback**: Color coding for immediate status (Red=Talk, Cyan=Listen, Gray=Off)

## Requirements

### Software
- Windows 10/11 (64-bit)
- .NET 8.0 Runtime or higher
- ASIO driver installed (e.g., ASIO4ALL, FlexASIO, or your professional audio interface driver)

### Hardware
- Microphone (WDM/WASAPI input device)
- Speaker/Headphones (WDM/WASAPI output device)
- ASIO audio interface with at least 8 inputs and 8 outputs

## Download and Installation

### Option 1: Windows Installer (Recommended)

1. Download the latest installer from the [Releases page](https://github.com/Roberto-Musso/medianews-intercom/releases)
2. Run `MediaNews-Intercom-Setup-v1.1.0.exe`
3. Follow the installation wizard
4. The installer includes:
   - ✅ Complete application
   - ✅ .NET 8.0 Runtime (if needed)
   - ✅ Desktop icon
   - ✅ Start Menu shortcut
   - ✅ Script to enable network web access

### Option 2: Portable Execution (no installation)

1. Download the ZIP package from the [Releases page](https://github.com/Roberto-Musso/medianews-intercom/releases)
2. Extract to a folder
3. Run `MediaNews-Intercom.exe`

### Option 3: Build from Source

```bash
# Clone the repository
git clone https://github.com/Roberto-Musso/medianews-intercom.git
cd medianews-intercom

# Build the project
dotnet build

# Run the application
dotnet run --project AudioWdmToAsio/AudioWdmToAsio.csproj
```

## Quick Start Guide

### 1. Initial Setup

1. **Launch the application**
2. **Click "Settings"** to open the configuration panel
3. **Configure devices**:
   - Select microphone (WDM Input)
   - Select speaker (WDM Output)
   - Select ASIO interface
   - Adjust buffer (recommended: 100ms to start)

4. **Configure ASIO channel mapping**:
   - For each software channel (1-8), choose which physical ASIO channel to use
   - ASIO Input Channel: Where to receive audio from
   - ASIO Output Channel: Where to send audio to
   - Default: Channel 1 → ASIO Ch 0, Channel 2 → ASIO Ch 1, etc.

5. **Click "OK"** to save settings

### 2. Starting the System

1. **Click "START"** in the bottom bar
2. The system starts audio streaming
3. Status becomes "Streaming Active" (green)
4. The system is now ready for Talk and Listen

### 3. Using Talk (Sending Audio)

1. **Click "TALK"** on the channel you want to send your microphone to
2. The button turns RED
3. Your microphone is now sent **only** to that ASIO channel
4. **Adjust Output Level** for that channel to control the sent volume
5. **Activate more TALK** on other channels to send to multiple destinations simultaneously
6. **Click "TALK" again** on the channel to deactivate it (turns gray)

### 4. Using Listen (Receiving Audio)

1. **Click "LISTEN"** on the channel you want to hear
2. The button turns CYAN
3. Audio from that ASIO channel is sent to your speaker
4. **Adjust Input Level** to control the received volume
5. **Activate multiple Listen channels** to hear multiple sources simultaneously
   - The mixer will combine them automatically
6. **Click "LISTEN" again** to deactivate that channel

### 5. Level Adjustment

**Input Level** (0-130):
- Controls the volume of incoming audio from ASIO (for Listen)
- 0 = Total silence
- 100 = Normal volume (default)
- 130 = +30% amplification

**Output Level** (0-130):
- Controls the volume of outgoing audio to ASIO (for Talk)
- Same range as Input Level

### 6. Stopping the System

1. **Click "STOP"** in the bottom bar
2. All channels are deactivated
3. Talk and Listen are reset
4. The system returns to Idle state

## Typical Use Cases

### Scenario 1: Recording Studio
- **Talk**: Producer speaks selectively to specific musicians (channel 1=drummer, 2=guitarist, etc.)
- **Listen**: Producer monitors individual musicians or mix of multiple musicians selectively

### Scenario 2: Broadcast/Podcast
- **Talk**: Host communicates selectively with specific guests (private channel)
- **Listen**: Host hears one or more guests simultaneously (automatic mixer)

### Scenario 3: Theater/Live Sound
- **Talk**: Audio director communicates with specific positions (Talk stage only, lights only, etc.)
- **Listen**: Director hears feedback from selective positions or all together

### Scenario 4: Multi-Point Conferences
- **Talk**: Moderator sends audio to specific rooms (Talk room 1, 3, 5 = only those receive)
- **Listen**: Moderator monitors questions from selected rooms

## Technical Architecture

### Technology Stack
- **Framework**: .NET 8.0 Windows Forms
- **Audio Library**: NAudio 2.2.1
  - NAudio.Asio (ASIO driver management)
  - NAudio.Wasapi (Windows audio capture/playback)
  - NAudio.Core (audio processing)

### Main Components

#### 1. IntercomSettings
Global configuration containing:
- Selected WDM devices
- Selected ASIO driver
- 8 IntercomChannel objects
- Talk and streaming state

#### 2. IntercomChannel
Each channel contains:
- Channel number (1-8)
- Talk state (active/inactive)
- Listen state (active/inactive)
- Input and Output levels (0-130)
- Physical ASIO channel mapping (input and output)

#### 3. Audio Engine
- **WasapiCapture**: Captures from microphone
- **AsioOut**: Manages bidirectional ASIO I/O
- **WasapiOut**: Playback on speaker
- **BufferedWaveProvider**: Buffers to manage latency

### Detailed Audio Flow

**Talk Path**:
1. Microphone captures audio (WasapiCapture)
2. Audio goes into `talkBuffer` only if at least one Talk is active
3. ASIO AudioAvailable reads from `talkBuffer`
4. Distributes **only** to channels with active Talk, with individual gain
5. Channels without active Talk receive silence
6. ASIO hardware receives audio on selected channels

**Listen Path**:
1. ASIO AudioAvailable receives from input channels
2. For each channel with active Listen:
   - Reads from ASIO buffer
   - Applies Input Level gain
   - Adds to mixer
3. Mixer normalizes and avoids clipping
4. Output goes to `speakerBuffer`
5. WasapiOut plays on speaker

### Buffer Management
- **talkBuffer**: Configurable size (10-500ms)
- **speakerBuffer**: Same size, Float32 stereo format
- **DiscardOnBufferOverflow**: Prevents deadlocks
- Thread-safe for real-time operations

## Project Structure

```
AudioWdmToAsio/
├── IntercomChannel.cs        # Single channel data model
├── IntercomSettings.cs       # Global configuration
├── Form1.cs                  # Main intercom logic
├── Form1.Designer.cs         # Main UI (8 channel strips)
├── SettingsForm.cs           # Settings panel logic
├── SettingsForm.Designer.cs  # Settings panel UI
├── Program.cs                # Application entry point
└── AudioWdmToAsio.csproj     # Project configuration
```

## Troubleshooting

### "No ASIO driver found"
- Install an ASIO driver:
  - **ASIO4ALL** (free): http://www.asio4all.org/
  - **FlexASIO** (open source): https://github.com/dechamps/FlexASIO
  - Your audio interface's proprietary driver

### "Please configure devices in Settings first"
- Open Settings and configure all three devices:
  - Microphone (WDM)
  - Speaker (WDM)
  - ASIO Device

### High latency / Echo
- Reduce buffer size in settings (try 50ms or less)
- Close other audio applications
- Check ASIO driver panel settings

### Audio dropouts / Interruptions
- Increase buffer size (try 150ms or more)
- Close background applications
- Verify ASIO driver is configured correctly
- Check CPU performance

### Channel not working
- Verify ASIO channel mapping in Settings
- Check that physical ASIO channel is within driver limits
- Verify Input/Output levels (might be at 0)

### Volume too low/high
- Adjust Input Level for Listen (0-130)
- Adjust Output Level for Talk (0-130)
- Default 100 = unity volume
- Values > 100 amplify but may cause distortion

### "Error starting streaming"
- Verify no other application is using the ASIO driver
- Restart the application
- Check log console for specific details
- Try a different buffer size

## Known Limitations

- The system uses a single ASIO driver for all 8 channels
- Some ASIO drivers have specific buffer size requirements
- Sample rate conversion is not implemented (uses microphone rate)
- Maximum 8 channels (can be extended by modifying code)
- Listen mixer supports up to 8 simultaneous channels

## Possible Future Features

- [ ] VU meters to visualize audio levels in real-time
- [ ] Saveable/loadable presets for different configurations
- [ ] Talkback with sidetone (hear yourself while talking)
- [ ] Recording of intercom sessions
- [ ] Support for more than 8 channels
- [ ] Audio effects (EQ, compressor, gate, etc.)
- [ ] Hotkeys/shortcuts for Talk and Listen
- [ ] MIDI control for external control
- [ ] Network streaming for remote intercom

## Best Practices

### For Low Latency
1. Use small buffer (30-100ms)
2. Close unnecessary applications
3. Use native ASIO drivers instead of emulated
4. Monitor CPU and don't exceed 70%

### For Stability
1. Use medium-large buffer (100-200ms)
2. Test configuration before live use
3. Save working configurations
4. Do soundcheck before important events

### For Audio Quality
1. Use quality audio interfaces
2. Keep levels between 80-120 to avoid distortion
3. Use quality microphones and speakers
4. Avoid feedback loops (don't listen to Talk while it's active)

## Development

### NuGet Dependencies
```xml
<PackageReference Include="NAudio" Version="2.2.1" />
<PackageReference Include="NAudio.Asio" Version="2.2.1" />
```

### Build
```bash
dotnet build AudioWdmToAsio.sln
```

### Debug
Open `AudioWdmToAsio.sln` in Visual Studio 2022 or Visual Studio Code for detailed debugging.

### Extending the System
To add more channels:
1. Modify `IntercomSettings` to create more channels
2. Modify `Form1.Designer.cs` CreateChannelStrips() to create them in the UI
3. Audio logic is already prepared for N channels

## License

This project is released under the MIT License. See the [LICENSE](LICENSE) file for complete details.

Free to use for educational and commercial purposes.

## Contributing

Contributions, issues, and feature requests are welcome!

1. Fork the project
2. Create a branch for your feature (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## Credits

- **NAudio**: https://github.com/naudio/NAudio (Mark Heath)
- **ASIO SDK**: Steinberg Media Technologies GmbH
- **Developed with**: Claude Code (Anthropic)

## Support

For issues, questions, or suggestions:
1. Check the "Troubleshooting" section
2. Review log console for detailed errors
3. Verify ASIO configuration via driver panel
4. Try different buffer sizes

## Changelog

### Version 1.1.0 (Current) - Remote Control & UX Improvements
- 🌐 **Web Remote Control**: Complete web interface for remote control
  - Access from any device on local network
  - Control Talk/Listen for all channels
  - Adjust input/output levels from browser
  - Real-time VU Meter
  - Responsive design for mobile/tablet
- 🖱️ **Enhanced Rotary Controls**:
  - Mouse capture during drag (work even outside channel box)
  - Double-click for direct numeric keyboard input
  - Confirm with ENTER, cancel with ESC
- 🔧 **Automatic Network Setup**:
  - ENABLE_NETWORK_ACCESS.bat script for firewall configuration
  - Improved diagnostic messages
  - Local IP auto-detection
- 📦 **Professional Installer**: Windows setup with all dependencies

### Version 1.0.0 - Intercom System
- **8-Channel Intercom System**: Complete with individual Talk and Listen
- **8 Talk Buttons**: Individual control for microphone send per channel
- **8 Listen Buttons**: Individual control for channel reception
- **Level Controls**: Input and Output Level for each channel (0-130)
- **Audio Mixer**: Automatically combines active Listen channels
- **Settings Panel**: Complete device configuration and channel mapping
- **Professional UI**: Dark theme design with immediate visual feedback
- **Channel Mapping**: Flexible routing between software and hardware ASIO channels
- **Real-time Logging**: Integrated console for system monitoring
- **VU Meters**: Real-time audio level visualization

### Screenshots

<img width="1922" height="572" alt="image" src="https://github.com/user-attachments/assets/6e8ec7e9-0f00-4af8-9d59-e9602071b583" />

<img width="1922" height="572" alt="image" src="https://github.com/user-attachments/assets/56b8402c-3592-48b7-ade9-5de347a7e02f" />

<img width="1124" height="810" alt="image" src="https://github.com/user-attachments/assets/311f50ec-7a1b-4b18-b384-0efdbe68644f" />


---

**MediaNews Intercom v1.1** - Software-Based 8-Channel Communication Platform with Remote Web Control
