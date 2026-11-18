using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AshClicker
{
    internal sealed class IconToolTips : Component
    {
        private readonly ToolTip _toolTip;
        private readonly Font _iconFont;
        private readonly bool _hasIconFont;

        public IconToolTips(IContainer container)
        {
            container.Add(this);
            _toolTip = new ToolTip
            {
                InitialDelay = 350,
                ReshowDelay = 100,
                AutoPopDelay = 7000,
                UseAnimation = false,
                UseFading = false
            };
            const string fontName = "Segoe MDL2 Assets";
            var iconFont = new Font(fontName, 13F);
            _hasIconFont = iconFont.Name == fontName;
            if (_hasIconFont)
            {
                _iconFont = iconFont;
            }
            else
            {
                iconFont.Dispose();
                _iconFont = new Font("Segoe UI Symbol", 13F);
            }
        }

        public void SetIcon(ButtonBase control, string glyph, string fallback, string name, string description = null)
        {
            control.Font = _iconFont;
            control.Text = _hasIconFont ? glyph : fallback;
            control.UseMnemonic = false;
            Describe(control, name, description);
        }

        public void Describe(Control control, string name, string description = null)
        {
            control.AccessibleName = name;
            control.AccessibleDescription = description;
            _toolTip.SetToolTip(control, description == null ? name : name + "\r\n" + description);
        }

        public void SetHelp(Button button, string name, string description)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.ForeColor = Color.FromArgb(78, 93, 172);
            SetIcon(button, "\uE946", "ⓘ", name, description);
            button.Click += (sender, args) =>
                _toolTip.Show(_toolTip.GetToolTip(button), button, button.Width / 2, button.Height, 7000);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _toolTip.Dispose();
                _iconFont.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
