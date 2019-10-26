using System.Drawing;

namespace HandyBoxApp.ColorScheme.Colors
{
    internal sealed class Black : ColorBase
    {
        internal override ColorSet Light => new ColorSet
        {
            BackColor = Color.FromArgb(255, 255, 255),
            ForeColor = Color.FromArgb(0, 0, 0)
        };

        internal override ColorSet Normal => new ColorSet
        {
            BackColor = Color.FromArgb(32, 35, 36),
            ForeColor = Color.FromArgb(161, 163, 164)
        };

        internal override ColorSet Dark => new ColorSet
        {
            BackColor = Color.FromArgb(0, 0, 0),
            ForeColor = Color.FromArgb(255, 255, 255)
        };
    }
}
