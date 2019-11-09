using HandyBoxApp.CustomComponents;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace HandyBoxApp.UserControls
{
    public class LayoutPanel : UserControl
    {
        //################################################################################
        #region Fields

        private FlowLayoutPanel containerPanel;

        #endregion

        //################################################################################
        #region Constructor

        public LayoutPanel()
        {
            InitializeComponent();
        }

        #endregion

        internal void Add(Control control)
        {
            containerPanel.Controls.Add(control);
        }

        //################################################################################
        #region Private Members

        private void InitializeComponent()
        {
            containerPanel = new FlowLayoutPanel();
            SuspendLayout();

            // 
            // containerPanel
            // 
            containerPanel.Name = "containerPanel";
            containerPanel.BackColor = Color.White;
            containerPanel.BorderStyle = BorderStyle.FixedSingle;
            containerPanel.FlowDirection = FlowDirection.TopDown;
            containerPanel.Location = new Point(0, 0);
            containerPanel.Padding = new Padding(Style.PanelSpacing, Style.PanelSpacing, Style.PanelSpacing, 0);
            containerPanel.TabIndex = 0;
            containerPanel.ControlAdded += ContainerPanel_ControlAdded;

            // 
            // LayoutPanel
            // 
            Name = "LayoutPanel";
            AutoScaleMode = AutoScaleMode.Font;

            Controls.Add(containerPanel);

            ResumeLayout(false);
        }

        private void ContainerPanel_ControlAdded(object sender, ControlEventArgs e)
        {
            UpdateContainerPanel();
        }

        private void UpdateContainerPanel()
        {
            containerPanel.Width = 0;
            containerPanel.Height = Style.PanelSpacing + Style.FormBorder;

            foreach (Control control in containerPanel.Controls)
            {
                if (control.Width > containerPanel.Width)
                {
                    containerPanel.Width = control.Width;
                }

                containerPanel.Height += control.Height + Style.PanelSpacing;
            }

            containerPanel.Width += (Style.PanelSpacing + Style.FormBorder) * 2;
            containerPanel.Height += Style.FormBorder;

            Width = containerPanel.Width;
            Height = containerPanel.Height;
        }

        #endregion
    }
}
