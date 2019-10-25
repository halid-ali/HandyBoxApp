using System;
using System.Drawing;

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

        public SwitchImageButton(Action action, Bitmap image, string tooltip, bool switchStatus)
            : base(action, image, tooltip)
        {
            m_SwitchStatus = switchStatus;
        }

        #endregion
    }
}
