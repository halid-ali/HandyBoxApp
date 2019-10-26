using HandyBoxApp.CustomComponents;

using System.Drawing;
using System.Windows.Forms;

namespace HandyBoxApp.Utilities
{
    internal class CustomControlHelper
    {
        internal static Point SetLocation(Control parentControl)
        {
            var x = 0;
            var y = 0;

            foreach (Control control in parentControl.Controls)
            {
                x += control.Width + Style.PanelSpacing;
                y = control.Location.Y;
            }

            return new Point(x, y);
        }
    }
}
