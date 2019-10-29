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

        public SwitchImageButton(Control parentControl, Action<ClickImageButton> action, string label, bool switchStatus)
            : base(parentControl, action, label)
        {
            m_SwitchStatus = switchStatus;
        }

        #endregion
    }
}
