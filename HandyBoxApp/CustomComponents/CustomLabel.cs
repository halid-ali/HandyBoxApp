using HandyBoxApp.Utilities;

using System;
using System.Drawing;
using System.Windows.Forms;

namespace HandyBoxApp.CustomComponents
{
    internal class CustomLabel : Label
    {
        public CustomLabel(Control parent, Tuple<Color, Color> colors, string text)
        {
            ParentControl = parent;
            Colors = colors;
            TextValue = text;

            InitializeComponent();
        }

        private Control ParentControl { get; set; }

        private Tuple<Color, Color> Colors { get; }

        private string TextValue { get; set; }

        private void InitializeComponent()
        {
            AutoSize = true;
            Text = TextValue;

            Padding = new Padding(Style.PanelPadding);
            Location = CustomControlHelper.SetLocation(ParentControl);

            TextAlign = ContentAlignment.MiddleCenter;
            Font = new Font(new FontFamily("Consolas"), Style.PanelFontSize, FontStyle.Bold);

            BackColor = Colors.Item1;
            ForeColor = Colors.Item2;
        }
    }
}
