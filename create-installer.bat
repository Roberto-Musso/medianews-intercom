@echo off
REM ========================================
REM MediaNews Intercom - Complete Build & Installer Creation
REM ========================================

echo.
echo ========================================
echo MediaNews Intercom v1.1
echo Complete Build and Installer Creation
echo ========================================
echo.

REM Check if Inno Setup is installed
set INNO_SETUP_PATH=C:\Program Files (x86)\Inno Setup 6\ISCC.exe
if not exist "%INNO_SETUP_PATH%" (
    set INNO_SETUP_PATH=C:\Program Files\Inno Setup 6\ISCC.exe
)

if not exist "%INNO_SETUP_PATH%" (
    echo.
    echo WARNING: Inno Setup not found!
    echo.
    echo Please download and install Inno Setup 6 from:
    echo https://jrsoftware.org/isdl.php
    echo.
    echo After installation, run this script again.
    echo.
    pause
    exit /b 1
)

echo Inno Setup found: %INNO_SETUP_PATH%
echo.

REM Step 1: Build the application
echo ========================================
echo Step 1: Building Release Version
echo ========================================
call publish-release.bat
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: Build failed!
    pause
    exit /b 1
)

REM Step 2: Create installer
echo.
echo ========================================
echo Step 2: Creating Installer with Inno Setup
echo ========================================
"%INNO_SETUP_PATH%" "installer-setup.iss"
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: Installer creation failed!
    pause
    exit /b 1
)

echo.
echo ========================================
echo SUCCESS! Build Complete
echo ========================================
echo.
echo Created files:
echo   1. Portable version: publish\MediaNews-Intercom-v1.1.0-Portable.zip
echo   2. Windows Installer: publish\MediaNews-Intercom-Setup-v1.1.0.exe
echo.
echo Ready for distribution!
echo.
echo Next steps:
echo   1. Test the installer on a clean Windows machine
echo   2. Create a GitHub release
echo   3. Upload both files to the release
echo.
pause
