using HandyBoxApp.CustomComponents.Panels.Base;

using System.Drawing;
using System.Windows.Forms;

namespace HandyBoxApp.CustomComponents.Panels
{
    internal sealed class ContainerPanel : StaticPanel
    {
        //################################################################################
        #region Constructor

        public ContainerPanel(Control parentControl, bool isVertical) : base(parentControl, isVertical)
        {
            Border = new Border(Color.Black, Style.FormBorder);

            InitializeComponents();
        }

        #endregion

        //################################################################################
        #region Overrides

        protected override void InitializeComponents()
        {
            Visible = true;
            AutoSize = false;
            BackColor = Color.White;
            Width = ParentControl.Width;
            Height = ParentControl.Height;
        }

        #endregion
    }
}
