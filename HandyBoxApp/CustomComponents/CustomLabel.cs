using HandyBoxApp.ColorScheme;
using HandyBoxApp.ColorScheme.Colors;
using HandyBoxApp.Utilities;

using System.Drawing;
using System.Windows.Forms;

namespace HandyBoxApp.CustomComponents
{
    internal class CustomLabel : Label
    {
        public CustomLabel(Control parent, string text)
        {
            ParentControl = parent;
            TextValue = text;

            InitializeComponent();
        }

        private Control ParentControl { get; set; }

        private string TextValue { get; set; }

        private void InitializeComponent()
        {
            AutoSize = true;
            Text = TextValue;

            Padding = new Padding(Style.PanelPadding);
            Location = CustomControlHelper.SetLocation(ParentControl);

            TextAlign = ContentAlignment.MiddleCenter;
            Font = new Font(new FontFamily("Consolas"), Style.PanelFontSize, FontStyle.Bold);

            Painter<Blue>.Paint(this, PaintMode.Normal);
        }
    }
}
