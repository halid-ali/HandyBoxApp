using HandyBoxApp.CustomComponents;

using System.Drawing;
using System.Windows.Forms;

namespace HandyBoxApp.UserControls
{
    public class LayoutPanel : UserControl
    {
        //################################################################################
        #region Constructor

        public LayoutPanel()
        {
            InitializeComponent();
        }

        #endregion

        //################################################################################
        #region Properties

        private FlowLayoutPanel ContainerPanel { get; } = new FlowLayoutPanel();

        #endregion

        //################################################################################
        #region Internal Members

        internal void Add(Control control)
        {
            ContainerPanel.Controls.Add(control);
        }

        #endregion

        //################################################################################
        #region Private Members

        private void InitializeComponent()
        {
            SuspendLayout();

            #region Container Panel

            ContainerPanel.Name = "ContainerPanel";
            ContainerPanel.BackColor = Color.White;
            ContainerPanel.BorderStyle = BorderStyle.FixedSingle;
            ContainerPanel.FlowDirection = FlowDirection.TopDown;
            ContainerPanel.Location = new Point(0, 0);
            ContainerPanel.Padding = new Padding(Style.PanelSpacing, Style.PanelSpacing, Style.PanelSpacing, 0);
            ContainerPanel.TabIndex = 0;
            ContainerPanel.ControlAdded += ContainerPanel_ControlAdded;

            #endregion

            #region LayoutPanel

            Name = "LayoutPanel";
            AutoScaleMode = AutoScaleMode.Font;

            Controls.Add(ContainerPanel);

            #endregion

            ResumeLayout(false);
        }

        private void UpdateContainerPanel()
        {
            ContainerPanel.Width = 0;
            ContainerPanel.Height = Style.PanelSpacing + Style.FormBorder;

            foreach (Control control in ContainerPanel.Controls)
            {
                if (control.Width > ContainerPanel.Width)
                {
                    ContainerPanel.Width = control.Width;
                }

                ContainerPanel.Height += control.Height + Style.PanelSpacing;
            }

            ContainerPanel.Width += (Style.PanelSpacing + Style.FormBorder) * 2;
            ContainerPanel.Height += Style.FormBorder;

            Width = ContainerPanel.Width;
            Height = ContainerPanel.Height;
        }

        #endregion

        //################################################################################
        #region Event Handlers

        private void ContainerPanel_ControlAdded(object sender, ControlEventArgs e)
        {
            UpdateContainerPanel();
        }

        #endregion
    }
}
