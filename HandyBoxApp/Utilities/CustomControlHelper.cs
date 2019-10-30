using HandyBoxApp.CustomComponents;
using HandyBoxApp.CustomComponents.Panels;

using System.Drawing;
using System.Windows.Forms;

namespace HandyBoxApp.Utilities
{
    internal class CustomControlHelper
    {
        //################################################################################
        #region Internal Members

        internal static void VerticalAlign(Control parentControl)
        {
            parentControl.Width = 0;
            parentControl.Height = Style.FormBorder + Style.PanelSpacing;

            var x = Style.FormBorder + Style.PanelSpacing;
            var y = Style.FormBorder + Style.PanelSpacing;

            foreach (Control childControl in parentControl.Controls)
            {
                if (SkipContainer(childControl)) continue;

                if (childControl.Width > parentControl.Width)
                {
                    parentControl.Width = childControl.Width;
                }

                parentControl.Height += childControl.Height + Style.PanelSpacing;
                childControl.Location = new Point(x, y);

                y += childControl.Height + Style.PanelSpacing;
            }

            parentControl.Width += Style.FormBorder * 2 + Style.PanelSpacing * 2;
            parentControl.Height += Style.FormBorder;
        }

        internal static void HorizontalAlign(Control parentControl)
        {
            parentControl.Width = Style.FormBorder + Style.PanelSpacing;
            parentControl.Height = 0;

            var x = Style.FormBorder + Style.PanelSpacing;
            var y = Style.FormBorder + Style.PanelSpacing;

            foreach (Control childControl in parentControl.Controls)
            {
                if (SkipContainer(childControl)) continue;

                if (childControl.Height > parentControl.Height)
                {
                    parentControl.Height = childControl.Height;
                }

                parentControl.Width += childControl.Width + Style.PanelSpacing;
                childControl.Location = new Point(x, y);

                x += childControl.Width + Style.PanelSpacing;
            }

            parentControl.Width += Style.FormBorder;
            parentControl.Height += Style.FormBorder * 2 + Style.PanelSpacing * 2;
        }

        #endregion

        //################################################################################
        #region Private Members

        private static bool SkipContainer(Control control)
        {
            return control is ContainerPanel;
        }

        #endregion
    }
}
