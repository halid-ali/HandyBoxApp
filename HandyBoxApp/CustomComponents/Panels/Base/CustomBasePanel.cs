using HandyBoxApp.Utilities;

using System.Drawing;
using System.Windows.Forms;

namespace HandyBoxApp.CustomComponents.Panels.Base
{
    internal abstract class CustomBasePanel : Panel
    {
        //################################################################################
        #region Constructor

        protected CustomBasePanel(Control parentControl, bool isVertical)
        {
            ParentControl = parentControl;
            IsVertical = isVertical;

            InitializeComponent();
        }

        #endregion

        //################################################################################
        #region Properties

        protected Control ParentControl { get; }

        private bool IsVertical { get; }

        #endregion

        //################################################################################
        #region Abstract Methods

        protected abstract void InitializeComponents();

        #endregion

        //################################################################################
        #region Protected Implementation

        protected Border Border { get; set; }

        protected void ShowBalloonTip(string title, string message, ToolTipIcon toolTipIcon)
        {
            ToolTipIcon icon = toolTipIcon;
            int timeout = 2000;

            BalloonTip.Show(title, message, icon, timeout);
        }

        #endregion

        //################################################################################
        #region Private Members

        private void InitializeComponent()
        {
            Visible = true;
            BackColor = Color.FromArgb(121, 121, 121);
            Border = new Border(Color.White, 1); //default border style

            ControlAdded += OnControlAdded;
            Paint += PaintBorder;
        }

        private void OnControlAdded(object sender, ControlEventArgs e)
        {
            if (IsVertical)
                CustomControlHelper.BoundsForVertical(e.Control, this);
            else
                CustomControlHelper.BoundsForHorizontal(e.Control, this);
        }

        private void PaintBorder(object sender, PaintEventArgs e)
        {
            var point = new Point(0, 0);
            var size = new Size(Width - Style.FormBorder, Height - Style.FormBorder);
            Rectangle borderRectangle = new Rectangle(point, size);
            CreateGraphics().DrawRectangle(new Pen(Border.Color, Border.Size), borderRectangle);
        }

        #endregion
    }
}
