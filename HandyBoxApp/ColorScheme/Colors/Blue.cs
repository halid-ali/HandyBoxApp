using System.Drawing;

namespace HandyBoxApp.ColorScheme.Colors
{
    internal sealed class Blue : ColorBase
    {
        internal override ColorSet Light => new ColorSet
        {
            BackColor = Color.FromArgb(129, 195, 215),
            ForeColor = Color.FromArgb(22, 66, 91)
        };

        internal override ColorSet Normal => new ColorSet
        {
            BackColor = Color.FromArgb(58, 124, 165),
            ForeColor = Color.FromArgb(10, 30, 42)
        };

        internal override ColorSet Dark => new ColorSet
        {
            BackColor = Color.FromArgb(22, 66, 91),
            ForeColor = Color.FromArgb(129, 195, 215)
        };
    }
}
