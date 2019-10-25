using HandyBoxApp.CustomComponents;
using HandyBoxApp.Properties;
using HandyBoxApp.Utilities;

using System.Drawing;
using System.Windows.Forms;

namespace HandyBoxApp
{
    public class CustomApplicationContext : ApplicationContext
    {
        //################################################################################
        #region Fields

        private static NotifyIcon s_NotifyIcon;
        private CustomContextMenu m_CustomContextMenu;
        private BalloonTip m_BalloonTip;

        #endregion

        //################################################################################
        #region Constructor

        public CustomApplicationContext(MainForm mainForm)
        {
            MainForm = mainForm;
            m_CustomContextMenu = new CustomContextMenu(mainForm);

#if DEBUG
            Bitmap icon = Resource.Debug_Logo;
#else
            Bitmap icon = Resource.Logo;
#endif

            s_NotifyIcon = new NotifyIcon()
            {
                //todo: badge icon for notify icon if anything occured eg. crash, update etc.
                //todo: also show balloon tips if any update or crash occured
                Icon = Icon.FromHandle(icon.GetHicon()),
                ContextMenu = m_CustomContextMenu,
                Visible = true
            };

            s_NotifyIcon.DoubleClick += NotifyIcon_DoubleClick;
            s_NotifyIcon.MouseMove += NotifyIcon_MouseMouse;

            m_BalloonTip = new BalloonTip(s_NotifyIcon);
        }

        #endregion

        //################################################################################
        #region Internal Members

        internal static void Exit()
        {
            s_NotifyIcon.Visible = false;
            Application.Exit();
        }

        #endregion

        //################################################################################
        #region Event Handlers

        private void NotifyIcon_MouseMouse(object sender, MouseEventArgs e)
        {
            //todo: show currency and working hour info
            s_NotifyIcon.Text = "This is a notify icon.";
        }

        private void NotifyIcon_DoubleClick(object sender, System.EventArgs e)
        {
            Exit();
        }

        #endregion
    }
}
