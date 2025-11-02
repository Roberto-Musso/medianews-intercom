using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AudioWdmToAsio
{
    /// <summary>
    /// Custom VU Meter control for displaying audio levels
    /// </summary>
    public class VUMeterControl : Control
    {
        private float _level = 0.0f; // 0.0 to 1.0
        private float _peakLevel = 0.0f;
        private System.Windows.Forms.Timer _peakDecayTimer;
        private const float PEAK_DECAY_RATE = 0.95f;

        public VUMeterControl()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.UserPaint |
                         ControlStyles.ResizeRedraw |
                         ControlStyles.DoubleBuffer, true);

            this.Size = new Size(400, 30);
            this.BackColor = Color.FromArgb(20, 20, 20);

            // Timer for peak decay
            _peakDecayTimer = new System.Windows.Forms.Timer();
            _peakDecayTimer.Interval = 50; // 20 FPS
            _peakDecayTimer.Tick += (s, e) =>
            {
                if (_peakLevel > _level)
                {
                    _peakLevel *= PEAK_DECAY_RATE;
                    if (_peakLevel < _level) _peakLevel = _level;
                    Invalidate();
                }
            };
            _peakDecayTimer.Start();
        }

        /// <summary>
        /// Set the current level (0.0 to 1.0)
        /// </summary>
        public float Level
        {
            get => _level;
            set
            {
                _level = Math.Max(0, Math.Min(1, value));
                if (_level > _peakLevel) _peakLevel = _level;
                Invalidate();
            }
        }

        /// <summary>
        /// Set level from decibels (-60 to 0 dB)
        /// </summary>
        public void SetLevelDb(float db)
        {
            // Convert dB to linear scale (0.0 to 1.0)
            // -60 dB = 0.0, 0 dB = 1.0
            float normalized = (db + 60f) / 60f;
            Level = Math.Max(0, Math.Min(1, normalized));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Draw background
            using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(30, 30, 30)))
            {
                Rectangle bgRect = new Rectangle(0, 0, Width, Height);
                g.FillRectangle(bgBrush, bgRect);
            }

            // Draw border
            using (Pen borderPen = new Pen(Color.FromArgb(80, 80, 80), 1))
            {
                g.DrawRectangle(borderPen, 0, 0, Width - 1, Height - 1);
            }

            if (_level > 0)
            {
                int levelWidth = (int)(Width * _level);

                // Create gradient brush (green -> yellow -> red)
                using (LinearGradientBrush gradientBrush = new LinearGradientBrush(
                    new Rectangle(0, 0, Width, Height),
                    Color.Green,
                    Color.Red,
                    LinearGradientMode.Horizontal))
                {
                    // Define color blend for smooth gradient
                    ColorBlend colorBlend = new ColorBlend();
                    colorBlend.Colors = new Color[]
                    {
                        Color.FromArgb(0, 200, 0),      // Green at 0%
                        Color.FromArgb(100, 255, 0),    // Green-Yellow at 40%
                        Color.FromArgb(255, 255, 0),    // Yellow at 70%
                        Color.FromArgb(255, 150, 0),    // Orange at 85%
                        Color.FromArgb(255, 0, 0)       // Red at 100%
                    };
                    colorBlend.Positions = new float[] { 0.0f, 0.4f, 0.7f, 0.85f, 1.0f };
                    gradientBrush.InterpolationColors = colorBlend;

                    // Draw level bar
                    Rectangle levelRect = new Rectangle(1, 1, levelWidth - 1, Height - 2);
                    g.FillRectangle(gradientBrush, levelRect);
                }
            }

            // Draw peak indicator (white line)
            if (_peakLevel > 0.01f)
            {
                int peakX = (int)(Width * _peakLevel);
                using (Pen peakPen = new Pen(Color.White, 2))
                {
                    g.DrawLine(peakPen, peakX, 0, peakX, Height);
                }
            }

            // Draw scale marks
            using (Pen scalePen = new Pen(Color.FromArgb(100, 100, 100), 1))
            {
                for (int i = 1; i < 10; i++)
                {
                    int x = (Width * i) / 10;
                    int markHeight = (i % 5 == 0) ? Height / 3 : Height / 5;
                    g.DrawLine(scalePen, x, Height - markHeight, x, Height);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _peakDecayTimer?.Stop();
                _peakDecayTimer?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
