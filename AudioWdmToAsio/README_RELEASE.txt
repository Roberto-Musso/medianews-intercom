================================================================================
MEDIANEWS INTERCOM - 8 CHANNEL PROFESSIONAL AUDIO INTERCOM SYSTEM
================================================================================

Version: 1.0.0
Developer: Roberto Musso
Date: November 2025

================================================================================
DESCRIPTION
================================================================================

MediaNews Intercom is a professional 8-channel audio intercom system designed
for broadcast and production environments. It allows routing audio between
a WDM/WASAPI microphone input, ASIO multi-channel interface, and speakers
with individual control over each channel.

KEY FEATURES:
- 8 independent intercom channels with custom naming
- TALK mode: Send microphone to specific ASIO output channels
- LISTEN mode: Receive audio from specific ASIO input channels
- Individual input/output level control (0-130%, with visual rotary knobs)
- Real-time VU meter for microphone input level
- Noise gate with adjustable threshold, attack, and release
- Web-based remote control interface (http://localhost:8080)
- Modern dark-themed user interface
- Persistent settings storage

================================================================================
SYSTEM REQUIREMENTS
================================================================================

- Windows 10 or later (64-bit)
- .NET 8.0 Runtime (included in self-contained installation)
- ASIO driver installed for your audio interface
- WDM/WASAPI compatible microphone
- Minimum 4GB RAM
- Audio interface with multi-channel ASIO support

================================================================================
INSTALLATION
================================================================================

1. Run the installer: INSTALL.bat
2. Follow the on-screen instructions
3. The application will be installed to:
   C:\Program Files\MediaNews-Intercom\

4. A desktop shortcut will be created automatically

================================================================================
QUICK START GUIDE
================================================================================

1. LAUNCH THE APPLICATION
   - Double-click the desktop shortcut or run MediaNews-Intercom.exe

2. CONFIGURE AUDIO DEVICES
   - Click the "SETTINGS" button
   - Select your microphone (WDM Input Device)
   - Select your speakers (WDM Output Device)
   - Select your ASIO device
   - Configure buffer size (lower = less latency, higher = more stability)
   - Map ASIO channels to intercom channels
   - Click "SAVE"

3. START STREAMING
   - Click the "START" button
   - The button will turn green when streaming is active

4. USE THE INTERCOM
   - TALK Button: Click to send your microphone to that channel's ASIO output
     (Button turns RED when active)
   - LISTEN Button: Click to hear audio from that channel's ASIO input
     (Button turns GREEN when active)
   - Input Level: Controls the volume of incoming audio from ASIO
   - Output Level: Controls the volume of your microphone sent to ASIO

5. CUSTOMIZE CHANNEL NAMES
   - Click on the channel name to edit it
   - Names are saved automatically

================================================================================
WEB REMOTE CONTROL
================================================================================

Access the web interface from any browser:
- Same computer: http://localhost:8080
- Other devices on network: http://[YOUR-PC-IP]:8080

Find your PC's IP address:
- Open Command Prompt
- Type: ipconfig
- Look for "IPv4 Address"

The web interface provides full control over all channels, including:
- Talk/Listen button toggles
- Input/Output level adjustments
- Real-time VU meter
- Channel status display

================================================================================
NOISE GATE
================================================================================

The noise gate helps reduce background noise when you're not speaking:

- Threshold: Audio level below which the gate closes (-60 to 0 dB)
- Attack: How quickly the gate opens when signal exceeds threshold (1-100 ms)
- Release: How quickly the gate closes when signal drops (10-1000 ms)

Recommended starting values:
- Threshold: -40 dB
- Attack: 5 ms
- Release: 100 ms

================================================================================
SETTINGS FILE
================================================================================

Settings are automatically saved to:
%APPDATA%\MediaNews-Intercom\settings.json

This file includes:
- Device selections
- Channel names
- Input/Output levels
- ASIO channel mappings
- Noise gate parameters

================================================================================
TROUBLESHOOTING
================================================================================

PROBLEM: Application won't start
SOLUTION: Make sure you have .NET 8.0 Runtime installed (included in installer)

PROBLEM: No audio devices showing in settings
SOLUTION: Check that your audio drivers are properly installed

PROBLEM: ASIO driver not found
SOLUTION: Install ASIO4ALL or your audio interface's ASIO driver

PROBLEM: High latency or audio dropouts
SOLUTION: Increase buffer size in settings (try 100-200ms)

PROBLEM: Microphone not working
SOLUTION: Check Windows sound settings and ensure microphone is not muted

PROBLEM: Web interface not accessible
SOLUTION: Check Windows Firewall settings, port 8080 must be allowed

PROBLEM: Settings not saving
SOLUTION: Run application as administrator or check folder permissions

================================================================================
TECHNICAL DETAILS
================================================================================

AUDIO PROCESSING:
- Sample Rate: Automatically matched to ASIO driver (typically 48kHz)
- Bit Depth: 32-bit float internal processing
- Latency: Configurable buffer (10-500ms)
- Channels: 8 independent stereo channels

WEB SERVER:
- Port: 8080
- Protocol: HTTP
- API: RESTful JSON
- Update Rate: 200ms polling

SUPPORTED FORMATS:
- Input: WDM/WASAPI (all Windows-compatible microphones)
- Output: WDM/WASAPI (all Windows-compatible speakers)
- ASIO: Professional multi-channel audio interfaces

================================================================================
KNOWN LIMITATIONS
================================================================================

- Maximum 8 channels
- Web interface requires modern browser (Chrome, Edge, Firefox)
- ASIO driver required for multi-channel operation
- Windows only (not compatible with Mac or Linux)

================================================================================
SUPPORT
================================================================================

This is non-commercial software provided AS-IS without warranty.
No official support is provided.

For bug reports or feature requests, please contact: Roberto Musso

================================================================================
LICENSE
================================================================================

This software is licensed for NON-COMMERCIAL USE ONLY.
See LICENSE.txt for full license terms.

Copyright (c) 2025 Roberto Musso
All Rights Reserved

================================================================================
VERSION HISTORY
================================================================================

Version 1.0.0 (November 2025)
- Initial release
- 8-channel intercom system
- WDM/ASIO integration
- Web remote control
- Noise gate functionality
- Custom rotary controls
- Real-time VU meter
- Persistent settings

================================================================================
CREDITS
================================================================================

Developer: Roberto Musso
Audio Processing: NAudio Library (https://github.com/naudio/NAudio)
Framework: .NET 8.0 / Windows Forms

================================================================================

Thank you for using MediaNews Intercom!

================================================================================
