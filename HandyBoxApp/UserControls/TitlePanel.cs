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

        private ImageButton LogoButton { get; set; }

        private Label TitleLabel { get; } = new Label();

        private ImageButton CloseButton { get; set; }

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

            #endregion

            #region Logo Label

            void HideAction(Control button)
            {
                button.DoubleClick += (sender, arg) =>
                {
                    ParentControl.Visible = false;
                };
            }

            LogoButton = new ImageButton(HideAction, "L") { Margin = new Padding(0, 0, 1, 0) };
            LogoButton.SetToolTip("Handy Box App v2.4");
            LogoButton.SetColor<Blue>(PaintMode.Light);

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

            void CloseAction(Control button)
            {
                button.Click += (sender, args) =>
                {
                    ((MainForm)ParentControl).Close();
                    CustomApplicationContext.Exit();
                };
            }

            CloseButton = new ImageButton(CloseAction, "X") { Margin = new Padding(0) };
            CloseButton.SetToolTip("Close");
            CloseButton.SetColor<Red>(PaintMode.Dark);

            #endregion

            #region Title Panel

            Name = "TitlePanel";
            AutoSize = true;
            Margin = new Padding(0, 0, 0, Style.PanelSpacing);
            BackColor = Color.FromArgb(100, 100, 100);
            BorderStyle = BorderStyle.FixedSingle;
            AutoScaleMode = AutoScaleMode.Font;

            #endregion

            ContainerPanel.Controls.Add(LogoButton);
            ContainerPanel.Controls.Add(TitleLabel);
            ContainerPanel.Controls.Add(CloseButton);
            Controls.Add(ContainerPanel);

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

            ContainerPanel.Width -= ContainerPanel.Controls.Count + 1;
            Height = ContainerPanel.Height;
            Width = ContainerPanel.Width;
        }

        #endregion

        //################################################################################
        #region Event Handlers

        private void DragAndDrop(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            ReleaseCapture();
            SendMessage(ParentControl.Handle, c_WmNclButtonDown, c_HtCaption, 0);
        }

        #endregion
    }
}
