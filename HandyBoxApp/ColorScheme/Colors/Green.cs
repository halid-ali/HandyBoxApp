using System.Drawing;

namespace HandyBoxApp.ColorScheme.Colors
{
    internal sealed class Green : ColorBase
    {
        internal override ColorSet Light => new ColorSet
        {
            BackColor = Color.FromArgb(214, 255, 67),
            ForeColor = Color.FromArgb(75, 93, 10)
        };

        internal override ColorSet Normal => new ColorSet
        {
            BackColor = Color.FromArgb(150, 186, 19),
            ForeColor = Color.FromArgb(228, 255, 130)
        };

        internal override ColorSet Dark => new ColorSet
        {
            BackColor = Color.FromArgb(75, 93, 10),
            ForeColor = Color.FromArgb(214, 255, 67)
        };
    }
}
