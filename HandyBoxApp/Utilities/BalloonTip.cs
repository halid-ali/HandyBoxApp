using System.Windows.Forms;

namespace HandyBoxApp.Utilities
{
    internal sealed class BalloonTip
    {
        //################################################################################
        #region Constructor

        public BalloonTip(NotifyIcon notifyIcon)
        {
            NotifyIcon = notifyIcon;
        }

        #endregion

        //################################################################################
        #region Properties

        private static NotifyIcon NotifyIcon { get; set; }

        #endregion

        //################################################################################
        #region Static Members

        internal static void Show(string title, string text, ToolTipIcon icon, int timeout)
        {
            NotifyIcon.BalloonTipIcon = icon;
            NotifyIcon.BalloonTipTitle = title;
            NotifyIcon.BalloonTipText = text;

            NotifyIcon.ShowBalloonTip(timeout);
        }

        #endregion
    }
}
