using HandyBoxApp.CustomComponents.Panels.Base;

using System.Drawing;
using System.Windows.Forms;

namespace HandyBoxApp.CustomComponents.Panels
{
    internal sealed class ContainerPanel : StaticPanel
    {
        //################################################################################
        #region Constructor

        public ContainerPanel(Control parentControl, Point location, bool isVertical) : base(parentControl, isVertical)
        {
            Border = new Border(Color.White, Style.FormBorder);
            InitialLocation = location;

            InitializeComponents();
        }

        #endregion

        //################################################################################
        #region Properties

        private Point InitialLocation { get; }

        #endregion

        //################################################################################
        #region Overrides

        protected override void InitializeComponents()
        {
            BackColor = Color.WhiteSmoke;
            Location = InitialLocation;
        }

        #endregion
    }
}
