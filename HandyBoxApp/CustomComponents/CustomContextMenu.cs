using HandyBoxApp.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HandyBoxApp.CustomComponents
{
    internal class CustomContextMenu : ContextMenu
    {
        //################################################################################
        #region Fields

        private readonly MainForm m_MainForm;

        private readonly MenuItem m_Transparency;
        private readonly MenuItem m_AlwaysOnTop;
        private readonly MenuItem m_ShowHide;
        private readonly MenuItem m_Exit;

        #endregion

        //################################################################################
        #region Constructor

        public CustomContextMenu(MainForm mainForm)
        {
            m_MainForm = mainForm;

            m_ShowHide = new MenuItem("Show/Hide", ShowHide_Click);
            m_AlwaysOnTop = new MenuItem("Always on top", AlwaysOnTop_Click);
            m_Transparency = new MenuItem("Transparency", OpacityOptions());
            m_Exit = new MenuItem("Exit", Exit_Click);

            m_MainForm.VisibleChanged += MainForm_VisibleChanged;

            InitializeComponent();
        }

        #endregion

        //################################################################################
        #region Event Handlers

        private void TransparencyOptions_Click(object sender, System.EventArgs e)
        {
            if (sender is MenuItem menuItem)
            {
                double opacity = menuItem.Index / 10D;
                m_MainForm.Opacity = opacity;

                Settings.Default.Transparency = opacity;
            }

            Settings.Default.Save();

            SetOpacityCheckedStatus();
        }

        private void AlwaysOnTop_Click(object sender, System.EventArgs args)
        {
            m_MainForm.TopMost = !m_MainForm.TopMost;
            m_AlwaysOnTop.Checked = m_MainForm.TopMost;

            Settings.Default.OnTop = m_MainForm.TopMost;
            Settings.Default.Save();
        }

        private void ShowHide_Click(object sender, System.EventArgs args)
        {
            m_MainForm.Visible = !m_MainForm.Visible;
        }

        private void Exit_Click(object sender, System.EventArgs args)
        {
            CustomApplicationContext.Exit();
        }

        private void MainForm_VisibleChanged(object sender, System.EventArgs e)
        {
            m_ShowHide.Text = m_MainForm.Visible ? @"Hide" : @"Show";
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
            MenuItems.Add(m_Exit);
        }

        private MenuItem[] OpacityOptions()
        {
            var options = new MenuItem[11];

            for (int i = 0; i <= 10; i++)
            {
                options[i] = new MenuItem($"%{i * 10}", TransparencyOptions_Click);
                options[i].Tag = i * 10;
            }

            return options;
        }

        private void SetOpacityCheckedStatus()
        {
            var opacitySetting = Settings.Default.Transparency;
            foreach (MenuItem opacityOption in m_Transparency.MenuItems)
            {
                opacityOption.Checked = false;

                if ((int)opacityOption.Tag / 100D == opacitySetting)
                {
                    opacityOption.Checked = true;
                }
            }
        }

        #endregion
    }
}
