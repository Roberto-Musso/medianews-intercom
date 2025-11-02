# Build Instructions - MediaNews Intercom v1.1.0

## Quick Start - Create Complete Release Package

### Prerequisites
1. **.NET 8.0 SDK** - Already installed ✅
2. **Inno Setup 6** - Download from: https://jrsoftware.org/isdl.php

### One-Command Build (After Installing Inno Setup)

```cmd
create-installer.bat
```

This single command will:
1. Clean previous builds
2. Publish self-contained release (includes .NET runtime)
3. Create portable ZIP package
4. Generate Windows installer (.exe)

**Output files** (in `publish\` folder):
- `MediaNews-Intercom-v1.1.0-Portable.zip` (~80 MB)
- `MediaNews-Intercom-Setup-v1.1.0.exe` (~80 MB)

---

## Manual Build Steps

If you prefer to build manually or troubleshoot:

### Step 1: Build Release
```cmd
publish-release.bat
```

This creates:
- Self-contained application in `publish\MediaNews-Intercom-v1.1.0\`
- Portable ZIP package

### Step 2: Create Installer (Requires Inno Setup)
```cmd
"C:\Program Files (x86)\Inno Setup 6\ISCC.exe" installer-setup.iss
```

This creates:
- `publish\MediaNews-Intercom-Setup-v1.1.0.exe`

---

## Development Builds

For testing during development:

### Debug Build
```cmd
dotnet build
```

### Run Debug Version
```cmd
dotnet run --project AudioWdmToAsio\AudioWdmToAsio.csproj
```

### Release Build (without publish)
```cmd
dotnet build --configuration Release
```

---

## Project Structure

```
C:\TEMP\INTERCOM_2\
├── AudioWdmToAsio\              # Main application source
│   ├── *.cs                     # C# source files
│   ├── *.Designer.cs            # Windows Forms designers
│   ├── AudioWdmToAsio.csproj    # Project file (v1.1.0)
│   └── app.ico                  # Application icon
│
├── publish\                      # Build output (created by scripts)
│   ├── MediaNews-Intercom-v1.1.0\              # Self-contained app
│   ├── MediaNews-Intercom-v1.1.0-Portable.zip  # ZIP package
│   └── MediaNews-Intercom-Setup-v1.1.0.exe     # Installer
│
├── README.md                     # Main documentation
├── LICENSE                       # MIT License
├── .gitignore                   # Git ignore rules
│
├── installer-setup.iss          # Inno Setup script
├── publish-release.bat          # Build script
├── create-installer.bat         # Complete build + installer
│
├── RELEASE_NOTES.md             # Release notes for GitHub
├── GITHUB_RELEASE_GUIDE.md      # Publishing guide
└── BUILD_INSTRUCTIONS.md        # This file
```

---

## Publishing Options

### Self-Contained vs Framework-Dependent

**Current configuration: Self-Contained** ✅

- **Pros**:
  - Users don't need to install .NET
  - Always uses the correct .NET version
  - No compatibility issues
- **Cons**:
  - Larger file size (~80 MB vs ~5 MB)

To switch to framework-dependent:
```cmd
dotnet publish --configuration Release --self-contained false
```

### Single File vs Multiple Files

**Current configuration: Multiple Files** ✅

- **Pros**:
  - Faster startup
  - Easier debugging
  - Native libraries work correctly
- **Cons**:
  - More files in installation folder

To create single-file:
Edit `publish-release.bat` and change:
```
-p:PublishSingleFile=true
```

---

## Troubleshooting

### Issue: "Inno Setup not found"
**Solution**: Install from https://jrsoftware.org/isdl.php
Default path: `C:\Program Files (x86)\Inno Setup 6\`

### Issue: "dotnet command not found"
**Solution**: Install .NET 8.0 SDK from https://dotnet.microsoft.com/download

### Issue: Build warnings about nullable types
**Expected**: These are harmless warnings in auto-generated code
**Action**: Can be ignored - they don't affect functionality

### Issue: Large installer size
**Expected**: ~80 MB is normal for self-contained .NET 8 apps
**Includes**:
- Application code (~2 MB)
- .NET 8 Runtime (~70 MB)
- NAudio libraries (~8 MB)

---

## Version Management

To release a new version (e.g., v1.2.0):

1. **Update version in**: `AudioWdmToAsio\AudioWdmToAsio.csproj`
   ```xml
   <Version>1.2.0</Version>
   <FileVersion>1.2.0.0</FileVersion>
   <AssemblyVersion>1.2.0.0</AssemblyVersion>
   ```

2. **Update installer script**: `installer-setup.iss`
   ```
   #define MyAppVersion "1.2.0"
   ```

3. **Update scripts**:
   - `publish-release.bat` (output folder names)
   - `create-installer.bat` (version references)

4. **Update documentation**:
   - README.md (version badge and changelog)
   - RELEASE_NOTES.md (new version info)

---

## Testing Checklist

Before releasing:

- [ ] Clean build successful (no errors)
- [ ] Application runs in Debug mode
- [ ] Application runs in Release mode
- [ ] Self-contained publish works
- [ ] ZIP package created successfully
- [ ] Installer created successfully
- [ ] Installer runs on clean Windows machine
- [ ] All features work after installation
- [ ] Web interface accessible
- [ ] Network access script works
- [ ] Desktop shortcut created
- [ ] Start Menu entries present
- [ ] Uninstaller works correctly

---

## Distribution

After successful build:

1. **Test Installer**: Run on clean VM or test machine
2. **Create GitHub Release**: Follow `GITHUB_RELEASE_GUIDE.md`
3. **Upload Files**:
   - `MediaNews-Intercom-Setup-v1.1.0.exe`
   - `MediaNews-Intercom-v1.1.0-Portable.zip`
4. **Announce Release**: Social media, forums, etc.

---

## Support

For build issues:
- Check .NET SDK installation: `dotnet --version`
- Verify Inno Setup: Check `C:\Program Files (x86)\Inno Setup 6\`
- Review build logs for specific errors

---

**Happy Building! 🛠️**
