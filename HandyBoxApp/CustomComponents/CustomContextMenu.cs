using HandyBoxApp.Properties;
using HandyBoxApp.UserControls;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace HandyBoxApp.CustomComponents
{
    internal class CustomContextMenu : ContextMenu
    {
        private static CustomContextMenu s_SingletonInstance;

        //################################################################################
        #region Fields

        private static MainForm s_MainForm;

        private MenuItem m_Transparency;
        private MenuItem m_AlwaysOnTop;
        private MenuItem m_ShowHide;
        private MenuItem m_Settings;
        private MenuItem m_Logs;
        private MenuItem m_About;
        private MenuItem m_Exit;

        #endregion

        //################################################################################
        #region Constructor

        private CustomContextMenu()
        { }

        #endregion

        public static CustomContextMenu Instance => s_SingletonInstance ?? (s_SingletonInstance = new CustomContextMenu());

        public void SetMenuOwner(MainForm mainForm)
        {
            s_MainForm = mainForm;

            if (Settings.Default.Stocks == null)
            {
                Settings.Default.Stocks = new StringCollection();
                Settings.Default.Save();
            }

            m_ShowHide = new MenuItem("Show/Hide", ShowHide_Click);
            m_AlwaysOnTop = new MenuItem("Always on top", AlwaysOnTop_Click);
            m_Transparency = new MenuItem("Transparency", OpacityOptions());

            m_Settings = new MenuItem("Settings", new[]
            {
                new MenuItem("Stocks", GetStockPanels())
            });

            m_Logs = new MenuItem("Logs", new[]
            {
                new MenuItem("Show Logs", ShowLogs),
                new MenuItem("Clear Logs", ClearLogs)
            });

            m_About = new MenuItem("About", About);
            m_Exit = new MenuItem("Exit", Exit_Click);

            s_MainForm.VisibleChanged += MainForm_VisibleChanged;

            InitializeComponent();
        }

        //################################################################################
        #region Event Handlers

        private void TransparencyOptions_Click(object sender, EventArgs e)
        {
            if (sender is MenuItem menuItem)
            {
                double opacity = menuItem.Index / 10D;
                s_MainForm.Opacity = opacity;

                Settings.Default.Transparency = opacity;
            }

            Settings.Default.Save();

            SetOpacityCheckedStatus();
        }

        private void AlwaysOnTop_Click(object sender, EventArgs args)
        {
            s_MainForm.TopMost = !s_MainForm.TopMost;
            m_AlwaysOnTop.Checked = s_MainForm.TopMost;

            Settings.Default.OnTop = s_MainForm.TopMost;
            Settings.Default.Save();
        }

        private void ShowHide_Click(object sender, EventArgs args)
        {
            s_MainForm.Visible = !s_MainForm.Visible;
        }

        private void StockOption_Click(object sender, EventArgs args)
        {
            var menuItem = (MenuItem)sender;

            menuItem.Checked = !menuItem.Checked;

            var panel = (StockPanel)menuItem.Tag;
            panel.Visible = menuItem.Checked;

            if (menuItem.Checked)
            {
                panel.StartStockDataFetching();
                Settings.Default.Stocks.Add(panel.ToString());
            }
            else
            {
                panel.StopStockDataFetching();
                Settings.Default.Stocks.Remove(panel.ToString());
            }

            Settings.Default.Save();
        }

        private void ShowLogs(object sender, EventArgs args)
        {
            var notepadPath = @"C:\Windows\notepad.exe";
            var logPath = s_MainForm.Log.LogPath;

            Process.Start(notepadPath, logPath);
        }

        private void ClearLogs(object sender, EventArgs args)
        {
            s_MainForm.Log.ClearLogs();
        }

        private void About(object sender, EventArgs args)
        {
            //todo: create an about panel
        }

        private void Exit_Click(object sender, EventArgs args)
        {
            s_MainForm.Close();
            CustomApplicationContext.Exit();
        }

        private void MainForm_VisibleChanged(object sender, EventArgs e)
        {
            m_ShowHide.Text = s_MainForm.Visible ? @"Hide" : @"Show";
        }

        #endregion

        //################################################################################
        #region Private Members

        private void InitializeComponent()
        {
            SetOpacityCheckedStatus();
            m_AlwaysOnTop.Checked = Settings.Default.OnTop;
            m_ShowHide.Text = @"Hide";

            MenuItems.Add(m_ShowHide);
            MenuItems.Add(m_AlwaysOnTop);
            MenuItems.Add(m_Transparency);
            MenuItems.Add("-");
            MenuItems.Add(m_Settings);
            MenuItems.Add(m_Logs);
            MenuItems.Add(m_About);
            MenuItems.Add("-");
            MenuItems.Add(m_Exit);
        }

        private MenuItem[] OpacityOptions()
        {
            var options = new MenuItem[11];

            for (int i = 0; i <= 10; i++)
            {
                options[i] = new MenuItem($"%{i * 10}", TransparencyOptions_Click)
                {
                    Tag = i * 10
                };
            }

            return options;
        }

        private void SetOpacityCheckedStatus()
        {
            var opacitySetting = Settings.Default.Transparency;
            foreach (MenuItem opacityOption in m_Transparency.MenuItems)
            {
                opacityOption.Checked = Math.Abs((int)opacityOption.Tag / 100D - opacitySetting) < 0.01;
            }
        }

        private MenuItem[] GetStockPanels()
        {
            IList<MenuItem> menuList = new List<MenuItem>();

            foreach (Control panel in s_MainForm.LayoutPanel.ControlPanels)
            {
                if (panel is StockPanel stockPanel)
                {
                    var panelName = panel.Name.Split('_')[1];
                    var menuItem = new MenuItem(panelName, StockOption_Click)
                    {
                        Tag = stockPanel,
                        Checked = Settings.Default.Stocks.Contains(panelName)
                    };

                    panel.Visible = Settings.Default.Stocks.Contains(panelName);
                    if (panel.Visible)
                    {
                        stockPanel.StopStockDataFetching();
                    }

                    menuList.Add(menuItem);
                }
            }

            return menuList.ToArray();
        }

        #endregion
    }
}
