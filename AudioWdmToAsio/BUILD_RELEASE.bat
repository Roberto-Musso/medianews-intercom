@echo off
echo ================================================================================
echo MEDIANEWS INTERCOM - BUILD RELEASE v1.0.0
echo ================================================================================
echo.
echo Building self-contained release package...
echo This may take a few minutes...
echo.

REM Clean previous builds
echo [1/4] Cleaning previous builds...
if exist "bin\Release" rd /s /q "bin\Release"
if exist "obj\Release" rd /s /q "obj\Release"
if exist "..\Release" rd /s /q "..\Release"

REM Build self-contained release for Windows x64
echo.
echo [2/4] Building self-contained Windows x64 release...
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=false -p:PublishTrimmed=false -p:IncludeNativeLibrariesForSelfExtract=true

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo ERROR: Build failed!
    pause
    exit /b 1
)

REM Create release folder structure
echo.
echo [3/4] Creating release package...
mkdir "..\Release\MediaNews-Intercom-v1.0.0" 2>nul
mkdir "..\Release\MediaNews-Intercom-v1.0.0\Data" 2>nul

REM Copy binaries
xcopy /Y /E "bin\Release\net8.0-windows\win-x64\publish\*" "..\Release\MediaNews-Intercom-v1.0.0\"

REM Copy documentation
copy /Y "LICENSE.txt" "..\Release\MediaNews-Intercom-v1.0.0\"
copy /Y "README_RELEASE.txt" "..\Release\MediaNews-Intercom-v1.0.0\README.txt"

REM Copy icon
copy /Y "..\Graphic\Icon.png" "..\Release\MediaNews-Intercom-v1.0.0\Data\"

echo.
echo [4/4] Build complete!
echo.
echo Release package created in: ..\Release\MediaNews-Intercom-v1.0.0\
echo.
echo Package contents:
dir /b "..\Release\MediaNews-Intercom-v1.0.0\"
echo.
echo Total size:
powershell -Command "'{0:N2} MB' -f ((Get-ChildItem '..\Release\MediaNews-Intercom-v1.0.0' -Recurse | Measure-Object -Property Length -Sum).Sum / 1MB)"
echo.
echo ================================================================================
echo Next step: Run CREATE_INSTALLER.bat to create the installer
echo ================================================================================
pause
