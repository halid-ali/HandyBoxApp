using System.Drawing;

namespace HandyBoxApp.ColorScheme.Colors
{
    internal sealed class Red : ColorBase
    {
        internal override ColorSet Light => new ColorSet
        {
            BackColor = Color.FromArgb(214, 109, 122),
            ForeColor = Color.FromArgb(105, 15, 26)
        };

        internal override ColorSet Normal => new ColorSet
        {
            BackColor = Color.FromArgb(196, 46, 65),
            ForeColor = Color.FromArgb(53, 8, 13)
        };

        internal override ColorSet Dark => new ColorSet
        {
            BackColor = Color.FromArgb(105, 15, 26),
            ForeColor = Color.FromArgb(214, 109, 122)
        };
    }
}
