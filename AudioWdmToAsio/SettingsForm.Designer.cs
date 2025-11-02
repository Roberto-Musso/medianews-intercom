namespace AudioWdmToAsio
{
    partial class SettingsForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            // Header
            this.panelHeader = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();

            // Main content panel
            this.panelContent = new System.Windows.Forms.Panel();

            // Audio Devices section
            this.labelSectionDevices = new System.Windows.Forms.Label();
            this.labelWdmInput = new System.Windows.Forms.Label();
            this.comboBoxWdmInput = new System.Windows.Forms.ComboBox();
            this.labelWdmOutput = new System.Windows.Forms.Label();
            this.comboBoxWdmOutput = new System.Windows.Forms.ComboBox();
            this.labelAsioDevice = new System.Windows.Forms.Label();
            this.comboBoxAsioDevice = new System.Windows.Forms.ComboBox();
            this.buttonAsioInfo = new System.Windows.Forms.Button();

            // Buffer section
            this.labelSectionBuffer = new System.Windows.Forms.Label();
            this.labelBuffer = new System.Windows.Forms.Label();
            this.trackBarBuffer = new System.Windows.Forms.TrackBar();
            this.labelBufferValue = new System.Windows.Forms.Label();
            this.checkBoxEnableDebug = new System.Windows.Forms.CheckBox();
            this.checkBoxShowLog = new System.Windows.Forms.CheckBox();

            // Noise Gate section
            this.labelSectionGate = new System.Windows.Forms.Label();
            this.checkBoxGateEnabled = new System.Windows.Forms.CheckBox();
            this.labelGateThreshold = new System.Windows.Forms.Label();
            this.trackBarGateThreshold = new System.Windows.Forms.TrackBar();
            this.labelGateThresholdValue = new System.Windows.Forms.Label();
            this.labelGateAttack = new System.Windows.Forms.Label();
            this.trackBarGateAttack = new System.Windows.Forms.TrackBar();
            this.labelGateAttackValue = new System.Windows.Forms.Label();
            this.labelGateRelease = new System.Windows.Forms.Label();
            this.trackBarGateRelease = new System.Windows.Forms.TrackBar();
            this.labelGateReleaseValue = new System.Windows.Forms.Label();

            // Channel Mapping section
            this.labelSectionMapping = new System.Windows.Forms.Label();
            this.panelChannelMappings = new System.Windows.Forms.Panel();

            // Footer buttons
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();

            this.panelHeader.SuspendLayout();
            this.panelContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBuffer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarGateThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarGateAttack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarGateRelease)).BeginInit();
            this.SuspendLayout();

            //
            // panelHeader
            //
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(20, 20, 20);
            this.panelHeader.Controls.Add(this.labelTitle);
            this.panelHeader.Controls.Add(this.buttonOK);
            this.panelHeader.Controls.Add(this.buttonCancel);
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1920, 80);
            this.panelHeader.TabIndex = 0;

            //
            // labelTitle
            //
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelTitle.ForeColor = System.Drawing.Color.White;
            this.labelTitle.Location = new System.Drawing.Point(20, 25);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(200, 32);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "Intercom Settings";

            //
            // buttonOK
            //
            this.buttonOK.BackColor = System.Drawing.Color.FromArgb(0, 150, 0);
            this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOK.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(0, 180, 0);
            this.buttonOK.FlatAppearance.BorderSize = 1;
            this.buttonOK.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.buttonOK.ForeColor = System.Drawing.Color.White;
            this.buttonOK.Location = new System.Drawing.Point(1540, 20);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(160, 45);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "SAVE";
            this.buttonOK.UseVisualStyleBackColor = false;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);

            //
            // buttonCancel
            //
            this.buttonCancel.BackColor = System.Drawing.Color.FromArgb(100, 100, 100);
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(130, 130, 130);
            this.buttonCancel.FlatAppearance.BorderSize = 1;
            this.buttonCancel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.buttonCancel.ForeColor = System.Drawing.Color.White;
            this.buttonCancel.Location = new System.Drawing.Point(1720, 20);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(160, 45);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "CANCEL";
            this.buttonCancel.UseVisualStyleBackColor = false;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);

            //
            // panelContent
            //
            this.panelContent.AutoScroll = true;
            this.panelContent.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.panelContent.Location = new System.Drawing.Point(20, 90);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(1880, 440);
            this.panelContent.TabIndex = 1;

            // Add all controls to content panel
            this.panelContent.Controls.Add(this.labelSectionDevices);
            this.panelContent.Controls.Add(this.labelWdmInput);
            this.panelContent.Controls.Add(this.comboBoxWdmInput);
            this.panelContent.Controls.Add(this.labelWdmOutput);
            this.panelContent.Controls.Add(this.comboBoxWdmOutput);
            this.panelContent.Controls.Add(this.labelAsioDevice);
            this.panelContent.Controls.Add(this.comboBoxAsioDevice);
            this.panelContent.Controls.Add(this.buttonAsioInfo);

            this.panelContent.Controls.Add(this.labelSectionBuffer);
            this.panelContent.Controls.Add(this.labelBuffer);
            this.panelContent.Controls.Add(this.trackBarBuffer);
            this.panelContent.Controls.Add(this.labelBufferValue);
            this.panelContent.Controls.Add(this.checkBoxEnableDebug);
            this.panelContent.Controls.Add(this.checkBoxShowLog);

            this.panelContent.Controls.Add(this.labelSectionGate);
            this.panelContent.Controls.Add(this.checkBoxGateEnabled);
            this.panelContent.Controls.Add(this.labelGateThreshold);
            this.panelContent.Controls.Add(this.trackBarGateThreshold);
            this.panelContent.Controls.Add(this.labelGateThresholdValue);
            this.panelContent.Controls.Add(this.labelGateAttack);
            this.panelContent.Controls.Add(this.trackBarGateAttack);
            this.panelContent.Controls.Add(this.labelGateAttackValue);
            this.panelContent.Controls.Add(this.labelGateRelease);
            this.panelContent.Controls.Add(this.trackBarGateRelease);
            this.panelContent.Controls.Add(this.labelGateReleaseValue);

            this.panelContent.Controls.Add(this.labelSectionMapping);
            this.panelContent.Controls.Add(this.panelChannelMappings);

            //
            // Section: Audio Devices (Column 1)
            //
            this.labelSectionDevices.AutoSize = false;
            this.labelSectionDevices.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            this.labelSectionDevices.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.labelSectionDevices.ForeColor = System.Drawing.Color.White;
            this.labelSectionDevices.Location = new System.Drawing.Point(15, 15);
            this.labelSectionDevices.Size = new System.Drawing.Size(580, 35);
            this.labelSectionDevices.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelSectionDevices.Text = "  AUDIO DEVICES";

            this.labelWdmInput.AutoSize = true;
            this.labelWdmInput.ForeColor = System.Drawing.Color.LightGray;
            this.labelWdmInput.Location = new System.Drawing.Point(25, 65);
            this.labelWdmInput.Text = "Microphone (WDM):";

            this.comboBoxWdmInput.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.comboBoxWdmInput.ForeColor = System.Drawing.Color.White;
            this.comboBoxWdmInput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxWdmInput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxWdmInput.Location = new System.Drawing.Point(25, 85);
            this.comboBoxWdmInput.Size = new System.Drawing.Size(550, 23);

            this.labelWdmOutput.AutoSize = true;
            this.labelWdmOutput.ForeColor = System.Drawing.Color.LightGray;
            this.labelWdmOutput.Location = new System.Drawing.Point(25, 120);
            this.labelWdmOutput.Text = "Speaker (WDM):";

            this.comboBoxWdmOutput.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.comboBoxWdmOutput.ForeColor = System.Drawing.Color.White;
            this.comboBoxWdmOutput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxWdmOutput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxWdmOutput.Location = new System.Drawing.Point(25, 140);
            this.comboBoxWdmOutput.Size = new System.Drawing.Size(550, 23);

            this.labelAsioDevice.AutoSize = true;
            this.labelAsioDevice.ForeColor = System.Drawing.Color.LightGray;
            this.labelAsioDevice.Location = new System.Drawing.Point(25, 175);
            this.labelAsioDevice.Text = "ASIO Device:";

            this.comboBoxAsioDevice.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.comboBoxAsioDevice.ForeColor = System.Drawing.Color.White;
            this.comboBoxAsioDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAsioDevice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxAsioDevice.Location = new System.Drawing.Point(25, 195);
            this.comboBoxAsioDevice.Size = new System.Drawing.Size(450, 23);
            this.comboBoxAsioDevice.SelectedIndexChanged += new System.EventHandler(this.comboBoxAsioDevice_SelectedIndexChanged);

            this.buttonAsioInfo.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            this.buttonAsioInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAsioInfo.ForeColor = System.Drawing.Color.White;
            this.buttonAsioInfo.Location = new System.Drawing.Point(490, 193);
            this.buttonAsioInfo.Size = new System.Drawing.Size(85, 27);
            this.buttonAsioInfo.Text = "Info";
            this.buttonAsioInfo.Click += new System.EventHandler(this.buttonAsioInfo_Click);

            //
            // Section: Buffer / Latency (Column 1)
            //
            this.labelSectionBuffer.AutoSize = false;
            this.labelSectionBuffer.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            this.labelSectionBuffer.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.labelSectionBuffer.ForeColor = System.Drawing.Color.White;
            this.labelSectionBuffer.Location = new System.Drawing.Point(15, 240);
            this.labelSectionBuffer.Size = new System.Drawing.Size(580, 35);
            this.labelSectionBuffer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelSectionBuffer.Text = "  BUFFER / LATENCY";

            this.labelBuffer.AutoSize = true;
            this.labelBuffer.ForeColor = System.Drawing.Color.LightGray;
            this.labelBuffer.Location = new System.Drawing.Point(25, 290);
            this.labelBuffer.Text = "Buffer Size:";

            this.trackBarBuffer.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.trackBarBuffer.Location = new System.Drawing.Point(120, 285);
            this.trackBarBuffer.Maximum = 500;
            this.trackBarBuffer.Minimum = 10;
            this.trackBarBuffer.Size = new System.Drawing.Size(380, 45);
            this.trackBarBuffer.TickFrequency = 50;
            this.trackBarBuffer.Value = 100;
            this.trackBarBuffer.Scroll += new System.EventHandler(this.trackBarBuffer_Scroll);

            this.labelBufferValue.AutoSize = true;
            this.labelBufferValue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelBufferValue.ForeColor = System.Drawing.Color.Cyan;
            this.labelBufferValue.Location = new System.Drawing.Point(510, 290);
            this.labelBufferValue.Text = "100 ms";

            this.checkBoxEnableDebug.AutoSize = true;
            this.checkBoxEnableDebug.ForeColor = System.Drawing.Color.LightGray;
            this.checkBoxEnableDebug.Location = new System.Drawing.Point(25, 335);
            this.checkBoxEnableDebug.Text = "Enable Debug Mode";

            this.checkBoxShowLog.AutoSize = true;
            this.checkBoxShowLog.ForeColor = System.Drawing.Color.LightGray;
            this.checkBoxShowLog.Location = new System.Drawing.Point(25, 360);
            this.checkBoxShowLog.Text = "Show Log Window";

            //
            // Section: Noise Gate (Column 2)
            //
            this.labelSectionGate.AutoSize = false;
            this.labelSectionGate.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            this.labelSectionGate.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.labelSectionGate.ForeColor = System.Drawing.Color.White;
            this.labelSectionGate.Location = new System.Drawing.Point(630, 15);
            this.labelSectionGate.Size = new System.Drawing.Size(580, 35);
            this.labelSectionGate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelSectionGate.Text = "  NOISE GATE (Microphone)";

            this.checkBoxGateEnabled.AutoSize = true;
            this.checkBoxGateEnabled.ForeColor = System.Drawing.Color.LightGray;
            this.checkBoxGateEnabled.Location = new System.Drawing.Point(640, 65);
            this.checkBoxGateEnabled.Text = "Enable Noise Gate";

            this.labelGateThreshold.AutoSize = true;
            this.labelGateThreshold.ForeColor = System.Drawing.Color.LightGray;
            this.labelGateThreshold.Location = new System.Drawing.Point(640, 100);
            this.labelGateThreshold.Text = "Threshold (dB):";

            this.trackBarGateThreshold.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.trackBarGateThreshold.Location = new System.Drawing.Point(760, 95);
            this.trackBarGateThreshold.Maximum = 0;
            this.trackBarGateThreshold.Minimum = -60;
            this.trackBarGateThreshold.Size = new System.Drawing.Size(380, 45);
            this.trackBarGateThreshold.TickFrequency = 10;
            this.trackBarGateThreshold.Value = -40;
            this.trackBarGateThreshold.Scroll += new System.EventHandler(this.trackBarGateThreshold_Scroll);

            this.labelGateThresholdValue.AutoSize = true;
            this.labelGateThresholdValue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelGateThresholdValue.ForeColor = System.Drawing.Color.Orange;
            this.labelGateThresholdValue.Location = new System.Drawing.Point(1150, 100);
            this.labelGateThresholdValue.Text = "-40 dB";

            this.labelGateAttack.AutoSize = true;
            this.labelGateAttack.ForeColor = System.Drawing.Color.LightGray;
            this.labelGateAttack.Location = new System.Drawing.Point(640, 145);
            this.labelGateAttack.Text = "Attack (ms):";

            this.trackBarGateAttack.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.trackBarGateAttack.Location = new System.Drawing.Point(760, 140);
            this.trackBarGateAttack.Maximum = 100;
            this.trackBarGateAttack.Minimum = 1;
            this.trackBarGateAttack.Size = new System.Drawing.Size(380, 45);
            this.trackBarGateAttack.TickFrequency = 10;
            this.trackBarGateAttack.Value = 10;
            this.trackBarGateAttack.Scroll += new System.EventHandler(this.trackBarGateAttack_Scroll);

            this.labelGateAttackValue.AutoSize = true;
            this.labelGateAttackValue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelGateAttackValue.ForeColor = System.Drawing.Color.Cyan;
            this.labelGateAttackValue.Location = new System.Drawing.Point(1150, 145);
            this.labelGateAttackValue.Text = "10 ms";

            this.labelGateRelease.AutoSize = true;
            this.labelGateRelease.ForeColor = System.Drawing.Color.LightGray;
            this.labelGateRelease.Location = new System.Drawing.Point(640, 190);
            this.labelGateRelease.Text = "Release (ms):";

            this.trackBarGateRelease.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.trackBarGateRelease.Location = new System.Drawing.Point(760, 185);
            this.trackBarGateRelease.Maximum = 1000;
            this.trackBarGateRelease.Minimum = 10;
            this.trackBarGateRelease.Size = new System.Drawing.Size(380, 45);
            this.trackBarGateRelease.TickFrequency = 100;
            this.trackBarGateRelease.Value = 200;
            this.trackBarGateRelease.Scroll += new System.EventHandler(this.trackBarGateRelease_Scroll);

            this.labelGateReleaseValue.AutoSize = true;
            this.labelGateReleaseValue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelGateReleaseValue.ForeColor = System.Drawing.Color.LightGreen;
            this.labelGateReleaseValue.Location = new System.Drawing.Point(1150, 190);
            this.labelGateReleaseValue.Text = "200 ms";

            //
            // Section: Channel Mapping (Column 2)
            //
            this.labelSectionMapping.AutoSize = false;
            this.labelSectionMapping.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            this.labelSectionMapping.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.labelSectionMapping.ForeColor = System.Drawing.Color.White;
            this.labelSectionMapping.Location = new System.Drawing.Point(630, 240);
            this.labelSectionMapping.Size = new System.Drawing.Size(580, 35);
            this.labelSectionMapping.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelSectionMapping.Text = "  ASIO CHANNEL MAPPING";

            this.panelChannelMappings.AutoScroll = true;
            this.panelChannelMappings.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            this.panelChannelMappings.Location = new System.Drawing.Point(640, 285);
            this.panelChannelMappings.Size = new System.Drawing.Size(560, 135);

            //
            // SettingsForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.ClientSize = new System.Drawing.Size(1920, 540);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panelContent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Intercom Settings";
            this.Load += new System.EventHandler(this.SettingsForm_Load);

            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelContent.ResumeLayout(false);
            this.panelContent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBuffer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarGateThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarGateAttack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarGateRelease)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panelContent;

        private System.Windows.Forms.Label labelSectionDevices;
        private System.Windows.Forms.Label labelWdmInput;
        private System.Windows.Forms.ComboBox comboBoxWdmInput;
        private System.Windows.Forms.Label labelWdmOutput;
        private System.Windows.Forms.ComboBox comboBoxWdmOutput;
        private System.Windows.Forms.Label labelAsioDevice;
        private System.Windows.Forms.ComboBox comboBoxAsioDevice;
        private System.Windows.Forms.Button buttonAsioInfo;

        private System.Windows.Forms.Label labelSectionBuffer;
        private System.Windows.Forms.Label labelBuffer;
        private System.Windows.Forms.TrackBar trackBarBuffer;
        private System.Windows.Forms.Label labelBufferValue;
        private System.Windows.Forms.CheckBox checkBoxEnableDebug;
        private System.Windows.Forms.CheckBox checkBoxShowLog;

        private System.Windows.Forms.Label labelSectionGate;
        private System.Windows.Forms.CheckBox checkBoxGateEnabled;
        private System.Windows.Forms.Label labelGateThreshold;
        private System.Windows.Forms.TrackBar trackBarGateThreshold;
        private System.Windows.Forms.Label labelGateThresholdValue;
        private System.Windows.Forms.Label labelGateAttack;
        private System.Windows.Forms.TrackBar trackBarGateAttack;
        private System.Windows.Forms.Label labelGateAttackValue;
        private System.Windows.Forms.Label labelGateRelease;
        private System.Windows.Forms.TrackBar trackBarGateRelease;
        private System.Windows.Forms.Label labelGateReleaseValue;

        private System.Windows.Forms.Label labelSectionMapping;
        private System.Windows.Forms.Panel panelChannelMappings;

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
    }
}
