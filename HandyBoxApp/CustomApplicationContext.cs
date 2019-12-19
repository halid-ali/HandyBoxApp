using HandyBoxApp.CustomComponents;
using HandyBoxApp.Properties;
using HandyBoxApp.UserControls;
using HandyBoxApp.Utilities;

using System;
using System.Drawing;
using System.Windows.Forms;

namespace HandyBoxApp
{
    public class CustomApplicationContext : ApplicationContext
    {
        //################################################################################
        #region Fields

        private static NotifyIcon s_NotifyIcon;
        private BalloonTip m_BalloonTip;

        #endregion

        //################################################################################
        #region Constructor

        public CustomApplicationContext(MainForm mainForm)
        {
            MainForm = mainForm;
            var customContextMenu = CustomContextMenu.Instance;
            customContextMenu.SetMenuOwner(mainForm);

#if DEBUG
            Bitmap icon = Resources.Debug_Logo;
#else
            Bitmap icon = Resources.Logo;
#endif

            s_NotifyIcon = new NotifyIcon
            {
                //todo: badge icon for notify icon if anything occured eg. crash, update etc.
                //todo: also show balloon tips if any update or crash occured
                Icon = Icon.FromHandle(icon.GetHicon()),
                ContextMenu = customContextMenu,
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
            var currencyInfos = string.Empty;
            var workHourInfo = string.Empty;

            foreach (Control panel in ((MainForm)MainForm).LayoutPanel.ControlPanels)
            {
                if (panel is StockPanel stockPanel)
                {
                    if (stockPanel.GetStockInformation().Contains("EUR/TRY"))
                    {
                        currencyInfos += $"{stockPanel.GetStockInformation()}\n"; 
                    }
                }

                if (panel is TimerPanel timerPanel)
                {
                    workHourInfo = $"{timerPanel}";
                }
            }

            s_NotifyIcon.Text = $@"{currencyInfos}{workHourInfo}";
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            if (((MouseEventArgs)e).Button == MouseButtons.Left)
            {
                MainForm.Visible = !MainForm.Visible;
            }
        }

        #endregion
    }
}
