using System.Windows.Forms;

namespace HandyBoxApp.CustomComponents.Panels.Base
{
    internal abstract class StaticPanel : CustomBasePanel
    {
        //################################################################################
        #region Constructor

        protected StaticPanel(Control parentControl, bool isVertical) : base(parentControl)
        {
            IsVertical = isVertical;
        }

        #endregion

        //################################################################################
        #region Properties

        private bool IsVertical { get; }

        #endregion
    }
}
