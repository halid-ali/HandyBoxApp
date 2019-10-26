using System;
using System.Windows.Forms;

namespace HandyBoxApp.CustomComponents.Buttons
{
    internal sealed class SwitchImageButton : ClickImageButton
    {
        //################################################################################
        #region Fields

        private bool m_SwitchStatus;

        #endregion

        //################################################################################
        #region Constructor

        public SwitchImageButton(Control parentControl, Action action, string tooltip, bool switchStatus)
            : base(parentControl, action, tooltip)
        {
            m_SwitchStatus = switchStatus;
        }

        #endregion
    }
}
