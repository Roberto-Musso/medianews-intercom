@echo off
REM ========================================
REM MediaNews Intercom - Build Release v1.1
REM ========================================

echo.
echo ========================================
echo MediaNews Intercom v1.1 - Release Build
echo ========================================
echo.

REM Clean previous builds
echo [1/4] Cleaning previous builds...
if exist "publish" rd /s /q "publish"
if exist "AudioWdmToAsio\bin\Release" rd /s /q "AudioWdmToAsio\bin\Release"
if exist "AudioWdmToAsio\obj\Release" rd /s /q "AudioWdmToAsio\obj\Release"

REM Publish self-contained with .NET runtime included
echo.
echo [2/4] Publishing self-contained release...
dotnet publish AudioWdmToAsio\AudioWdmToAsio.csproj ^
    --configuration Release ^
    --runtime win-x64 ^
    --self-contained true ^
    --output "publish\MediaNews-Intercom-v1.1.0" ^
    -p:PublishSingleFile=false ^
    -p:PublishReadyToRun=true ^
    -p:IncludeNativeLibrariesForSelfExtract=true

if %ERRORLEVEL% NEQ 0 (
    echo ERROR: Build failed!
    pause
    exit /b 1
)

REM Copy additional files
echo.
echo [3/4] Copying additional files...
copy "README.md" "publish\MediaNews-Intercom-v1.1.0\"
copy "LICENSE" "publish\MediaNews-Intercom-v1.1.0\"
copy "AudioWdmToAsio\ENABLE_NETWORK_ACCESS.bat" "publish\MediaNews-Intercom-v1.1.0\"

REM Create ZIP for portable version
echo.
echo [4/4] Creating portable ZIP package...
powershell -Command "Compress-Archive -Path 'publish\MediaNews-Intercom-v1.1.0\*' -DestinationPath 'publish\MediaNews-Intercom-v1.1.0-Portable.zip' -Force"

echo.
echo ========================================
echo BUILD COMPLETE!
echo ========================================
echo.
echo Output folder: publish\MediaNews-Intercom-v1.1.0
echo Portable ZIP:  publish\MediaNews-Intercom-v1.1.0-Portable.zip
echo.
echo Next step: Run Inno Setup to create installer
echo.
pause
