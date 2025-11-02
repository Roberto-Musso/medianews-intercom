# Quick GitHub Publishing Guide - MediaNews Intercom v1.1.0

## Step-by-Step Guide to Publish Your Project

---

## Prerequisites Check

Before starting, make sure you have:
- [ ] GitHub account (create one at https://github.com/signup if needed)
- [ ] Git installed on Windows
- [ ] Release files ready (installer, ZIP, PDF manual)

### Installing Git (if needed)

1. Download Git for Windows: https://git-scm.com/download/win
2. Run the installer with default options
3. Restart your terminal/command prompt

**Verify Git is installed**:
```cmd
git --version
```
Should show something like: `git version 2.x.x`

---

## STEP 1: Create GitHub Repository

### 1.1 Go to GitHub
- Open browser: https://github.com/new
- (Login if needed)

### 1.2 Fill Repository Details

**Repository name**: `medianews-intercom`

**Description**:
```
Professional 8-channel audio intercom system with remote web control
```

**Visibility**:
- ✅ **Public** (recommended for open source)
- ⬜ Private (if you want to keep it private initially)

**Initialize repository**:
- ⬜ **DO NOT** check "Add a README file" (we already have one)
- ⬜ **DO NOT** add .gitignore (we have our own)
- ⬜ **DO NOT** choose a license (we have MIT already)

### 1.3 Click "Create repository"

GitHub will show you a page with setup instructions. **Keep this page open!**

---

## STEP 2: Initialize Git in Your Project

Open **Command Prompt** or **PowerShell** in your project folder:

```cmd
cd C:\TEMP\INTERCOM_2
```

### 2.1 Initialize Git

```cmd
git init
```

Output: `Initialized empty Git repository in C:/TEMP/INTERCOM_2/.git/`

### 2.2 Configure Git (First Time Only)

If you haven't used Git before on this computer:

```cmd
git config --global user.name "Your Name"
git config --global user.email "your.email@example.com"
```

**Replace with YOUR information**:
- Use your real name
- Use the email associated with your GitHub account

---

## STEP 3: Add Files to Git

### 3.1 Check What Will Be Added

```cmd
git status
```

This shows all files that will be included. The `.gitignore` file excludes build outputs automatically.

### 3.2 Add All Files

```cmd
git add .
```

This adds all files except those in `.gitignore` (build folders, etc.)

### 3.3 Verify

```cmd
git status
```

Should show many files in green (ready to commit).

---

## STEP 4: Create First Commit

```cmd
git commit -m "Initial release v1.1.0 - Professional 8-channel intercom with web control"
```

Output should show: `X files changed, Y insertions(+)`

---

## STEP 5: Connect to GitHub

**Get your repository URL from GitHub** (from the page that opened in Step 1.3)

It looks like: `https://github.com/YOUR-USERNAME/medianews-intercom.git`

### 5.1 Add Remote Repository

```cmd
git remote add origin https://github.com/YOUR-USERNAME/medianews-intercom.git
```

**⚠️ IMPORTANT**: Replace `YOUR-USERNAME` with your actual GitHub username!

**Example**:
```cmd
git remote add origin https://github.com/robertomusso/medianews-intercom.git
```

### 5.2 Rename Branch to "main"

```cmd
git branch -M main
```

---

## STEP 6: Push to GitHub

### 6.1 Push Your Code

```cmd
git push -u origin main
```

**What happens**:
- Git will ask for your GitHub credentials
- **Username**: Your GitHub username
- **Password**: **NOT your password!** Use a **Personal Access Token** (see below)

### 6.2 GitHub Authentication

GitHub no longer accepts passwords. You need a **Personal Access Token**:

**Creating a Token**:
1. Go to: https://github.com/settings/tokens
2. Click "Generate new token" → "Generate new token (classic)"
3. Note: "MediaNews Intercom Access"
4. Expiration: Choose duration (90 days or more)
5. Scopes: Check ✅ **repo** (full control of private repositories)
6. Click "Generate token"
7. **COPY THE TOKEN** (you won't see it again!)
8. Use this token as your password when Git asks

**Alternative**: Use GitHub Desktop (easier for beginners)
- Download: https://desktop.github.com/
- Login with your GitHub account
- It handles authentication automatically

### 6.3 Verify Push Succeeded

Go to your GitHub repository page:
```
https://github.com/YOUR-USERNAME/medianews-intercom
```

You should see all your files!

---

## STEP 7: Create a Release

### 7.1 Go to Releases

On your GitHub repository page:
1. Click **"Releases"** (right sidebar)
2. Click **"Create a new release"**

### 7.2 Fill Release Details

**Choose a tag**:
- Click "Choose a tag"
- Type: `v1.1.0`
- Click "Create new tag: v1.1.0 on publish"

**Release title**:
```
MediaNews Intercom v1.1.0 - Remote Control & UX Improvements
```

**Description** (copy this):

```markdown
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

### 📥 Downloads

**For most users**: Download the **Windows Installer** (easiest option)

**For portable use**: Download the **Portable ZIP** (no installation required)

**Documentation**: Download the **User Manual PDF** for complete guide

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

### 📋 System Requirements

- Windows 10/11 (64-bit)
- ASIO-compatible audio interface (8+ channels)
- 4 GB RAM (8 GB recommended)
- ASIO driver (ASIO4ALL, FlexASIO, or native driver)

### 🐛 Bug Fixes

- Fixed rotary controls losing mouse tracking outside control bounds
- Improved web server startup reliability
- Better error handling for network configuration

### 📖 Documentation

- [User Manual (PDF)](USER_MANUAL.pdf) - Complete 60-page guide
- [README](README.md) - Quick start and overview
- [Build Instructions](BUILD_INSTRUCTIONS.md) - For developers

### 🙏 Acknowledgments

- **NAudio** by Mark Heath - Excellent .NET audio library
- **Steinberg ASIO SDK** - Professional audio standard
- Open source community

### 📝 License

MIT License - Free for personal and commercial use

---

**Full Changelog**: https://github.com/YOUR-USERNAME/medianews-intercom/commits/v1.1.0

🐛 [Report Issues](https://github.com/YOUR-USERNAME/medianews-intercom/issues) | 💬 [Discussions](https://github.com/YOUR-USERNAME/medianews-intercom/discussions)
```

**⚠️ Replace `YOUR-USERNAME`** in the URLs with your GitHub username!

### 7.3 Upload Release Files

**Drag and drop these files** to the "Attach binaries" area:

1. `MediaNews-Intercom-Setup-v1.1.0.exe` (from `publish\` folder)
2. `MediaNews-Intercom-v1.1.0-Portable.zip` (from `publish\` folder)
3. `USER_MANUAL.pdf` (if you created it)

**Files should be**:
- ✅ Installer: ~49 MB
- ✅ Portable ZIP: ~68 MB
- ✅ User Manual: ~1-5 MB

### 7.4 Final Settings

- ✅ Check "Set as the latest release"
- ⬜ "Set as a pre-release" (leave unchecked)
- ⬜ "Create a discussion" (optional)

### 7.5 Publish!

Click **"Publish release"** button

---

## STEP 8: Verify Your Release

### 8.1 Check Release Page

Your release is now live at:
```
https://github.com/YOUR-USERNAME/medianews-intercom/releases/tag/v1.1.0
```

### 8.2 What Users Will See

- Release title and description
- Download buttons for all 3 files
- Installation instructions
- System requirements

### 8.3 Test Downloads

Try downloading one of your files to verify it works!

---

## STEP 9: Update Repository Settings (Optional but Recommended)

### 9.1 Add Topics

On your repository main page:
1. Click ⚙️ (gear icon) next to "About"
2. Add topics/tags:
   - `audio`
   - `intercom`
   - `asio`
   - `broadcasting`
   - `studio`
   - `csharp`
   - `dotnet`
   - `windows`
   - `audio-engineering`
3. Click "Save changes"

### 9.2 Edit Description

In the same dialog:
- **Description**: `Professional 8-channel audio intercom system with remote web control`
- **Website**: (leave blank or add your website)
- Click "Save changes"

### 9.3 Enable Issues

If not already enabled:
1. Go to "Settings" tab
2. Scroll to "Features"
3. Check ✅ "Issues"

---

## STEP 10: Share Your Release!

### Your project is now live! Share it:

**Repository URL**:
```
https://github.com/YOUR-USERNAME/medianews-intercom
```

**Latest Release URL**:
```
https://github.com/YOUR-USERNAME/medianews-intercom/releases/latest
```

**Clone Command** (for users who want source code):
```
git clone https://github.com/YOUR-USERNAME/medianews-intercom.git
```

### Promote on Social Media (Optional)

- **Reddit**: r/audioengineering, r/livesound
- **Twitter/X**: #AudioEngineering #ASIO #OpenSource
- **Audio Forums**: Gearslutz, KVR Audio, Sound on Sound

---

## Troubleshooting

### "fatal: 'origin' does not appear to be a git repository"

**Solution**: You didn't add the remote. Run:
```cmd
git remote add origin https://github.com/YOUR-USERNAME/medianews-intercom.git
```

### "error: failed to push some refs"

**Solution**: The remote has commits you don't have. Usually happens on first push. Try:
```cmd
git pull origin main --allow-unrelated-histories
git push -u origin main
```

### "Permission denied (publickey)"

**Solution**: Use HTTPS instead of SSH, or set up SSH keys properly.

For HTTPS:
```cmd
git remote set-url origin https://github.com/YOUR-USERNAME/medianews-intercom.git
```

### "Support for password authentication was removed"

**Solution**: Use a Personal Access Token instead of your password (see Step 6.2)

### Can't upload files larger than 100 MB

GitHub has a 100 MB limit per file. Your files are:
- Installer: 49 MB ✅
- ZIP: 68 MB ✅
- PDF: <5 MB ✅

All within limits!

---

## Quick Reference Commands

```cmd
# Check Git status
git status

# Add all changes
git add .

# Commit with message
git commit -m "Your message here"

# Push to GitHub
git push

# Pull from GitHub
git pull

# Create new tag
git tag v1.2.0
git push origin v1.2.0

# View remote URL
git remote -v

# Change remote URL
git remote set-url origin https://github.com/NEW-URL.git
```

---

## Next Steps After Publishing

### Monitor Your Project

1. **Watch for Issues**: Check https://github.com/YOUR-USERNAME/medianews-intercom/issues
2. **Respond to Questions**: Be helpful to users
3. **Accept Contributions**: Review pull requests

### Future Updates

When you release v1.2.0:

```cmd
# Make your changes to code
# ...

# Commit changes
git add .
git commit -m "Add new feature XYZ"
git push

# Create new tag
git tag v1.2.0
git push origin v1.2.0

# Then create new release on GitHub with tag v1.2.0
```

### Documentation

Consider adding:
- Screenshots in README
- Video tutorial
- Wiki pages for advanced topics
- CONTRIBUTING.md for contributors

---

## Success Checklist

After completing all steps:

- [ ] Repository created on GitHub
- [ ] Code pushed to main branch
- [ ] Release v1.1.0 created
- [ ] Installer uploaded to release
- [ ] Portable ZIP uploaded to release
- [ ] User Manual PDF uploaded to release
- [ ] Release description is complete
- [ ] Topics/tags added to repository
- [ ] README.md is visible and formatted correctly
- [ ] Downloads tested and working

---

**Congratulations!** 🎉

Your MediaNews Intercom project is now publicly available on GitHub!

**Your repository**: https://github.com/YOUR-USERNAME/medianews-intercom

---

*Need help? Open an issue on GitHub or check Git documentation at https://git-scm.com/doc*
