namespace AudioWdmToAsio;

partial class Form1
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

    private System.Windows.Forms.Timer debugTimer;

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.debugTimer = new System.Windows.Forms.Timer(this.components);

        this.panelHeader = new System.Windows.Forms.Panel();
        this.labelTitle = new System.Windows.Forms.Label();

        this.panelChannels = new System.Windows.Forms.Panel();

        // Channel controls arrays
        this.channelPanels = new List<Panel>();
        this.channelNameTextBoxes = new List<TextBox>();
        this.talkButtons = new List<Button>();
        this.listenButtons = new List<Button>();
        this.inputLevelKnobs = new List<RotaryControl>();
        this.outputLevelKnobs = new List<RotaryControl>();
        this.inputLevelLabels = new List<Label>();
        this.outputLevelLabels = new List<Label>();
        this.inputValueLabels = new List<Label>();
        this.outputValueLabels = new List<Label>();

        // VU Meter
        this.labelInputLevel = new System.Windows.Forms.Label();
        this.vuMeter = new VUMeterControl();

        this.buttonSettings = new System.Windows.Forms.Button();
        this.buttonStartStop = new System.Windows.Forms.Button();

        this.textBoxLog = new System.Windows.Forms.TextBox();

        this.panelHeader.SuspendLayout();
        this.SuspendLayout();

        //
        // panelHeader
        //
        this.panelHeader.BackColor = System.Drawing.Color.FromArgb(20, 20, 20);
        this.panelHeader.Controls.Add(this.labelTitle);
        this.panelHeader.Controls.Add(this.labelInputLevel);
        this.panelHeader.Controls.Add(this.vuMeter);
        this.panelHeader.Controls.Add(this.buttonSettings);
        this.panelHeader.Controls.Add(this.buttonStartStop);
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
        this.labelTitle.Size = new System.Drawing.Size(250, 32);
        this.labelTitle.TabIndex = 0;
        this.labelTitle.Text = "8 Channel Intercom";

        //
        // labelInputLevel
        //
        this.labelInputLevel.AutoSize = true;
        this.labelInputLevel.Font = new System.Drawing.Font("Segoe UI", 10F);
        this.labelInputLevel.ForeColor = System.Drawing.Color.LightGray;
        this.labelInputLevel.Location = new System.Drawing.Point(680, 15);
        this.labelInputLevel.Name = "labelInputLevel";
        this.labelInputLevel.Size = new System.Drawing.Size(80, 19);
        this.labelInputLevel.TabIndex = 1;
        this.labelInputLevel.Text = "Input Level";

        //
        // vuMeter
        //
        this.vuMeter.Location = new System.Drawing.Point(680, 38);
        this.vuMeter.Name = "vuMeter";
        this.vuMeter.Size = new System.Drawing.Size(460, 30);
        this.vuMeter.TabIndex = 2;

        //
        // buttonSettings
        //
        this.buttonSettings.BackColor = System.Drawing.Color.FromArgb(120, 90, 50);
        this.buttonSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.buttonSettings.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(150, 110, 60);
        this.buttonSettings.FlatAppearance.BorderSize = 1;
        this.buttonSettings.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
        this.buttonSettings.ForeColor = System.Drawing.Color.White;
        this.buttonSettings.Location = new System.Drawing.Point(1180, 20);
        this.buttonSettings.Name = "buttonSettings";
        this.buttonSettings.Size = new System.Drawing.Size(180, 45);
        this.buttonSettings.TabIndex = 3;
        this.buttonSettings.Text = "SETTINGS";
        this.buttonSettings.UseVisualStyleBackColor = false;
        this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);

        //
        // buttonStartStop
        //
        this.buttonStartStop.BackColor = System.Drawing.Color.FromArgb(0, 150, 0);
        this.buttonStartStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.buttonStartStop.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(0, 180, 0);
        this.buttonStartStop.FlatAppearance.BorderSize = 1;
        this.buttonStartStop.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
        this.buttonStartStop.ForeColor = System.Drawing.Color.White;
        this.buttonStartStop.Location = new System.Drawing.Point(1380, 20);
        this.buttonStartStop.Name = "buttonStartStop";
        this.buttonStartStop.Size = new System.Drawing.Size(180, 45);
        this.buttonStartStop.TabIndex = 4;
        this.buttonStartStop.Text = "START";
        this.buttonStartStop.UseVisualStyleBackColor = false;
        this.buttonStartStop.Click += new System.EventHandler(this.buttonStartStop_Click);

        //
        // panelChannels
        //
        this.panelChannels.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
        this.panelChannels.Location = new System.Drawing.Point(20, 90);
        this.panelChannels.Name = "panelChannels";
        this.panelChannels.Size = new System.Drawing.Size(1880, 360);
        this.panelChannels.TabIndex = 1;

        // Create 8 channel strips
        CreateChannelStrips();

        //
        // textBoxLog
        //
        this.textBoxLog.BackColor = System.Drawing.Color.Black;
        this.textBoxLog.Font = new System.Drawing.Font("Consolas", 8F);
        this.textBoxLog.ForeColor = System.Drawing.Color.Lime;
        this.textBoxLog.Location = new System.Drawing.Point(20, 460);
        this.textBoxLog.Multiline = true;
        this.textBoxLog.Name = "textBoxLog";
        this.textBoxLog.ReadOnly = true;
        this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        this.textBoxLog.Size = new System.Drawing.Size(1880, 70);
        this.textBoxLog.TabIndex = 2;
        this.textBoxLog.Visible = false;  // Hidden by default

        //
        // debugTimer
        //
        this.debugTimer.Interval = 100; // Update every 100ms
        this.debugTimer.Tick += new System.EventHandler(this.debugTimer_Tick);
        this.debugTimer.Enabled = true;

        //
        // Form1
        //
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
        this.ClientSize = new System.Drawing.Size(1920, 540);  // 16:9 aspect ratio
        this.Controls.Add(this.panelHeader);
        this.Controls.Add(this.panelChannels);
        this.Controls.Add(this.textBoxLog);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;  // Fixed size
        this.MaximizeBox = false;
        this.MinimizeBox = true;
        this.Name = "Form1";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Audio Intercom - 8 Channel";
        this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
        this.Load += new System.EventHandler(this.Form1_Load);
        this.panelHeader.ResumeLayout(false);
        this.panelHeader.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private void CreateChannelStrips()
    {
        int channelWidth = 223;
        int channelHeight = 340;
        int spacing = 10;
        int startX = 15;
        int startY = 10;

        for (int i = 0; i < 8; i++)
        {
            int x = startX + (i * (channelWidth + spacing));
            int y = startY;

            Panel channelPanel = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(channelWidth, channelHeight),
                BackColor = System.Drawing.Color.FromArgb(45, 45, 48),
                BorderStyle = BorderStyle.FixedSingle
            };

            TextBox channelNameTextBox = new TextBox
            {
                Text = $"Channel {i + 1}",
                Location = new Point(10, 10),
                Size = new Size(205, 30),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = HorizontalAlignment.Center,
                BackColor = Color.FromArgb(30, 35, 40),
                BorderStyle = BorderStyle.FixedSingle,
                Tag = i
            };
            channelNameTextBox.TextChanged += ChannelName_TextChanged;

            Button talkButton = new Button
            {
                Text = "TALK",
                Location = new Point(15, 50),
                Size = new Size(90, 60),
                BackColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Tag = i
            };
            talkButton.FlatAppearance.BorderColor = Color.FromArgb(80, 80, 80);
            talkButton.FlatAppearance.BorderSize = 2;
            talkButton.Click += TalkButton_Click;

            Button listenButton = new Button
            {
                Text = "LISTEN",
                Location = new Point(120, 50),
                Size = new Size(90, 60),
                BackColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Tag = i
            };
            listenButton.FlatAppearance.BorderColor = Color.FromArgb(80, 80, 80);
            listenButton.FlatAppearance.BorderSize = 2;
            listenButton.Click += ListenButton_Click;

            Label inputLevelLabel = new Label
            {
                Text = "Input Level",
                Location = new Point(15, 130),
                Size = new Size(85, 18),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.LightGray,
                TextAlign = ContentAlignment.MiddleCenter
            };

            RotaryControl inputLevelKnob = new RotaryControl
            {
                Location = new Point(20, 155),
                Size = new Size(80, 80),
                Minimum = 0,
                Maximum = 130,
                Value = 100,
                Tag = i,
                IndicatorColor = Color.Cyan,
                ArcColor = Color.FromArgb(0, 200, 255)
            };
            inputLevelKnob.ValueChanged += InputLevel_Changed;

            Label inputValueLabel = new Label
            {
                Text = "100",
                Location = new Point(15, 240),
                Size = new Size(85, 20),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.Cyan,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label outputLevelLabel = new Label
            {
                Text = "Output Level",
                Location = new Point(115, 130),
                Size = new Size(90, 18),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.LightGray,
                TextAlign = ContentAlignment.MiddleCenter
            };

            RotaryControl outputLevelKnob = new RotaryControl
            {
                Location = new Point(125, 155),
                Size = new Size(80, 80),
                Minimum = 0,
                Maximum = 130,
                Value = 100,
                Tag = i,
                IndicatorColor = Color.Orange,
                ArcColor = Color.FromArgb(255, 150, 0)
            };
            outputLevelKnob.ValueChanged += OutputLevel_Changed;

            Label outputValueLabel = new Label
            {
                Text = "100",
                Location = new Point(115, 240),
                Size = new Size(90, 20),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.Orange,
                TextAlign = ContentAlignment.MiddleCenter
            };

            channelPanel.Controls.Add(channelNameTextBox);
            channelPanel.Controls.Add(talkButton);
            channelPanel.Controls.Add(listenButton);
            channelPanel.Controls.Add(inputLevelLabel);
            channelPanel.Controls.Add(inputLevelKnob);
            channelPanel.Controls.Add(inputValueLabel);
            channelPanel.Controls.Add(outputLevelLabel);
            channelPanel.Controls.Add(outputLevelKnob);
            channelPanel.Controls.Add(outputValueLabel);

            panelChannels.Controls.Add(channelPanel);

            // Store references
            channelPanels.Add(channelPanel);
            channelNameTextBoxes.Add(channelNameTextBox);
            talkButtons.Add(talkButton);
            listenButtons.Add(listenButton);
            inputLevelKnobs.Add(inputLevelKnob);
            outputLevelKnobs.Add(outputLevelKnob);
            inputLevelLabels.Add(inputLevelLabel);
            outputLevelLabels.Add(outputLevelLabel);
            inputValueLabels.Add(inputValueLabel);
            outputValueLabels.Add(outputValueLabel);
        }
    }

    // Event handlers for channel name editing
    private void ChannelName_TextChanged(object? sender, EventArgs e)
    {
        if (sender is TextBox textBox && textBox.Tag is int channelIndex)
        {
            if (channelIndex >= 0 && channelIndex < settings.Channels.Count)
            {
                settings.Channels[channelIndex].ChannelName = textBox.Text;
                SaveSettingsQuietly();
            }
        }
    }

    // Event handlers for rotary controls
    private void InputLevel_Changed(object? sender, EventArgs e)
    {
        if (sender is RotaryControl knob && knob.Tag is int channelIndex)
        {
            if (channelIndex >= 0 && channelIndex < settings.Channels.Count)
            {
                settings.Channels[channelIndex].InputLevel = knob.Value;
                inputValueLabels[channelIndex].Text = knob.Value.ToString();
                SaveSettingsQuietly();
            }
        }
    }

    private void OutputLevel_Changed(object? sender, EventArgs e)
    {
        if (sender is RotaryControl knob && knob.Tag is int channelIndex)
        {
            if (channelIndex >= 0 && channelIndex < settings.Channels.Count)
            {
                settings.Channels[channelIndex].OutputLevel = knob.Value;
                outputValueLabels[channelIndex].Text = knob.Value.ToString();
                SaveSettingsQuietly();
            }
        }
    }

    #endregion

    private Panel panelHeader;
    private Label labelTitle;
    private Label labelInputLevel;
    private VUMeterControl vuMeter;
    private Button buttonSettings;
    private Button buttonStartStop;

    private Panel panelChannels;

    // Channel controls
    private List<Panel> channelPanels;
    private List<TextBox> channelNameTextBoxes;
    private List<Button> talkButtons;
    private List<Button> listenButtons;
    private List<RotaryControl> inputLevelKnobs;
    private List<RotaryControl> outputLevelKnobs;
    private List<Label> inputLevelLabels;
    private List<Label> outputLevelLabels;
    private List<Label> inputValueLabels;
    private List<Label> outputValueLabels;

    private TextBox textBoxLog;
}
