@echo off
REM ========================================
REM MediaNews Intercom - Enable Network Access
REM ========================================
REM This script must be run as Administrator
REM Right-click -> Run as Administrator

echo.
echo ========================================
echo MediaNews Intercom - Network Setup
echo ========================================
echo.
echo This script will configure Windows to allow
echo remote access to the web interface.
echo.
echo Press any key to continue or Ctrl+C to cancel...
pause > nul

echo.
echo [1/3] Adding HTTP URL reservation...
netsh http add urlacl url=http://+:8080/ user=Everyone
if %ERRORLEVEL% EQU 0 (
    echo SUCCESS: URL reservation added
) else (
    echo WARNING: URL reservation failed or already exists
)

echo.
echo [2/3] Adding Windows Firewall rule...
netsh advfirewall firewall delete rule name="MediaNews Intercom Web 8080" > nul 2>&1
netsh advfirewall firewall add rule name="MediaNews Intercom Web 8080" dir=in action=allow protocol=TCP localport=8080
if %ERRORLEVEL% EQU 0 (
    echo SUCCESS: Firewall rule added
) else (
    echo ERROR: Failed to add firewall rule
)

echo.
echo [3/3] Getting local IP address...
for /f "tokens=2 delims=:" %%a in ('ipconfig ^| findstr /c:"IPv4"') do (
    set IP=%%a
    goto :found
)
:found
set IP=%IP:~1%
echo Your local IP address: %IP%

echo.
echo ========================================
echo SETUP COMPLETE!
echo ========================================
echo.
echo You can now access the web interface from other devices at:
echo   http://%IP%:8080/
echo.
echo Make sure both devices are on the same network!
echo.
echo Press any key to exit...
pause > nul
