# MediaNews Intercom v1.1 - User Manual

## Table of Contents

1. [Introduction](#introduction)
2. [System Requirements](#system-requirements)
3. [Installation](#installation)
4. [First-Time Setup](#first-time-setup)
5. [Understanding the Interface](#understanding-the-interface)
6. [Basic Operations](#basic-operations)
7. [Advanced Features](#advanced-features)
8. [Web Remote Control](#web-remote-control)
9. [Troubleshooting](#troubleshooting)
10. [Best Practices](#best-practices)
11. [FAQ](#faq)

---

## Introduction

MediaNews Intercom is a professional 8-channel audio intercom system designed for broadcast studios, recording facilities, live sound environments, and any professional audio application requiring selective communication between multiple channels.

### Key Features

- **8 Independent Channels**: Each with individual Talk and Listen controls
- **Dual Audio Paths**: Simultaneous Talk (send) and Listen (receive) on each channel
- **ASIO Integration**: Professional low-latency audio interface support
- **Web Remote Control**: Control from any device on your local network
- **Individual Level Controls**: Precise input and output level adjustment (0-130)
- **Real-time VU Meters**: Visual feedback for all channels
- **Flexible Channel Mapping**: Route any software channel to any physical ASIO channel

### What This Manual Covers

This manual will guide you through:
- Installing and configuring the software
- Understanding the user interface
- Operating the Talk and Listen functions
- Adjusting audio levels
- Using the web remote control
- Troubleshooting common issues
- Optimizing performance

---

## System Requirements

### Minimum Requirements

- **Operating System**: Windows 10 (64-bit) or Windows 11
- **Processor**: Intel Core i5 or AMD equivalent (2.5 GHz or faster)
- **RAM**: 4 GB minimum (8 GB recommended)
- **Storage**: 200 MB free disk space
- **Audio Interface**: ASIO-compatible audio device with at least 8 input and 8 output channels

### Required Audio Drivers

You must have an **ASIO driver** installed. Options include:

1. **Native ASIO Driver** (Recommended)
   - Provided by your audio interface manufacturer
   - Offers best performance and lowest latency
   - Examples: RME, Focusrite, MOTU, Universal Audio drivers

2. **ASIO4ALL** (Universal Option)
   - Free ASIO driver for any audio device
   - Download: http://www.asio4all.org/
   - Good for testing and general use

3. **FlexASIO** (Open Source)
   - Modern open-source ASIO driver
   - Download: https://github.com/dechamps/FlexASIO
   - Flexible configuration options

### Network Requirements (for Web Interface)

- **Local Network**: Ethernet or Wi-Fi connection
- **Firewall**: Port 8080 must be allowed (configured during installation)
- **Remote Devices**: Any device with a modern web browser

---

## Installation

### Option 1: Using the Windows Installer (Recommended)

1. **Download** `MediaNews-Intercom-Setup-v1.1.0.exe` from the [Releases page](https://github.com/robertomusso/medianews-intercom/releases)

2. **Run the installer**
   - Double-click the downloaded file
   - If prompted by Windows Defender, click "More info" → "Run anyway"

3. **Follow the installation wizard**:
   - Accept the license agreement (MIT License)
   - Choose installation directory (default: `C:\Program Files\MediaNews Intercom`)
   - Select optional features:
     - ✅ **Desktop shortcut** (recommended)
     - ✅ **Enable network access** (for web interface)

4. **Complete installation**
   - Click "Install"
   - If prompted for administrator privileges, click "Yes"
   - Click "Finish" to launch the application

### Option 2: Portable ZIP Version

1. **Download** `MediaNews-Intercom-v1.1.0-Portable.zip`

2. **Extract** to your preferred location
   - Right-click → "Extract All..."
   - Choose destination folder

3. **Run** `MediaNews-Intercom.exe`

4. **Enable Network Access** (if needed)
   - Run `ENABLE_NETWORK_ACCESS.bat` as Administrator
   - This configures firewall rules for web interface

### Post-Installation: ASIO Driver Setup

If you don't have an ASIO driver:

1. **Install ASIO4ALL**:
   - Download from http://www.asio4all.org/
   - Run installer with default settings
   - Restart your computer

2. **Configure ASIO4ALL** (if using):
   - Open ASIO4ALL settings from system tray
   - Enable your audio interface
   - Set buffer size (start with 512 samples)

---

## First-Time Setup

### Step 1: Launch the Application

- Double-click the desktop shortcut, or
- Start Menu → MediaNews Intercom

### Step 2: Open Settings

1. Click the **"Settings"** button in the bottom-right corner
2. The Settings window will open

### Step 3: Configure Audio Devices

#### Microphone (WDM Input Device)

- **Purpose**: Your talk microphone input
- **Selection**: Choose your microphone from the dropdown
- **Example**: "Microphone (USB Audio Device)"
- **Tip**: Use a dedicated microphone for best quality

#### Speaker (WDM Output Device)

- **Purpose**: Where you'll hear the Listen channels
- **Selection**: Choose your headphones or speakers
- **Example**: "Headphones (USB Audio Device)"
- **Important**: Use headphones to prevent feedback

#### ASIO Device

- **Purpose**: Professional audio interface for the 8 channels
- **Selection**: Choose your ASIO driver
- **Example**: "RME Fireface", "ASIO4ALL", "FlexASIO"
- **Note**: This must be running before other audio applications

#### Buffer Size

- **Purpose**: Balance between latency and stability
- **Range**: 10-500 milliseconds
- **Recommendations**:
  - **Low Latency** (30-50ms): For live use with powerful computers
  - **Balanced** (100-150ms): General purpose, good stability
  - **High Stability** (200-300ms): Slower computers or network streaming

### Step 4: Configure Channel Mapping

Each of the 8 software channels can be mapped to any physical ASIO channel.

**For each channel (1-8), configure**:

- **ASIO Input Channel**: Which physical input to receive Listen audio from
- **ASIO Output Channel**: Which physical output to send Talk audio to

**Default Mapping** (recommended for beginners):
```
Channel 1 → ASIO Input 0, ASIO Output 0
Channel 2 → ASIO Input 1, ASIO Output 1
Channel 3 → ASIO Input 2, ASIO Output 2
...and so on
```

**Custom Mapping Example** (studio talkback):
```
Channel 1 (Drummer)      → ASIO Input 0, ASIO Output 0
Channel 2 (Guitarist)    → ASIO Input 1, ASIO Output 1
Channel 3 (Bassist)      → ASIO Input 2, ASIO Output 2
Channel 4 (Vocalist)     → ASIO Input 3, ASIO Output 3
Channel 5 (Control Room) → ASIO Input 4, ASIO Output 4
Channels 6-8 (Spare)     → As needed
```

### Step 5: Save Configuration

1. Click **"OK"** to save all settings
2. Settings are automatically saved to `intercom_settings.json`
3. They will be restored next time you launch the application

---

## Understanding the Interface

### Main Window Layout

The main window is divided into several sections:

```
┌─────────────────────────────────────────────────────┐
│  MediaNews Intercom v1.1                           │
├─────────────────────────────────────────────────────┤
│  ┌─────────┐  ┌─────────┐  ┌─────────┐  ┌─────────┐│
│  │Channel 1│  │Channel 2│  │Channel 3│  │Channel 4││
│  │         │  │         │  │         │  │         ││
│  │  [TALK] │  │  [TALK] │  │  [TALK] │  │  [TALK] ││
│  │ [LISTEN]│  │ [LISTEN]│  │ [LISTEN]│  │ [LISTEN]││
│  │  (knob) │  │  (knob) │  │  (knob) │  │  (knob) ││
│  │  (knob) │  │  (knob) │  │  (knob) │  │  (knob) ││
│  │  [VU]   │  │  [VU]   │  │  [VU]   │  │  [VU]   ││
│  └─────────┘  └─────────┘  └─────────┘  └─────────┘│
│  ┌─────────┐  ┌─────────┐  ┌─────────┐  ┌─────────┐│
│  │Channel 5│  │Channel 6│  │Channel 7│  │Channel 8││
│  └─────────┘  └─────────┘  └─────────┘  └─────────┘│
├─────────────────────────────────────────────────────┤
│  [START] [STOP]              [Settings] [Log ▼/▲]  │
├─────────────────────────────────────────────────────┤
│  Log Console                                        │
│  > System ready...                                  │
└─────────────────────────────────────────────────────┘
```

### Channel Strip Components

Each channel strip contains:

#### 1. Channel Name/Number
- Displays channel identifier (e.g., "Channel 1")
- Can be customized in Settings

#### 2. TALK Button
- **Color States**:
  - **Gray**: Talk is OFF (default)
  - **Red**: Talk is ACTIVE (your microphone is being sent to this channel)
- **Function**: Click to toggle sending your microphone to this ASIO output
- **Multiple Channels**: You can activate Talk on multiple channels simultaneously

#### 3. LISTEN Button
- **Color States**:
  - **Gray**: Listen is OFF (default)
  - **Cyan/Blue**: Listen is ACTIVE (you're hearing this channel)
- **Function**: Click to toggle receiving audio from this ASIO input
- **Mixing**: Multiple Listen channels are automatically mixed together

#### 4. Input Level Knob
- **Purpose**: Controls the volume of incoming Listen audio
- **Range**: 0-130
  - 0 = Silence (muted)
  - 100 = Unity gain (normal volume)
  - 130 = +30% amplification
- **Control Methods**:
  - **Drag**: Click and drag to adjust
  - **Mouse Wheel**: Scroll over the knob
  - **Double-Click**: Type exact value, press Enter to confirm
- **Color**: Cyan indicator

#### 5. Output Level Knob
- **Purpose**: Controls the volume of outgoing Talk audio
- **Range**: 0-130 (same as Input Level)
- **Control Methods**: Same as Input Level
- **Color**: Orange indicator

#### 6. VU Meter
- **Purpose**: Visual feedback of audio level
- **Color Code**:
  - **Green**: Normal levels (-20dB to -6dB)
  - **Yellow**: Loud levels (-6dB to 0dB)
  - **Red**: Clipping/distortion (avoid red!)
- **Shows**: Real-time audio activity for that channel

### Control Bar

#### START Button
- **Function**: Begins audio streaming
- **Effect**: Activates all audio processing
- **Status**: Changes to "Streaming Active" (green)
- **Prerequisite**: Devices must be configured in Settings

#### STOP Button
- **Function**: Stops audio streaming
- **Effect**: All Talk and Listen buttons are disabled
- **Status**: Returns to "Idle" (gray)
- **Cleanup**: Resets all channels

#### Settings Button
- **Function**: Opens the Settings window
- **Note**: Cannot change settings while streaming (stop first)

#### Log Toggle (▼/▲)
- **Function**: Show or hide the log console
- **Purpose**: View system messages, errors, and activity
- **Useful for**: Troubleshooting and monitoring

### Log Console

- **Location**: Bottom panel (toggle with ▼/▲)
- **Purpose**: Real-time system messages
- **Messages Include**:
  - Device initialization
  - Configuration changes
  - Audio buffer status
  - Error messages
  - Web server status
- **Tip**: Always check the log if something isn't working

---

## Basic Operations

### Starting the Intercom System

1. **Configure devices** (if first time - see [First-Time Setup](#first-time-setup))
2. **Click START** in the control bar
3. **Wait for confirmation**:
   - Status changes to "Streaming Active" (green)
   - Log shows "Audio streaming started successfully"
4. **System is now ready** for Talk and Listen operations

### Sending Audio (TALK)

**To talk to a specific channel**:

1. **Ensure streaming is active** (click START if not)
2. **Click the TALK button** on the desired channel
   - Button turns **RED**
   - Your microphone is now routed to that ASIO output
3. **Speak into your microphone**
   - The recipient on that ASIO channel will hear you
4. **Adjust Output Level** if needed (orange knob)
   - Higher values = louder to recipient
   - Lower values = quieter to recipient
5. **Click TALK again** to stop sending
   - Button returns to gray

**To talk to multiple channels simultaneously**:

1. **Click TALK** on Channel 1 (turns red)
2. **Click TALK** on Channel 3 (also turns red)
3. **Click TALK** on Channel 5 (also turns red)
4. **Now speaking** sends your microphone to channels 1, 3, and 5 simultaneously
5. Each channel can have different Output Levels

### Receiving Audio (LISTEN)

**To listen to a specific channel**:

1. **Ensure streaming is active** (click START if not)
2. **Click the LISTEN button** on the desired channel
   - Button turns **CYAN/BLUE**
   - Audio from that ASIO input is routed to your speakers
3. **Adjust Input Level** if needed (cyan knob)
   - Higher values = louder in your speakers
   - Lower values = quieter in your speakers
4. **Click LISTEN again** to stop receiving
   - Button returns to gray

**To listen to multiple channels simultaneously**:

1. **Click LISTEN** on multiple channels
   - All active LISTEN channels turn cyan
2. **Audio is automatically mixed**
   - All selected channels are combined
   - Automatic gain control prevents clipping
   - Each channel retains its individual Input Level setting
3. **Adjust individual levels** as needed
   - Balance different sources for optimal mix

### Adjusting Audio Levels

#### Method 1: Drag the Knob

1. **Click and hold** on the rotary knob
2. **Drag up or right** to increase
3. **Drag down or left** to decrease
4. **Release** to set the value
5. **Works anywhere**: Mouse capture allows dragging outside the channel box

#### Method 2: Mouse Wheel

1. **Hover** over the rotary knob
2. **Scroll up** to increase
3. **Scroll down** to decrease

#### Method 3: Keyboard Input (NEW in v1.1)

1. **Double-click** on the rotary knob
2. **A text box appears** with current value
3. **Type the desired value** (0-130)
4. **Press ENTER** to apply
5. **Press ESC** to cancel

**Example**: To set exactly 85:
- Double-click knob
- Type "85"
- Press Enter

### Understanding Audio Levels

**Input Level** (Cyan knob):
- Affects how loud you hear incoming LISTEN audio
- Does NOT affect what the remote person hears
- Personal monitoring preference

**Output Level** (Orange knob):
- Affects how loud your TALK is sent to the remote channel
- Does NOT affect what you hear
- Important: Set appropriately to avoid distortion

**Recommended Starting Values**:
- Input Level: **100** (unity gain)
- Output Level: **100** (unity gain)
- Adjust from there based on needs

### Monitoring with VU Meters

**Watch the VU meters** for each channel:

- **No activity**: Meter stays at zero
  - Check connections and settings
- **Green bars**: Healthy audio levels
  - Good signal strength
  - No distortion
- **Yellow bars**: Loud but acceptable
  - Approaching maximum
  - Consider reducing levels slightly
- **Red bars**: CLIPPING/DISTORTION
  - Reduce Input or Output Level immediately
  - Audio quality is degraded

### Stopping the System

1. **Click STOP** in the control bar
2. **All channels deactivate**:
   - Talk buttons return to gray
   - Listen buttons return to gray
   - Audio streaming stops
3. **Status** changes to "Idle"
4. **System is now idle** and can be reconfigured or closed

---

## Advanced Features

### Channel Naming

**Custom channel names** help identify sources:

1. **Open Settings**
2. **For each channel**, enter a descriptive name:
   - "Studio A"
   - "Control Room"
   - "Drummer"
   - "Guest 1"
   - etc.
3. **Click OK** to save
4. Names appear on channel strips and web interface

### Complex Routing Scenarios

#### Scenario 1: Producer to Multiple Musicians

**Setup**:
- Channels 1-4: Individual musicians (Drummer, Guitarist, Bassist, Vocalist)
- Channel 5: Producer's monitoring

**Operation**:
1. **Listen** to Channel 5 (hear producer cues)
2. **Talk** to Channels 1, 2, 3, 4 simultaneously (send instructions to all)
3. **Adjust Output Levels** individually if some need louder cues

#### Scenario 2: Selective Talkback

**Setup**:
- Channels 1-3: Talent booths
- Channels 4-8: Technical positions

**Operation**:
1. **Talk to Channel 1 only** (private direction to talent)
2. **Talk to Channels 4, 5, 6** (technical coordination)
3. **Listen to Channel 1** (hear talent response)

#### Scenario 3: Multi-Room Conference

**Setup**:
- Each channel represents a different room or participant

**Operation**:
1. **Moderator**: Talk to all channels (simultaneous broadcast)
2. **Listen to specific channels** for questions
3. **Dynamic routing**: Enable/disable channels as needed

### Buffer Size Optimization

**Finding the right buffer size**:

1. **Start with 100ms** (default)
2. **If you experience**:
   - **Dropouts/crackling**: INCREASE buffer (150ms, 200ms)
   - **Noticeable delay**: DECREASE buffer (70ms, 50ms)
3. **Test while streaming**:
   - Stop streaming
   - Open Settings
   - Change buffer size
   - Click OK
   - Start streaming
   - Test audio quality
4. **Optimal setting** depends on:
   - Computer CPU power
   - ASIO driver quality
   - Audio interface capabilities
   - Number of active channels

### Latency Considerations

**Total system latency** includes:
- ASIO driver latency (buffer size dependent)
- WDM device latency
- Processing time

**Typical latencies**:
- **30ms buffer**: ~10-15ms total (excellent for live work)
- **100ms buffer**: ~30-40ms total (acceptable for most uses)
- **200ms buffer**: ~60-80ms total (noticeable but stable)

**Reducing latency**:
1. Use native ASIO drivers (not ASIO4ALL)
2. Reduce buffer size
3. Close unnecessary applications
4. Use wired network (not Wi-Fi) for remote control

### Saving and Loading Configurations

**Automatic saving**:
- Settings are saved when you click "OK" in Settings
- Stored in: `intercom_settings.json` (same folder as executable)
- Automatically loaded on next launch

**Manual backup**:
1. **Locate** `intercom_settings.json`
   - Installed version: `C:\Program Files\MediaNews Intercom\`
   - Portable version: Same folder as `.exe`
2. **Copy** to a backup location
3. **Restore** by copying back

**Multiple configurations**:
- Rename `intercom_settings.json` for different setups
- Example:
  - `intercom_settings_studio_A.json`
  - `intercom_settings_live_show.json`
  - `intercom_settings_podcast.json`
- Rename the desired config to `intercom_settings.json` before launching

---

## Web Remote Control

### Overview

The web interface allows you to **control MediaNews Intercom from any device** on your local network using a web browser (smartphone, tablet, laptop).

**Features**:
- Control all 8 channels (Talk/Listen)
- Adjust all Input and Output levels
- Real-time VU meter
- Responsive design (works on mobile)
- No app installation required

### Enabling Network Access

#### Option 1: During Installation

- Check "Enable network access" during installation
- Firewall rules are configured automatically

#### Option 2: After Installation

1. **Find** the "Enable Network Access" shortcut in Start Menu
   - Start → MediaNews Intercom → Enable Network Access
2. **Or** run `ENABLE_NETWORK_ACCESS.bat` from installation folder
3. **Right-click** → "Run as Administrator"
4. **Follow prompts** to configure firewall

#### Option 3: Manual Configuration

Open **PowerShell as Administrator** and run:
```powershell
# Allow HTTP listener
netsh http add urlacl url=http://+:8080/ user=Everyone

# Add firewall rule
netsh advfirewall firewall add rule name="MediaNews Intercom Web 8080" dir=in action=allow protocol=TCP localport=8080
```

### Finding Your IP Address

When you **start MediaNews Intercom**, check the **log console**:

```
======================================
WEB SERVER STARTED SUCCESSFULLY
======================================
Local access:   http://localhost:8080/
Network access: http://192.168.1.100:8080/
======================================
```

Your **network IP address** is shown (example: `192.168.1.100`)

**Alternative method**:
1. Open Command Prompt
2. Type: `ipconfig`
3. Look for "IPv4 Address" under your network adapter
4. Example: `192.168.1.100`

### Accessing the Web Interface

#### From the Same Computer

Open your web browser and navigate to:
```
http://localhost:8080/
```

#### From Another Device (Same Network)

1. **Ensure both devices are on the same network**
   - Same Wi-Fi network, or
   - Same wired network
2. **Get the server IP** (from log console)
   - Example: `192.168.1.100`
3. **Open a web browser** on the remote device
4. **Navigate to**:
   ```
   http://192.168.1.100:8080/
   ```
   (Use YOUR IP address, not this example)

### Using the Web Interface

#### Interface Layout

```
┌─────────────────────────────────────────┐
│ MediaNews Intercom - Remote Control     │
│ [VU Meter==================]  Connected │
├─────────────────────────────────────────┤
│  ┌──────────┐  ┌──────────┐            │
│  │Channel 1 │  │Channel 2 │  ...       │
│  │[  TALK  ]│  │[  TALK  ]│            │
│  │[ LISTEN ]│  │[ LISTEN ]│            │
│  │ ━━━━━━ 100│  │ ━━━━━━ 100│          │
│  │ ━━━━━━ 100│  │ ━━━━━━ 100│          │
│  └──────────┘  └──────────┘            │
└─────────────────────────────────────────┘
```

#### Controls

**TALK Button**:
- Tap to toggle Talk on/off
- **Red** = Active
- **Gray** = Inactive

**LISTEN Button**:
- Tap to toggle Listen on/off
- **Green** = Active
- **Gray** = Inactive

**Input Level Slider**:
- Drag slider left/right to adjust (0-130)
- Value displayed on right
- Real-time update to application

**Output Level Slider**:
- Drag slider left/right to adjust (0-130)
- Value displayed on right
- Real-time update to application

#### Status Indicators

**Connection Status** (top right):
- **Green "Connected"**: Successfully communicating with server
- **Red "Disconnected"**: Lost connection to server

**VU Meter** (top):
- Shows master input level
- Updates every 200ms
- Color gradient from green to yellow to red

#### Mobile Tips

**For best mobile experience**:
- Use landscape orientation for 8-channel view
- Portrait mode stacks channels vertically
- Tap buttons with finger
- Drag sliders smoothly
- Works on iOS, Android, and tablets

### Troubleshooting Web Access

**Problem**: Cannot access from another device

**Solutions**:
1. **Check firewall**:
   - Run `ENABLE_NETWORK_ACCESS.bat` as Administrator
   - Or manually configure firewall (see above)

2. **Verify both devices on same network**:
   - Same Wi-Fi network name
   - Or both on wired network
   - Cannot access across different networks (e.g., guest Wi-Fi vs. main network)

3. **Check the IP address**:
   - Use the IP shown in the log console
   - IP may change if using DHCP
   - Consider setting a static IP for the server computer

4. **Test localhost first**:
   - On the server computer, try `http://localhost:8080/`
   - If this works, the server is running
   - Problem is network/firewall related

5. **Disable antivirus temporarily**:
   - Some antivirus software blocks HTTP servers
   - Test with antivirus disabled
   - If this fixes it, add exception for MediaNews Intercom

**Problem**: "Disconnected" status in web interface

**Solutions**:
1. **Refresh the page** (browser refresh)
2. **Check if application is running** on server
3. **Verify START button clicked** (streaming active)
4. **Check network connection** between devices

---

## Troubleshooting

### No Audio / Cannot Hear Anything

**Possible Causes & Solutions**:

1. **System not started**
   - ✅ Click the **START** button
   - Look for "Streaming Active" status

2. **No Listen channels active**
   - ✅ Click **LISTEN** on the channel you want to hear
   - Button should turn cyan/blue

3. **Input Level too low**
   - ✅ Check the cyan knob (Input Level)
   - Try setting to 100 or higher

4. **Wrong speaker selected**
   - ✅ Open Settings
   - ✅ Verify Speaker (WDM Output) is correct
   - ✅ Test with different output device

5. **ASIO device not sending audio**
   - ✅ Check ASIO Input Channel mapping
   - ✅ Verify physical connections to ASIO interface
   - ✅ Test ASIO device with other software

6. **VU meter shows no activity**
   - ✅ No audio is arriving from ASIO
   - ✅ Check cables and ASIO input routing

### Talk Not Working / Others Can't Hear Me

**Possible Causes & Solutions**:

1. **Talk not activated**
   - ✅ Click **TALK** on desired channel
   - Button should turn red

2. **Wrong microphone selected**
   - ✅ Open Settings
   - ✅ Verify Microphone (WDM Input) is correct
   - ✅ Test microphone in other applications

3. **Output Level too low**
   - ✅ Check orange knob (Output Level)
   - ✅ Try setting to 100 or higher

4. **Microphone muted in Windows**
   - ✅ Right-click speaker icon in taskbar
   - ✅ Open Sound settings
   - ✅ Check microphone is not muted
   - ✅ Adjust Windows microphone level

5. **ASIO output channel incorrect**
   - ✅ Open Settings
   - ✅ Verify ASIO Output Channel mapping
   - ✅ Check recipient is monitoring correct ASIO input

### Application Won't Start / Crashes

**Solutions**:

1. **Missing .NET Runtime**
   - ✅ Installer version includes runtime (use this)
   - ✅ Or download .NET 8.0 Runtime from Microsoft

2. **ASIO driver not installed**
   - ✅ Install ASIO4ALL or your interface's driver
   - ✅ Restart computer after installing

3. **Corrupted settings file**
   - ✅ Close application
   - ✅ Delete `intercom_settings.json`
   - ✅ Restart and reconfigure

4. **Antivirus blocking**
   - ✅ Add exception for MediaNews-Intercom.exe
   - ✅ Test with antivirus temporarily disabled

5. **Check log file**
   - ✅ Look for error messages in console
   - ✅ Screenshot and report issues on GitHub

### Audio Dropouts / Crackling / Glitches

**Solutions**:

1. **Increase buffer size**
   - ✅ Stop streaming
   - ✅ Open Settings
   - ✅ Increase buffer to 150ms or 200ms
   - ✅ Click OK and restart

2. **Close other applications**
   - ✅ Close web browsers
   - ✅ Close DAWs or other audio software
   - ✅ Close unnecessary background apps

3. **Check CPU usage**
   - ✅ Open Task Manager
   - ✅ If CPU > 80%, close applications
   - ✅ Consider using more powerful computer

4. **Update ASIO driver**
   - ✅ Check manufacturer's website for latest driver
   - ✅ Install and restart

5. **USB power issues** (if using USB audio)
   - ✅ Connect to USB port directly on computer (not hub)
   - ✅ Use USB 3.0 port if available
   - ✅ Try different USB cable

### High Latency / Noticeable Delay

**Solutions**:

1. **Reduce buffer size**
   - ✅ Stop streaming
   - ✅ Open Settings
   - ✅ Try 50ms or 70ms
   - ✅ Monitor for dropouts

2. **Use native ASIO driver**
   - ✅ Replace ASIO4ALL with manufacturer's driver
   - ✅ Native drivers typically have lower latency

3. **Optimize ASIO settings**
   - ✅ Open ASIO driver control panel
   - ✅ Reduce ASIO buffer size
   - ✅ Usually accessible from Windows system tray

4. **Disable Windows audio enhancements**
   - ✅ Right-click speaker icon → Sounds
   - ✅ Select your playback device → Properties
   - ✅ Enhancements tab → Disable all enhancements

### Feedback / Echo Issues

**Solutions**:

1. **Use headphones**
   - ✅ Never use speakers while Talk is active
   - ✅ Always use closed-back headphones
   - ✅ Keep microphone away from speakers

2. **Reduce output levels**
   - ✅ Lower Output Level knobs
   - ✅ Reduce Input Level knobs

3. **Mute Listen when not needed**
   - ✅ Only activate Listen when you need to hear
   - ✅ Deactivate immediately after

4. **Don't Talk and Listen on same channel**
   - ✅ If Channel 1 Talk is active, don't activate Channel 1 Listen
   - ✅ This creates a feedback loop

### "Please configure devices in Settings first"

**Solution**:
- ✅ Click Settings
- ✅ Select all three devices (Microphone, Speaker, ASIO)
- ✅ Click OK
- ✅ Try START again

### Web Interface Not Accessible

See [Web Remote Control → Troubleshooting Web Access](#troubleshooting-web-access) section above.

---

## Best Practices

### For Professional Use

1. **Dedicated Computer**
   - Use a dedicated computer for intercom only
   - Don't run other audio applications simultaneously
   - Minimize background processes

2. **Quality Audio Interface**
   - Use professional ASIO interface (RME, MOTU, UA, Focusrite)
   - Ensure at least 8 input and 8 output channels
   - Keep drivers updated

3. **Proper Cabling**
   - Use balanced XLR or TRS cables
   - Keep cable runs as short as practical
   - Label all cables for easy troubleshooting

4. **Backup Configuration**
   - Save `intercom_settings.json` in multiple locations
   - Document your channel routing
   - Keep a printed diagram of your setup

### For Live Events

1. **Test Before Event**
   - Complete sound check days before
   - Test all Talk and Listen paths
   - Verify web remote control access
   - Have backup plan ready

2. **Buffer Settings**
   - Use 100-150ms for stability
   - Don't prioritize low latency over reliability
   - Test with full load (all channels active)

3. **Redundancy**
   - Have backup microphone ready
   - Keep spare headphones available
   - Consider dual computer setup for critical events

### For Recording Studios

1. **Low Latency Setup**
   - Use 30-70ms buffer
   - Native ASIO drivers
   - Powerful CPU (i7/i9 or Ryzen 7/9)
   - SSD for operating system

2. **Clear Communication**
   - Name channels clearly (Control Room, Booth A, etc.)
   - Use consistent Talk/Listen patterns
   - Train all users on the system

3. **Integration**
   - Coordinate with DAW playback
   - Use separate monitoring paths
   - Consider using digital patchbay

### For Broadcasting

1. **Reliability First**
   - Higher buffer sizes (150-200ms)
   - Wired network only (no Wi-Fi for critical paths)
   - UPS power backup
   - Tested and documented procedures

2. **Monitoring**
   - Always watch VU meters
   - Keep levels in green range
   - Monitor log console for errors

3. **Emergency Procedures**
   - Document manual fallback procedures
   - Train backup operators
   - Keep printed manual accessible

### Audio Level Management

1. **Start Conservative**
   - Begin with all levels at 100
   - Gradually adjust as needed
   - Avoid exceeding 120 unless necessary

2. **Watch VU Meters**
   - Green = Good
   - Yellow = Caution
   - Red = Reduce immediately

3. **Consistent Levels**
   - Set similar Output Levels for all Talk channels
   - Set similar Input Levels for all Listen channels
   - Document your "standard" settings

4. **Prevent Distortion**
   - If clipping occurs, reduce Output Level first
   - If still clipping, reduce Input Level
   - Never exceed 130 on both knobs simultaneously

### Network Security

1. **Local Network Only**
   - Never expose to public internet
   - Use private IP addresses only
   - Consider VPN for remote access

2. **Firewall**
   - Keep firewall enabled
   - Only allow port 8080 for intercom
   - Monitor for unauthorized access

3. **Physical Security**
   - Restrict access to server computer
   - Use screen lock when unattended
   - Consider password protecting system

---

## FAQ

### General Questions

**Q: What does "Intercom" mean in this context?**

A: An intercom system allows selective point-to-point communication. In professional audio, it means you can talk to specific channels (people/rooms) individually or in groups, and listen to specific channels independently.

**Q: Can I use this with a regular USB microphone and headphones?**

A: Yes, for your Talk microphone and Listen speakers (WDM devices). However, you still need an ASIO-compatible audio interface for the 8 channels.

**Q: Do I need an expensive audio interface?**

A: Not necessarily. Even a budget 8-channel interface will work. Quality affects latency and audio quality, but the software works with any ASIO device.

**Q: Is ASIO4ALL good enough?**

A: ASIO4ALL works for testing and light use. For professional applications, a native ASIO driver from your interface manufacturer is recommended for better performance and lower latency.

### Technical Questions

**Q: What is the difference between Input Level and Output Level?**

A:
- **Input Level** (cyan): Controls volume of incoming Listen audio (what YOU hear)
- **Output Level** (orange): Controls volume of outgoing Talk audio (what THEY hear)

**Q: Can I use more than 8 channels?**

A: Currently, the software supports exactly 8 channels. Future versions may support more. The architecture is designed to be extensible.

**Q: What audio sample rates are supported?**

A: The system automatically adapts to your microphone's sample rate. Common rates (44.1kHz, 48kHz, 96kHz) all work. The ASIO driver handles its own sample rate.

**Q: Can I record the intercom communications?**

A: Not directly within this application. However, you can use your ASIO interface's routing software or a DAW to record the ASIO inputs and outputs.

**Q: What is the audio quality/bitrate?**

A: Audio is processed in 32-bit floating point internally, with quality determined by your ASIO interface. There is no compression or quality loss in the software.

### Setup Questions

**Q: Why do I need both WDM and ASIO devices?**

A: WDM (Windows standard) handles your personal microphone and headphones. ASIO handles the professional multi-channel interface. This separation allows you to use a simple USB headset for personal monitoring while routing professional audio through ASIO.

**Q: Can I use the same device for both WDM and ASIO?**

A: Generally not recommended and may cause conflicts. Some interfaces support this, but it's better to use separate devices (e.g., USB headset for WDM, professional interface for ASIO).

**Q: My ASIO device doesn't appear in the list. What should I do?**

A:
1. Ensure the ASIO driver is installed
2. Restart the application
3. Check if the device works in other ASIO applications
4. Try reinstalling the ASIO driver

**Q: What buffer size should I use?**

A:
- **Live work**: 50-100ms
- **General use**: 100-150ms
- **Stability priority**: 150-300ms
- Start at 100ms and adjust based on your experience.

### Operation Questions

**Q: Can I Talk and Listen on the same channel simultaneously?**

A: Technically yes, but NOT recommended. This creates a feedback loop. Use Talk to send and Listen to receive on different channels.

**Q: How many channels can I Talk to at once?**

A: All 8 simultaneously if needed. Each channel's Talk button can be independently activated.

**Q: How many channels can I Listen to at once?**

A: All 8 simultaneously. The software automatically mixes them together with level balancing.

**Q: The VU meter is always red. Is this bad?**

A: Yes! Red indicates clipping/distortion. Reduce the Input Level or Output Level until the meter stays mostly green with occasional yellow peaks.

**Q: Can I change settings while streaming?**

A: No, you must click STOP first, then open Settings, make changes, click OK, then START again.

### Web Interface Questions

**Q: Can I access the web interface over the internet?**

A: Not by default (security reasons). It's designed for local network only. You could set up port forwarding or VPN, but this is not officially supported.

**Q: What browsers are supported?**

A: All modern browsers: Chrome, Firefox, Safari, Edge. Mobile browsers (iOS Safari, Android Chrome) also work.

**Q: Can multiple people control via web interface simultaneously?**

A: Yes, multiple browsers can connect. All changes are synchronized in real-time. Be careful with conflicting commands.

**Q: Does the web interface work if I close the main application window?**

A: No, the main application must be running for the web server to work.

**Q: Why is my IP address different from last time?**

A: Most networks use DHCP, which assigns dynamic IP addresses. Consider setting a static IP for the server computer if this is problematic.

### Troubleshooting Questions

**Q: I get "Error starting streaming". What does this mean?**

A: The audio system couldn't initialize. Common causes:
- Another application is using the ASIO device
- Devices not properly configured in Settings
- ASIO driver crashed (restart computer)
- Buffer size too small (increase it)

**Q: Audio is delayed/echoing. How do I fix this?**

A: Reduce buffer size for less latency. If echo persists, you may have a feedback loop—disable Listen on channels where Talk is active.

**Q: Can I run multiple instances of MediaNews Intercom?**

A: No, only one instance can control an ASIO device at a time. Running multiple instances will cause conflicts.

**Q: The application is slow/laggy. What can I do?**

A:
- Close other applications
- Increase buffer size
- Update graphics drivers
- Check CPU usage in Task Manager
- Consider a faster computer

### Licensing & Support Questions

**Q: Is this free software?**

A: Yes, MediaNews Intercom is open source under the MIT License. You can use it for free, even commercially.

**Q: Can I modify the source code?**

A: Yes! The MIT License allows modification, redistribution, and commercial use. Source code is available on GitHub.

**Q: Where do I report bugs?**

A: Please report issues on the GitHub repository: https://github.com/robertomusso/medianews-intercom/issues

**Q: Can I request new features?**

A: Absolutely! Open a feature request on GitHub. Community contributions are welcome.

**Q: Is there commercial support available?**

A: This is a community project. Support is provided through GitHub issues and community forums. For commercial support, you may hire a developer to customize the software.

---

## Appendix A: Keyboard Shortcuts

Currently, MediaNews Intercom does not have keyboard shortcuts. This may be added in a future version.

**Feature Request**: If you'd like keyboard shortcuts (e.g., hotkeys for Talk buttons), please request this feature on GitHub!

---

## Appendix B: Technical Specifications

### Audio Processing

- **Internal Processing**: 32-bit IEEE floating point
- **Sample Rates**: Adapts to WDM microphone rate (typically 44.1kHz or 48kHz)
- **Latency**: Configurable via buffer size (10-500ms)
- **Channels**: 8 independent bidirectional channels
- **Mixing**: Automatic gain-controlled mixing for multiple Listen channels

### System Architecture

- **Framework**: .NET 8.0 (Windows)
- **Audio Library**: NAudio 2.2.1
- **ASIO Support**: NAudio.Asio 2.2.1
- **WDM Support**: NAudio.Wasapi
- **Web Server**: Built-in HTTP server (port 8080)
- **Configuration**: JSON file storage

### File Locations

**Installed Version**:
- Application: `C:\Program Files\MediaNews Intercom\`
- Configuration: `C:\Program Files\MediaNews Intercom\intercom_settings.json`
- Logs: Console only (not saved to file)

**Portable Version**:
- Application: `[Extraction folder]`
- Configuration: `[Extraction folder]\intercom_settings.json`

### Network Specifications

- **Protocol**: HTTP (not HTTPS)
- **Port**: 8080 (hardcoded)
- **Update Rate**: 200ms (5 times per second)
- **CORS**: Enabled (allows access from any origin)
- **Maximum Clients**: Unlimited (limited by network bandwidth)

---

## Appendix C: Credits & Acknowledgments

### Software Components

- **NAudio** by Mark Heath
  - Excellent .NET audio library
  - https://github.com/naudio/NAudio
  - License: MIT

- **ASIO SDK** by Steinberg
  - Professional audio interface standard
  - https://www.steinberg.net/

### Development

- **Created with**: Claude Code (Anthropic)
- **Author**: Roberto Musso
- **License**: MIT License

### Community

Special thanks to:
- Beta testers and early adopters
- Contributors on GitHub
- NAudio community
- Professional audio community for feedback

---

## Appendix D: Version History

### Version 1.1.0 (Current)

**Release Date**: November 2025

**New Features**:
- 🌐 Complete web remote control interface
- 🖱️ Enhanced rotary controls (mouse capture, keyboard input)
- 🔧 Automatic network configuration tools
- 📦 Professional Windows installer

**Improvements**:
- Rotary controls work outside channel box boundaries
- Double-click for direct numeric value entry
- Better diagnostic messages for network setup
- Improved stability and error handling

**Bug Fixes**:
- Fixed mouse tracking issues with level controls
- Improved web server reliability
- Better handling of ASIO driver edge cases

### Version 1.0.0

**Release Date**: October 2025

**Features**:
- 8-channel professional intercom system
- Individual Talk and Listen controls
- Input and Output level adjustment (0-130)
- ASIO and WDM device support
- Real-time VU meters
- Channel mapping configuration
- Settings persistence
- Dark theme UI

---

## Getting Help

### Documentation

- **This Manual**: Complete user guide
- **README.md**: Quick start and overview
- **BUILD_INSTRUCTIONS.md**: For developers
- **GITHUB_RELEASE_GUIDE.md**: For maintainers

### Online Resources

- **GitHub Repository**: https://github.com/robertomusso/medianews-intercom
- **Issues**: https://github.com/robertomusso/medianews-intercom/issues
- **Releases**: https://github.com/robertomusso/medianews-intercom/releases

### Reporting Issues

When reporting a bug, please include:

1. **System Information**:
   - Windows version (10 or 11)
   - MediaNews Intercom version
   - ASIO driver name and version

2. **Steps to Reproduce**:
   - Exact steps that cause the problem
   - Expected behavior
   - Actual behavior

3. **Log Output**:
   - Screenshot of log console
   - Error messages if any

4. **Configuration**:
   - Device names
   - Buffer size
   - Channel mapping (if relevant)

### Contributing

Contributions are welcome! See the repository for:
- Code contribution guidelines
- Feature requests
- Pull request process

---

**Thank you for using MediaNews Intercom!**

For the latest updates and releases, visit:
https://github.com/robertomusso/medianews-intercom

---

*MediaNews Intercom v1.1 - User Manual*
*Last Updated: November 2025*
*Copyright © 2025 Roberto Musso - MIT License*
