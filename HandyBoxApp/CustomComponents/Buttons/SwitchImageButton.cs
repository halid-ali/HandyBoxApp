using System;

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

        public SwitchImageButton(Action<ClickImageButton> action, string labelText, bool switchStatus)
            : base(action, labelText)
        {
            m_SwitchStatus = switchStatus;
        }

        #endregion
    }
}
