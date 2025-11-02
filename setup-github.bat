@echo off
REM ========================================
REM MediaNews Intercom - GitHub Setup
REM Automated script for Roberto-Musso
REM ========================================

echo.
echo ========================================
echo MediaNews Intercom - GitHub Setup
echo ========================================
echo.

REM Check if git is installed
where git >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: Git is not installed!
    echo Please install Git from: https://git-scm.com/download/win
    pause
    exit /b 1
)

echo [Step 1/6] Configuring Git...
git config --global user.name "Roberto Musso"
git config --global user.email "your.email@example.com"
echo IMPORTANT: If the email above is wrong, edit it with:
echo   git config --global user.email "correct@email.com"
echo.

echo [Step 2/6] Initializing Git repository...
if not exist ".git" (
    git init
    echo Repository initialized.
) else (
    echo Repository already initialized.
)
echo.

echo [Step 3/6] Adding files...
git add .
echo.

echo [Step 4/6] Creating first commit...
git commit -m "Initial release v1.1.0 - Professional 8-channel intercom with web control"
echo.

echo [Step 5/6] Setting up remote repository...
git remote remove origin >nul 2>&1
git remote add origin https://github.com/Roberto-Musso/medianews-intercom.git
echo Remote added: https://github.com/Roberto-Musso/medianews-intercom
echo.

echo [Step 6/6] Renaming branch to main...
git branch -M main
echo.

echo ========================================
echo SETUP COMPLETE!
echo ========================================
echo.
echo Next steps:
echo.
echo 1. CREATE REPOSITORY ON GITHUB:
echo    Go to: https://github.com/new
echo    Repository name: medianews-intercom
echo    Description: Professional 8-channel audio intercom system with remote web control
echo    Visibility: Public
echo    DO NOT add README, .gitignore, or license (we have them)
echo    Click "Create repository"
echo.
echo 2. PUSH TO GITHUB:
echo    Run: git push -u origin main
echo.
echo 3. WHEN GIT ASKS FOR CREDENTIALS:
echo    Username: Roberto-Musso
echo    Password: Use a Personal Access Token (NOT your password!)
echo.
echo    How to get a token:
echo    - Go to: https://github.com/settings/tokens
echo    - Click "Generate new token (classic)"
echo    - Select scope: repo (full control)
echo    - Generate and COPY the token
echo    - Use it as password when Git asks
echo.
echo Your repository will be at:
echo https://github.com/Roberto-Musso/medianews-intercom
echo.
pause
