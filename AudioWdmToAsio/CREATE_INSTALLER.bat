@echo off
echo ================================================================================
echo MEDIANEWS INTERCOM - CREATE INSTALLER
echo ================================================================================
echo.
echo Creating installer script...
echo.

REM Create the installer script in the release folder
(
echo @echo off
echo echo ================================================================================
echo echo MEDIANEWS INTERCOM v1.0.0 - INSTALLER
echo echo Copyright ^(c^) 2025 Roberto Musso
echo echo ================================================================================
echo echo.
echo echo This installer will install MediaNews Intercom to:
echo echo C:\Program Files\MediaNews-Intercom\
echo echo.
echo pause
echo.
echo REM Check for administrator privileges
echo net session ^>nul 2^>^&1
echo if %%ERRORLEVEL%% NEQ 0 ^(
echo     echo.
echo     echo ERROR: This installer requires administrator privileges.
echo     echo Please right-click on INSTALL.bat and select "Run as administrator"
echo     echo.
echo     pause
echo     exit /b 1
echo ^)
echo.
echo echo.
echo echo [1/4] Creating installation directory...
echo mkdir "C:\Program Files\MediaNews-Intercom" 2^>nul
echo.
echo echo [2/4] Copying files...
echo xcopy /Y /E /I ".\*" "C:\Program Files\MediaNews-Intercom\" /EXCLUDE:exclude.txt
echo.
echo echo [3/4] Creating desktop shortcut...
echo powershell -Command "$WshShell = New-Object -comObject WScript.Shell; $Shortcut = $WshShell.CreateShortcut(\"$env:USERPROFILE\Desktop\MediaNews Intercom.lnk\"^); $Shortcut.TargetPath = \"C:\Program Files\MediaNews-Intercom\MediaNews-Intercom.exe\"; $Shortcut.WorkingDirectory = \"C:\Program Files\MediaNews-Intercom\"; $Shortcut.IconLocation = \"C:\Program Files\MediaNews-Intercom\MediaNews-Intercom.exe,0\"; $Shortcut.Description = \"MediaNews Intercom - 8 Channel Audio Intercom System\"; $Shortcut.Save(^)"
echo.
echo echo [4/4] Creating uninstaller...
echo ^(
echo echo @echo off
echo echo Uninstalling MediaNews Intercom...
echo echo.
echo del "%%USERPROFILE%%\Desktop\MediaNews Intercom.lnk" 2^>nul
echo rd /s /q "C:\Program Files\MediaNews-Intercom"
echo rd /s /q "%%APPDATA%%\MediaNews-Intercom"
echo echo.
echo echo MediaNews Intercom has been uninstalled.
echo pause
echo ^) ^> "C:\Program Files\MediaNews-Intercom\UNINSTALL.bat"
echo.
echo echo.
echo echo ================================================================================
echo echo INSTALLATION COMPLETE!
echo echo ================================================================================
echo echo.
echo echo MediaNews Intercom has been installed successfully.
echo echo.
echo echo Installation folder: C:\Program Files\MediaNews-Intercom\
echo echo Desktop shortcut: Created
echo echo.
echo echo You can now:
echo echo 1. Double-click the desktop shortcut to launch the application
echo echo 2. Or run: C:\Program Files\MediaNews-Intercom\MediaNews-Intercom.exe
echo echo.
echo echo To uninstall: Run C:\Program Files\MediaNews-Intercom\UNINSTALL.bat
echo echo.
echo echo ================================================================================
echo pause
) > "..\Release\MediaNews-Intercom-v1.0.0\INSTALL.bat"

REM Create exclusion list for xcopy (to avoid copying the installer itself)
(
echo INSTALL.bat
echo UNINSTALL.bat
echo exclude.txt
) > "..\Release\MediaNews-Intercom-v1.0.0\exclude.txt"

echo.
echo Installer created successfully!
echo.
echo Location: ..\Release\MediaNews-Intercom-v1.0.0\INSTALL.bat
echo.
echo ================================================================================
echo RELEASE PACKAGE READY!
echo ================================================================================
echo.
echo The complete release package is ready in:
echo ..\Release\MediaNews-Intercom-v1.0.0\
echo.
echo To distribute:
echo 1. Compress the entire folder to a ZIP file
echo 2. Users extract the ZIP and run INSTALL.bat as administrator
echo.
echo ================================================================================
pause
