using HandyBoxApp.CustomComponents;

using System.Drawing;
using System.Windows.Forms;

namespace HandyBoxApp.Utilities
{
    internal class CustomControlHelper
    {
        internal static void SetHorizontalLocation(Control parentControl)
        {
            var x = Style.PanelMargin;
            var y = Style.PanelMargin;

            foreach (Control control in parentControl.Controls)
            {
                control.Location = new Point(x, y);

                x += control.Width + Style.PanelSpacing;
                y = control.Location.Y;
            }
        }

        internal static Point SetVerticalLocation(Control parentControl)
        {
            var x = Style.PanelMargin;
            var y = Style.PanelMargin;

            foreach (Control control in parentControl.Controls)
            {
                x = control.Location.X;
                y += control.Height + Style.PanelSpacing;
            }

            return new Point(x, y);
        }
    }
}
