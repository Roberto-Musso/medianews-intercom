# Release Text for GitHub v1.1.0
# Copy and paste this into GitHub Release description

---

## 🎉 MediaNews Intercom v1.1.0

Professional 8-channel audio intercom system with remote web control capabilities.

### ✨ What's New

**🌐 Web Remote Control**
- Complete web interface accessible from any device on local network
- Control all 8 channels (Talk/Listen) from browser
- Adjust input/output levels remotely
- Real-time VU meter visualization
- Responsive design for mobile and tablet

**🖱️ Enhanced User Experience**
- Improved rotary controls with mouse capture (work outside channel box)
- Double-click on knobs for direct numeric input
- Type value and press Enter to confirm

**🔧 Network Configuration**
- Automatic firewall configuration during installation
- ENABLE_NETWORK_ACCESS.bat script included
- Better diagnostic messages
- Auto-detection of local IP address

**📦 Professional Installer**
- Windows installer with all dependencies
- Includes .NET 8.0 Runtime (no manual installation needed)
- Desktop shortcut and Start Menu integration
- Optional network access setup during install

---

### 📥 Downloads

**For most users**: Download the **Windows Installer** (easiest option)

**For portable use**: Download the **Portable ZIP** (no installation required)

**Documentation**: Download the **User Manual PDF** for complete guide

---

### 💾 Installation

**Windows Installer**:
1. Download `MediaNews-Intercom-Setup-v1.1.0.exe`
2. Run the installer
3. Follow the setup wizard
4. Launch from desktop shortcut

**Portable Version**:
1. Download `MediaNews-Intercom-v1.1.0-Portable.zip`
2. Extract to any folder
3. Run `MediaNews-Intercom.exe`
4. (Optional) Run `ENABLE_NETWORK_ACCESS.bat` as admin for web access

---

### 📋 System Requirements

- Windows 10/11 (64-bit)
- ASIO-compatible audio interface (8+ channels)
- 4 GB RAM (8 GB recommended)
- ASIO driver (ASIO4ALL, FlexASIO, or native driver)

---

### 🐛 Bug Fixes

- Fixed rotary controls losing mouse tracking outside control bounds
- Improved web server startup reliability
- Better error handling for network configuration

---

### 📖 Documentation

- **User Manual (PDF)** - Complete 60-page guide (attached)
- [README](README.md) - Quick start and overview
- [Build Instructions](BUILD_INSTRUCTIONS.md) - For developers

---

### 🙏 Acknowledgments

- **NAudio** by Mark Heath - Excellent .NET audio library
- **Steinberg ASIO SDK** - Professional audio standard
- Open source community

---

### 📝 License

MIT License - Free for personal and commercial use

---

**Full Changelog**: https://github.com/Roberto-Musso/medianews-intercom/commits/v1.1.0

🐛 [Report Issues](https://github.com/Roberto-Musso/medianews-intercom/issues) | 💬 [Discussions](https://github.com/Roberto-Musso/medianews-intercom/discussions)
