using HandyBoxApp.ColorScheme;
using HandyBoxApp.ColorScheme.Colors;
using HandyBoxApp.CustomComponents;

using System.Drawing;
using System.Windows.Forms;

namespace HandyBoxApp.UserControls
{
    public class TitlePanel : UserControl
    {
        //################################################################################
        #region Fields

        private Label logoLabel;
        private Label titleLabel;
        private Label closeLabel;
        private FlowLayoutPanel containerPanel;

        #endregion

        //################################################################################
        #region Constructor

        public TitlePanel()
        {
            InitializeComponent();
            ReorderComponents();
        }

        #endregion

        //################################################################################
        #region Private Members

        private void InitializeComponent()
        {
            containerPanel = new FlowLayoutPanel();
            logoLabel = new Label();
            titleLabel = new Label();
            closeLabel = new Label();

            containerPanel.SuspendLayout();
            SuspendLayout();

            // 
            // ContainerPanel
            // 
            containerPanel.Margin = new Padding(0);
            containerPanel.Padding = new Padding(0);
            containerPanel.Location = new Point(0, 0);
            containerPanel.FlowDirection = FlowDirection.LeftToRight;

            containerPanel.Controls.Add(logoLabel);
            containerPanel.Controls.Add(titleLabel);
            containerPanel.Controls.Add(closeLabel);

            // 
            // LogoLabel
            // 
            logoLabel.Name = "LogoLabel";
            logoLabel.Text = "L";
            logoLabel.AutoSize = true;
            logoLabel.Margin = new Padding(0, 0, Style.PanelSpacing, 0);
            logoLabel.Padding = new Padding(Style.PanelPadding);
            logoLabel.TextAlign = ContentAlignment.MiddleCenter;
            logoLabel.Font = new Font(new FontFamily(Style.FontName), Style.PanelFontSize, FontStyle.Bold);
            Painter<Green>.Paint(logoLabel, PaintMode.Light);

            // 
            // TitleLabel
            // 
            titleLabel.Name = "TitleLabel";
            titleLabel.Text = "Handy Box App";
            titleLabel.AutoSize = true;
            titleLabel.Margin = new Padding(0, 0, Style.PanelSpacing, 0);
            titleLabel.Padding = new Padding(Style.PanelPadding);
            titleLabel.TextAlign = ContentAlignment.MiddleLeft;
            titleLabel.Font = new Font(new FontFamily(Style.FontName), Style.PanelFontSize, FontStyle.Bold);
            Painter<Black>.Paint(titleLabel, PaintMode.Dark);

            // 
            // CloseLabel
            // 
            closeLabel.Name = "CloseLabel";
            closeLabel.Text = "X";
            closeLabel.AutoSize = true;
            closeLabel.Margin = new Padding(0, 0, 0, 0);
            closeLabel.Padding = new Padding(Style.PanelPadding);
            closeLabel.TextAlign = ContentAlignment.MiddleCenter;
            closeLabel.Font = new Font(new FontFamily(Style.FontName), Style.PanelFontSize, FontStyle.Bold);
            Painter<Red>.Paint(closeLabel, PaintMode.Dark);

            // 
            // TitlePanel
            // 
            Name = "TitlePanel";
            AutoSize = true;
            Margin = new Padding(0, 0, 0, Style.PanelSpacing);
            BackColor = Color.FromArgb(100, 100, 100);
            BorderStyle = BorderStyle.FixedSingle;
            AutoScaleMode = AutoScaleMode.Font;

            Controls.Add(containerPanel);

            containerPanel.ResumeLayout(false);
            containerPanel.PerformLayout();
            ResumeLayout(false);
        }

        private void ReorderComponents()
        {
            containerPanel.Width = 0;
            containerPanel.Height = 0;

            foreach (Control control in containerPanel.Controls)
            {
                if (control.PreferredSize.Height > containerPanel.Height)
                {
                    containerPanel.Height = control.PreferredSize.Height;
                }

                containerPanel.Width += control.PreferredSize.Width;
            }

            containerPanel.Width += containerPanel.Controls.Count - 1;
            Height = containerPanel.Height;
            Width = containerPanel.Width;
        }

        #endregion
    }
}
