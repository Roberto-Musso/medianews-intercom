# MediaNews Intercom v1.1.0 - Release Notes

## 🎉 What's New in v1.1.0

### 🌐 Web Remote Control
- Complete web interface for remote control from any device on the local network
- Control Talk/Listen for all 8 channels from your browser
- Adjust input/output levels remotely
- Real-time VU meter visualization
- Responsive design optimized for mobile and tablet devices
- Access from: `http://[your-ip]:8080/`

### 🖱️ Enhanced Rotary Controls
- **Mouse Capture**: Rotary knobs now work properly even when dragging outside the channel box
- **Keyboard Input**: Double-click any rotary control to enter a numeric value directly
  - Type the desired value (0-130)
  - Press ENTER to confirm
  - Press ESC to cancel
- Much smoother and more intuitive user experience

### 🔧 Network Setup Tools
- New `ENABLE_NETWORK_ACCESS.bat` script for automatic firewall configuration
- Improved diagnostic messages when starting the web server
- Automatic local IP detection and display
- Clear instructions for enabling remote access

### 📦 Professional Installer
- Windows installer with all dependencies included
- No need to manually install .NET 8.0 Runtime
- Automatic network configuration during installation (optional)
- Desktop shortcut and Start Menu integration
- Clean uninstall process

## 📥 Download Options

### Windows Installer (Recommended)
- **File**: `MediaNews-Intercom-Setup-v1.1.0.exe`
- **Size**: ~80 MB
- Includes .NET 8.0 Runtime
- Automatic setup and configuration
- Creates shortcuts and Start Menu entries

### Portable ZIP Package
- **File**: `MediaNews-Intercom-v1.1.0-Portable.zip`
- **Size**: ~80 MB
- No installation required
- Extract and run
- Includes .NET 8.0 Runtime

## ⚙️ System Requirements

- **OS**: Windows 10/11 (64-bit)
- **RAM**: 4 GB minimum, 8 GB recommended
- **Audio**: ASIO-compatible audio interface with 8+ input/output channels
- **ASIO Driver**: ASIO4ALL, FlexASIO, or manufacturer's native ASIO driver
- **Network** (for web interface): Local network connection

## 🚀 Quick Start

1. Download and run the installer
2. Launch MediaNews Intercom
3. Go to Settings and configure:
   - Microphone (WDM Input)
   - Speaker (WDM Output)
   - ASIO Device
   - Channel mapping
4. Click START
5. Use Talk and Listen buttons to communicate

### Enabling Remote Web Access

**Option 1**: During installation, check "Enable network access"

**Option 2**: After installation, run "Enable Network Access" from Start Menu

**Option 3**: Run application as Administrator (first launch)

The web interface will be accessible at `http://[your-ip]:8080/`

## 🐛 Bug Fixes

- Fixed rotary controls losing focus when mouse moves outside channel bounds
- Improved web server startup reliability
- Better error handling for network configuration
- Fixed potential race conditions in audio processing

## 🔒 Security Notes

- Web interface uses HTTP (not HTTPS) - only use on trusted local networks
- Network access requires firewall rule configuration
- Always run the application with appropriate user permissions

## 📝 Known Issues

- Some ASIO drivers may have specific buffer size requirements
- Web interface may experience slight delays (<200ms) on slow networks
- Firewall software may need manual configuration on some systems

## 🙏 Acknowledgments

- **NAudio** by Mark Heath - Excellent .NET audio library
- **ASIO SDK** by Steinberg - Professional audio interface standard
- All beta testers and early adopters

## 📚 Documentation

Full documentation available in the [README.md](README.md) file and on the [GitHub repository](https://github.com/robertomusso/medianews-intercom).

## 💬 Support

- Report issues: [GitHub Issues](https://github.com/robertomusso/medianews-intercom/issues)
- Check existing solutions in the troubleshooting section
- Review the log console for detailed error messages

---

**Enjoy MediaNews Intercom v1.1.0!**
