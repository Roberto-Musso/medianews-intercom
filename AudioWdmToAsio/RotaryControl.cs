using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AudioWdmToAsio
{
    /// <summary>
    /// Custom rotary knob control for audio level adjustment
    /// </summary>
    public class RotaryControl : Control
    {
        private int _minimum = 0;
        private int _maximum = 130;
        private int _value = 100;
        private bool _isDragging = false;
        private Point _lastMousePos;
        private Color _knobColor = Color.FromArgb(60, 60, 60);
        private Color _indicatorColor = Color.Cyan;
        private Color _arcColor = Color.FromArgb(0, 150, 200);
        private TextBox? _inputTextBox = null;

        public event EventHandler? ValueChanged;

        public RotaryControl()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.UserPaint |
                         ControlStyles.ResizeRedraw |
                         ControlStyles.DoubleBuffer, true);
            this.Size = new Size(80, 80);
        }

        public int Minimum
        {
            get => _minimum;
            set
            {
                _minimum = value;
                if (_value < _minimum) Value = _minimum;
                Invalidate();
            }
        }

        public int Maximum
        {
            get => _maximum;
            set
            {
                _maximum = value;
                if (_value > _maximum) Value = _maximum;
                Invalidate();
            }
        }

        public int Value
        {
            get => _value;
            set
            {
                int newValue = Math.Max(_minimum, Math.Min(_maximum, value));
                if (_value != newValue)
                {
                    _value = newValue;
                    Invalidate();
                    ValueChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public Color KnobColor
        {
            get => _knobColor;
            set { _knobColor = value; Invalidate(); }
        }

        public Color IndicatorColor
        {
            get => _indicatorColor;
            set { _indicatorColor = value; Invalidate(); }
        }

        public Color ArcColor
        {
            get => _arcColor;
            set { _arcColor = value; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int size = Math.Min(Width, Height);
            int centerX = Width / 2;
            int centerY = Height / 2;
            int radius = (size - 10) / 2;

            // Calculate angle based on value (270 degrees range, -135 to +135)
            float percentage = (float)(_value - _minimum) / (_maximum - _minimum);
            float angle = -135 + (percentage * 270);

            // Draw background arc track
            using (Pen trackPen = new Pen(Color.FromArgb(40, 40, 40), 4))
            {
                Rectangle arcRect = new Rectangle(centerX - radius, centerY - radius, radius * 2, radius * 2);
                g.DrawArc(trackPen, arcRect, -135, 270);
            }

            // Draw value arc
            using (Pen arcPen = new Pen(_arcColor, 4))
            {
                Rectangle arcRect = new Rectangle(centerX - radius, centerY - radius, radius * 2, radius * 2);
                float sweepAngle = percentage * 270;
                g.DrawArc(arcPen, arcRect, -135, sweepAngle);
            }

            // Draw knob circle
            int knobRadius = radius - 8;
            using (SolidBrush knobBrush = new SolidBrush(_knobColor))
            {
                Rectangle knobRect = new Rectangle(centerX - knobRadius, centerY - knobRadius,
                                                   knobRadius * 2, knobRadius * 2);
                g.FillEllipse(knobBrush, knobRect);
            }

            // Draw knob border
            using (Pen borderPen = new Pen(Color.FromArgb(80, 80, 80), 2))
            {
                Rectangle knobRect = new Rectangle(centerX - knobRadius, centerY - knobRadius,
                                                   knobRadius * 2, knobRadius * 2);
                g.DrawEllipse(borderPen, knobRect);
            }

            // Draw indicator line
            double angleRad = angle * Math.PI / 180.0;
            int indicatorLength = knobRadius - 5;
            int startX = centerX + (int)(5 * Math.Cos(angleRad));
            int startY = centerY + (int)(5 * Math.Sin(angleRad));
            int endX = centerX + (int)(indicatorLength * Math.Cos(angleRad));
            int endY = centerY + (int)(indicatorLength * Math.Sin(angleRad));

            using (Pen indicatorPen = new Pen(_indicatorColor, 3))
            {
                indicatorPen.StartCap = LineCap.Round;
                indicatorPen.EndCap = LineCap.Round;
                g.DrawLine(indicatorPen, startX, startY, endX, endY);
            }

            // Draw center dot
            using (SolidBrush dotBrush = new SolidBrush(Color.FromArgb(30, 30, 30)))
            {
                g.FillEllipse(dotBrush, centerX - 3, centerY - 3, 6, 6);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                _isDragging = true;
                _lastMousePos = e.Location;
                Cursor = Cursors.Hand;
                this.Capture = true; // Capture mouse events even when outside control bounds
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_isDragging)
            {
                int deltaY = _lastMousePos.Y - e.Y;
                int deltaX = e.X - _lastMousePos.X;
                int delta = deltaY + deltaX;

                int sensitivity = (_maximum - _minimum) / 100;
                if (sensitivity < 1) sensitivity = 1;

                Value += delta * sensitivity / 10;
                _lastMousePos = e.Location;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Left)
            {
                _isDragging = false;
                Cursor = Cursors.Default;
                this.Capture = false; // Release mouse capture
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            // Don't reset dragging here - capture will keep it working
            // Only reset cursor if not dragging
            if (!_isDragging)
            {
                Cursor = Cursors.Default;
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            int delta = e.Delta / 120;
            Value += delta;
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            ShowInputTextBox();
        }

        private void ShowInputTextBox()
        {
            if (_inputTextBox != null) return; // Already showing

            _inputTextBox = new TextBox
            {
                Text = _value.ToString(),
                BackColor = Color.FromArgb(45, 45, 48),
                ForeColor = Color.Cyan,
                BorderStyle = BorderStyle.FixedSingle,
                TextAlign = HorizontalAlignment.Center,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            // Position in the center of the control
            int textBoxWidth = 60;
            int textBoxHeight = 25;
            _inputTextBox.Size = new Size(textBoxWidth, textBoxHeight);
            _inputTextBox.Location = new Point((Width - textBoxWidth) / 2, (Height - textBoxHeight) / 2);

            _inputTextBox.KeyDown += InputTextBox_KeyDown;
            _inputTextBox.LostFocus += InputTextBox_LostFocus;

            this.Controls.Add(_inputTextBox);
            _inputTextBox.Focus();
            _inputTextBox.SelectAll();
        }

        private void InputTextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                ApplyTextBoxValue();
                HideInputTextBox();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                HideInputTextBox();
            }
        }

        private void InputTextBox_LostFocus(object? sender, EventArgs e)
        {
            ApplyTextBoxValue();
            HideInputTextBox();
        }

        private void ApplyTextBoxValue()
        {
            if (_inputTextBox != null && int.TryParse(_inputTextBox.Text, out int newValue))
            {
                Value = newValue; // This will clamp to min/max automatically
            }
        }

        private void HideInputTextBox()
        {
            if (_inputTextBox != null)
            {
                _inputTextBox.KeyDown -= InputTextBox_KeyDown;
                _inputTextBox.LostFocus -= InputTextBox_LostFocus;
                this.Controls.Remove(_inputTextBox);
                _inputTextBox.Dispose();
                _inputTextBox = null;
                this.Focus();
            }
        }
    }
}
