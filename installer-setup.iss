; MediaNews Intercom - Inno Setup Script
; Version 1.1.0

#define MyAppName "MediaNews Intercom"
#define MyAppVersion "1.1.0"
#define MyAppPublisher "Roberto Musso"
#define MyAppURL "https://github.com/robertomusso/medianews-intercom"
#define MyAppExeName "MediaNews-Intercom.exe"
#define MyAppDescription "8-Channel Professional Audio Intercom System"

[Setup]
; Basic application information
AppId={{8F7D9A3B-1E4C-4B5A-9D2E-6F3A8C1B4E7D}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}/issues
AppUpdatesURL={#MyAppURL}/releases
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
LicenseFile=LICENSE
OutputDir=publish
OutputBaseFilename=MediaNews-Intercom-Setup-v{#MyAppVersion}
SetupIconFile=AudioWdmToAsio\app.ico
Compression=lzma2/ultra64
SolidCompression=yes
WizardStyle=modern
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible

; Privileges
PrivilegesRequired=admin
PrivilegesRequiredOverridesAllowed=dialog

; Visual settings
; Using default modern wizard images

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "italian"; MessagesFile: "compiler:Languages\Italian.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 6.1; Check: not IsAdminInstallMode
Name: "networkaccess"; Description: "Enable network access for web interface (requires admin)"; GroupDescription: "Network Configuration:"; Flags: checkedonce

[Files]
; Main application files
Source: "publish\MediaNews-Intercom-v{#MyAppVersion}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; Documentation
Source: "README.md"; DestDir: "{app}"; Flags: ignoreversion
Source: "LICENSE"; DestDir: "{app}"; Flags: ignoreversion
; Network setup script
Source: "AudioWdmToAsio\ENABLE_NETWORK_ACCESS.bat"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\Enable Network Access"; Filename: "{app}\ENABLE_NETWORK_ACCESS.bat"; IconFilename: "{sys}\imageres.dll"; IconIndex: 1
Name: "{group}\README"; Filename: "{app}\README.md"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: quicklaunchicon

[Run]
; Configure network access if selected
Filename: "netsh"; Parameters: "http add urlacl url=http://+:8080/ user=Everyone"; Flags: runhidden; Tasks: networkaccess; StatusMsg: "Configuring HTTP access..."
Filename: "netsh"; Parameters: "advfirewall firewall delete rule name=""MediaNews Intercom Web 8080"""; Flags: runhidden; Tasks: networkaccess
Filename: "netsh"; Parameters: "advfirewall firewall add rule name=""MediaNews Intercom Web 8080"" dir=in action=allow protocol=TCP localport=8080"; Flags: runhidden; Tasks: networkaccess; StatusMsg: "Adding firewall rule..."
; Launch application after install
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[UninstallRun]
; Clean up network configuration on uninstall
Filename: "netsh"; Parameters: "http delete urlacl url=http://+:8080/"; Flags: runhidden; RunOnceId: "CleanupHTTP"
Filename: "netsh"; Parameters: "advfirewall firewall delete rule name=""MediaNews Intercom Web 8080"""; Flags: runhidden; RunOnceId: "CleanupFirewall"

[Code]
procedure InitializeWizard;
var
  WelcomePage: TWizardPage;
begin
  // Custom welcome message
  WizardForm.WelcomeLabel2.Caption :=
    'This will install {#MyAppName} v{#MyAppVersion} on your computer.' + #13#10#13#10 +
    'MediaNews Intercom is a professional 8-channel audio intercom system ' +
    'with remote web control capabilities.' + #13#10#13#10 +
    'Requirements:' + #13#10 +
    '  • Windows 10/11 (64-bit)' + #13#10 +
    '  • ASIO audio driver' + #13#10 +
    '  • Audio interface with 8+ channels' + #13#10#13#10 +
    'Click Next to continue, or Cancel to exit Setup.';
end;

function NextButtonClick(CurPageID: Integer): Boolean;
begin
  Result := True;

  // Show information about network access on the Ready page
  if CurPageID = wpReady then
  begin
    if IsTaskSelected('networkaccess') then
    begin
      MsgBox('Network access will be configured during installation.' + #13#10#13#10 +
             'After installation, you can access the web interface from any device on your local network.' + #13#10#13#10 +
             'The application will show you the URL to use.',
             mbInformation, MB_OK);
    end
    else
    begin
      MsgBox('Network access was NOT selected.' + #13#10#13#10 +
             'The web interface will only be accessible from this computer (localhost).' + #13#10#13#10 +
             'You can enable network access later by running "Enable Network Access" from the Start Menu.',
             mbInformation, MB_OK);
    end;
  end;
end;

procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep = ssPostInstall then
  begin
    // Installation completed successfully
  end;
end;

function InitializeUninstall(): Boolean;
begin
  Result := True;
  if MsgBox('Do you want to remove network firewall rules and HTTP configuration?',
            mbConfirmation, MB_YESNO) = IDNO then
  begin
    Result := True; // Continue with uninstall but skip network cleanup
  end;
end;
