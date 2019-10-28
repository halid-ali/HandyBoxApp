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
            Border = new Border(Color.White, 1); //default border style
            BackColor = Color.FromArgb(91, 91, 91);
        }

        #endregion

        //################################################################################
        #region Properties

        protected Control ParentControl { get; }

        #endregion

        //################################################################################
        #region Abstract Methods

        protected abstract void InitializeComponents();

        #endregion

        //################################################################################
        #region Protected Implementation

        protected Border Border { get; set; }

        protected Size GetPanelDimensions()
        {
            var width = Style.PanelMargin * 2 + Border.Size * 2;
            var height = 0;

            foreach (Control control in Controls)
            {
                width += control.Width;

                if (height < control.Height)
                {
                    height = control.Height;
                }
            }

            height += Style.PanelMargin * 2;

            return new Size(width, height);
        }

        protected void PaintBorder(object sender, PaintEventArgs e)
        {
            Rectangle borderRectangle = new Rectangle(new Point(0, 0), new Size(Width - 1, Height - 1));
            CreateGraphics().DrawRectangle(new Pen(Border.Color, Border.Size), borderRectangle);
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
