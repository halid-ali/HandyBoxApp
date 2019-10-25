using HandyBoxApp.EventArgs;
using HandyBoxApp.Utilities;

using System.Drawing;
using System.Windows.Forms;

namespace HandyBoxApp.CustomComponents.Panels.Base
{
    internal abstract class CustomBasePanel : Panel
    {
        //################################################################################
        #region Constructor

        protected CustomBasePanel(Control parentControl)
        {
            ParentControl = parentControl;
        }

        #endregion

        //################################################################################
        #region Properties

        protected Control ParentControl { get; }

        #endregion

        //################################################################################
        #region Abstract Methods

        protected abstract void InitializeComponents();

        protected abstract void PaintBorder(object sender, PaintEventArgs e);

        #endregion

        //################################################################################
        #region Protected Implementation

        protected Size GetPanelDimensions()
        {
            var width = 0;
            var height = 0;

            foreach (Control control in Controls)
            {
                width += control.Width;
                height += control.Height;
            }

            return new Size(width, height);
        }

        protected void PaintPanelBorder(object sender, BorderEventArgs e)
        {
            Rectangle borderRectangle = new Rectangle(new Point(0, 0), new Size(Width - 1, Height - 1));
            CreateGraphics().DrawRectangle(new Pen(e.Color, e.Size), borderRectangle);
        }

        protected void ShowBalloonTip(string title, string message, ToolTipIcon toolTipIcon)
        {
            ToolTipIcon icon = toolTipIcon;
            int timeout = 2000;

            BalloonTip.Show(title, message, icon, timeout);
        }

        #endregion
    }


}
