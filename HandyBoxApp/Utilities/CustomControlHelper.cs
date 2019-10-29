using HandyBoxApp.CustomComponents;

using System.Drawing;
using System.Windows.Forms;
using HandyBoxApp.CustomComponents.Panels;

namespace HandyBoxApp.Utilities
{
    internal class CustomControlHelper
    {
        internal static void BoundsForVertical(Control control, Control parentControl)
        {
            if (SkipContainer(control)) return;

            parentControl.Width = 0;
            parentControl.Height = Style.FormBorder + Style.PanelSpacing;

            var x = Style.PanelSpacing;
            var y = Style.PanelSpacing;

            foreach (Control childControl in parentControl.Controls)
            {
                if (SkipContainer(childControl)) continue;

                if (childControl.Width > parentControl.Width)
                {
                    parentControl.Width = childControl.Width;
                }

                parentControl.Height += childControl.Height + Style.PanelSpacing;

                if (!control.Equals(childControl))
                {
                    y += childControl.Height + Style.PanelSpacing;
                }
            }

            parentControl.Width += Style.FormBorder * 2 + Style.PanelSpacing * 2;
            parentControl.Height += Style.FormBorder;

            x += Style.PanelSpacing;
            y += Style.PanelSpacing;

            control.Location = new Point(x, y);
        }

        internal static void BoundsForHorizontal(Control control, Control parentControl)
        {
            if (SkipContainer(control)) return;

            parentControl.Width = Style.FormBorder + Style.PanelSpacing;
            parentControl.Height = 0;

            var x = Style.PanelSpacing;
            var y = Style.PanelSpacing;

            foreach (Control childControl in parentControl.Controls)
            {
                if (SkipContainer(childControl)) continue;

                if (childControl.Height > parentControl.Height)
                {
                    parentControl.Height = childControl.Height;
                }

                parentControl.Width += childControl.Width + Style.PanelSpacing;

                if (!control.Equals(childControl))
                {
                    x += childControl.Width + Style.PanelSpacing;
                }
            }

            parentControl.Width += Style.FormBorder;
            parentControl.Height += Style.FormBorder * 2 + Style.PanelSpacing * 2;

            x += Style.PanelSpacing;
            y += Style.PanelSpacing;

            control.Location = new Point(x, y);
        }

        private static bool SkipContainer(Control control)
        {
            return control is ContainerPanel;
        }
    }
}
