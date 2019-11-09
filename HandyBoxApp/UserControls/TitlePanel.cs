using HandyBoxApp.ColorScheme;
using HandyBoxApp.ColorScheme.Colors;
using HandyBoxApp.CustomComponents;

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HandyBoxApp.UserControls
{
    public class TitlePanel : UserControl
    {
        //################################################################################
        #region Constants

        private const int c_WmNclButtonDown = 0xA1;
        private const int c_HtCaption = 0x2;

        #endregion

        //################################################################################
        #region DLL Imports

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        #endregion

        //################################################################################
        #region Constructor

        public TitlePanel(Control parentControl)
        {
            ParentControl = parentControl;

            InitializeComponent();
            ReorderComponents();
        }

        #endregion

        //################################################################################
        #region Properties

        private Control ParentControl { get; }

        private Label LogoLabel { get; } = new Label();

        private Label TitleLabel { get; } = new Label();

        private Label CloseLabel { get; } = new Label();

        private FlowLayoutPanel ContainerPanel { get; } = new FlowLayoutPanel();

        #endregion

        //################################################################################
        #region Private Members

        private void InitializeComponent()
        {
            ContainerPanel.SuspendLayout();
            SuspendLayout();

            #region Container Panel

            ContainerPanel.Margin = new Padding(0);
            ContainerPanel.Padding = new Padding(0);
            ContainerPanel.Location = new Point(0, 0);
            ContainerPanel.FlowDirection = FlowDirection.LeftToRight;

            ContainerPanel.Controls.Add(LogoLabel);
            ContainerPanel.Controls.Add(TitleLabel);
            ContainerPanel.Controls.Add(CloseLabel);

            #endregion

            #region Logo Label

            LogoLabel.Name = "LogoLabel";
            LogoLabel.Text = "L";
            LogoLabel.AutoSize = true;
            LogoLabel.Margin = new Padding(0, 0, Style.PanelSpacing, 0);
            LogoLabel.Padding = new Padding(Style.PanelPadding);
            LogoLabel.TextAlign = ContentAlignment.MiddleCenter;
            LogoLabel.Font = new Font(new FontFamily(Style.FontName), Style.PanelFontSize, FontStyle.Bold);
            Painter<Green>.Paint(LogoLabel, PaintMode.Light);
            LogoLabel.DoubleClick += LogoLabel_DoubleClick;

            #endregion

            #region Title Label

            TitleLabel.Name = "TitleLabel";
            TitleLabel.Text = "Handy Box App";
            TitleLabel.AutoSize = true;
            TitleLabel.Margin = new Padding(0, 0, Style.PanelSpacing, 0);
            TitleLabel.Padding = new Padding(Style.PanelPadding);
            TitleLabel.TextAlign = ContentAlignment.MiddleLeft;
            TitleLabel.Font = new Font(new FontFamily(Style.FontName), Style.PanelFontSize, FontStyle.Bold);
            Painter<Black>.Paint(TitleLabel, PaintMode.Dark);
            TitleLabel.MouseDown += DragAndDrop;

            #endregion

            #region Close Label

            CloseLabel.Name = "CloseLabel";
            CloseLabel.Text = "X";
            CloseLabel.AutoSize = true;
            CloseLabel.Margin = new Padding(0, 0, 0, 0);
            CloseLabel.Padding = new Padding(Style.PanelPadding);
            CloseLabel.TextAlign = ContentAlignment.MiddleCenter;
            CloseLabel.Font = new Font(new FontFamily(Style.FontName), Style.PanelFontSize, FontStyle.Bold);
            Painter<Red>.Paint(CloseLabel, PaintMode.Dark);
            CloseLabel.Click += CloseLabel_Click;

            #endregion

            #region Title Panel

            Name = "TitlePanel";
            AutoSize = true;
            Margin = new Padding(0, 0, 0, Style.PanelSpacing);
            BackColor = Color.FromArgb(100, 100, 100);
            BorderStyle = BorderStyle.FixedSingle;
            AutoScaleMode = AutoScaleMode.Font;

            Controls.Add(ContainerPanel);

            #endregion

            ContainerPanel.ResumeLayout(false);
            ContainerPanel.PerformLayout();
            ResumeLayout(false);
        }

        private void ReorderComponents()
        {
            ContainerPanel.Width = 0;
            ContainerPanel.Height = 0;

            foreach (Control control in ContainerPanel.Controls)
            {
                if (control.PreferredSize.Height > ContainerPanel.Height)
                {
                    ContainerPanel.Height = control.PreferredSize.Height;
                }

                ContainerPanel.Width += control.PreferredSize.Width;
            }

            ContainerPanel.Width += ContainerPanel.Controls.Count - 1;
            Height = ContainerPanel.Height;
            Width = ContainerPanel.Width;
        }

        #endregion

        //################################################################################
        #region Event Handlers

        private void DragAndDrop(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(ParentControl.Handle, c_WmNclButtonDown, c_HtCaption, 0);
            }
        }

        private void LogoLabel_DoubleClick(object sender, System.EventArgs e)
        {
            ParentControl.Visible = false;
        }

        private void CloseLabel_Click(object sender, System.EventArgs e)
        {
            ((MainForm)ParentControl).Close();
            CustomApplicationContext.Exit();
        }

        #endregion
    }
}
