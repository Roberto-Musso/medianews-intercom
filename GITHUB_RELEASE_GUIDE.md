# GitHub Release Guide - MediaNews Intercom v1.1.0

## Prerequisites

Before creating the release, ensure you have:

1. ✅ GitHub account
2. ✅ Git installed on your system
3. ✅ Inno Setup 6 installed (for creating the installer)
4. ✅ Built and tested the application

## Step 1: Prepare the Build

1. **Install Inno Setup** (if not already installed):
   - Download from: https://jrsoftware.org/isdl.php
   - Install with default options

2. **Create the release build**:
   ```cmd
   cd C:\TEMP\INTERCOM_2
   create-installer.bat
   ```

3. **Verify the output files**:
   - `publish\MediaNews-Intercom-Setup-v1.1.0.exe` (~80 MB)
   - `publish\MediaNews-Intercom-v1.1.0-Portable.zip` (~80 MB)

4. **Test the installer**:
   - Run the installer on a clean Windows VM (if possible)
   - Verify all features work correctly
   - Test the web interface

## Step 2: Create GitHub Repository

1. **Go to GitHub**: https://github.com/
2. **Create new repository**:
   - Click "+" → "New repository"
   - Repository name: `medianews-intercom`
   - Description: "Professional 8-channel audio intercom system with remote web control"
   - Visibility: Public
   - ✅ Add a README file: NO (we already have one)
   - ✅ Add .gitignore: NO (we have our own)
   - ✅ Choose a license: NO (we have MIT license)
   - Click "Create repository"

## Step 3: Initialize Git and Push Code

1. **Open Command Prompt** in project folder:
   ```cmd
   cd C:\TEMP\INTERCOM_2
   ```

2. **Initialize Git repository**:
   ```cmd
   git init
   git add .
   git commit -m "Initial release v1.1.0 - Professional 8-channel intercom with web control"
   ```

3. **Connect to GitHub** (replace YOUR_USERNAME):
   ```cmd
   git remote add origin https://github.com/YOUR_USERNAME/medianews-intercom.git
   git branch -M main
   git push -u origin main
   ```

4. **Verify**: Check that all files appear on GitHub (except those in .gitignore)

## Step 4: Create GitHub Release

1. **Go to your repository** on GitHub

2. **Navigate to Releases**:
   - Click "Releases" on the right sidebar
   - Click "Create a new release"

3. **Fill in Release Information**:
   - **Tag version**: `v1.1.0`
   - **Release title**: `MediaNews Intercom v1.1.0 - Remote Control & UX Improvements`
   - **Description**: Copy content from `RELEASE_NOTES.md`

4. **Upload Release Assets**:
   - Drag and drop `MediaNews-Intercom-Setup-v1.1.0.exe`
   - Drag and drop `MediaNews-Intercom-v1.1.0-Portable.zip`

5. **Final settings**:
   - ✅ Set as the latest release: YES
   - ✅ Create a discussion for this release: Optional
   - Click "Publish release"

## Step 5: Update Repository Settings

1. **Add Topics** (helps discoverability):
   - Go to repository main page
   - Click ⚙️ next to "About"
   - Add topics: `audio`, `intercom`, `asio`, `broadcasting`, `studio`, `csharp`, `dotnet`, `windows`

2. **Add Description**:
   - "Professional 8-channel audio intercom system with remote web control"

3. **Add Website**:
   - Leave blank or add your personal website

## Step 6: Create Additional Documentation

Consider creating these optional files for better project presentation:

1. **CONTRIBUTING.md**: Guidelines for contributors
2. **CODE_OF_CONDUCT.md**: Community guidelines
3. **SECURITY.md**: Security policy
4. **Wiki pages**: Detailed tutorials and guides

## Step 7: Promote Your Release

1. **Social Media** (optional):
   - Share on Twitter/X with #AudioEngineering #ASIO #Intercom
   - Post on relevant Reddit communities (r/audioengineering, r/livesound)
   - Share on audio production forums

2. **Audio Communities**:
   - Gearslutz forums
   - KVR Audio forums
   - Sound on Sound forums

## Quick Reference: Git Commands

```bash
# Check status
git status

# Add new files
git add .

# Commit changes
git commit -m "Description of changes"

# Push to GitHub
git push

# Create new tag
git tag v1.1.0
git push origin v1.1.0

# View commit history
git log --oneline
```

## Troubleshooting

### Issue: "Permission denied (publickey)"
**Solution**: Set up SSH keys or use HTTPS with Personal Access Token
```cmd
git remote set-url origin https://github.com/YOUR_USERNAME/medianews-intercom.git
```

### Issue: Large files not uploading
**Solution**: GitHub has a 100MB file size limit. Current files are ~80MB, which is fine.

### Issue: .gitignore not working
**Solution**: Clear git cache:
```cmd
git rm -r --cached .
git add .
git commit -m "Fix .gitignore"
```

## Next Steps After Release

1. **Monitor Issues**: Respond to bug reports and feature requests
2. **Plan v1.2**: Based on user feedback
3. **Update Documentation**: As needed
4. **Version Control**: Use semantic versioning (MAJOR.MINOR.PATCH)

---

## Release Checklist

Before publishing, verify:

- [ ] Version numbers updated everywhere (v1.1.0)
- [ ] README.md is complete and accurate
- [ ] LICENSE file is present
- [ ] .gitignore excludes build artifacts
- [ ] Installer tested on clean Windows machine
- [ ] Portable ZIP tested
- [ ] Web interface tested from remote device
- [ ] All features documented
- [ ] Known issues documented
- [ ] RELEASE_NOTES.md is accurate
- [ ] Git repository is clean (no unwanted files)
- [ ] All commits have meaningful messages

---

**Good luck with your release! 🚀**
