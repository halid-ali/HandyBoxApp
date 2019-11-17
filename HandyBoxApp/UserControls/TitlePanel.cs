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
            //ParentControl.SizeChanged += ParentControl_SizeChanged;

            InitializeComponent();
            OrderControls();
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

            #region Logo Button

            void HideAction(Control button)
            {
                button.DoubleClick += (sender, arg) =>
                {
                    ParentControl.Visible = false;
                };
            }

            LogoButton = new ImageButton(HideAction, "L") { Margin = new Padding(0, 0, Style.PanelSpacing, 0) };
            LogoButton.SetToolTip("Handy Box App v2.4");
            LogoButton.SetColor<Blue>(PaintMode.Light);

            #endregion

            #region Title Label

            TitleLabel.Name = "TitleLabel";
            TitleLabel.Text = "Handy Box App v2.4";
            TitleLabel.AutoSize = true;
            TitleLabel.Margin = new Padding(0, 0, Style.PanelSpacing, 0);
            TitleLabel.Padding = new Padding(Style.PanelPadding);
            TitleLabel.TextAlign = ContentAlignment.MiddleLeft;
            TitleLabel.Font = new Font(new FontFamily(Style.FontName), Style.PanelFontSize, FontStyle.Bold);
            Painter<Black>.Paint(TitleLabel, PaintMode.Dark);
            TitleLabel.MouseDown += DragAndDrop;

            #endregion

            #region Close Button

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

        private void OrderControls()
        {
            ContainerPanel.Width = ContainerPanel.Controls.Count * 2 - 1;
            ContainerPanel.Height = 0;

            foreach (Control control in ContainerPanel.Controls)
            {
                if (control.PreferredSize.Height > ContainerPanel.Height)
                {
                    ContainerPanel.Height = control.PreferredSize.Height;
                }

                ContainerPanel.Width += control.PreferredSize.Width - Style.PanelSpacing;
            }

            Height = ContainerPanel.Height;
            Width = ContainerPanel.Width;
        }

        //private void UpdateControls(Control adaptingControl)
        //{
        //    var borderSpacing = 2 * Style.FormBorder + 2 * Style.PanelSpacing;
        //    var enlarged = ParentControl.Width - Width - borderSpacing;

        //    var width = adaptingControl.Width + enlarged;
        //    var height = adaptingControl.Height;

        //    adaptingControl.Width = width;
        //    adaptingControl.Height = height;

        //    Width = ParentControl.Width - borderSpacing;
        //}

        #endregion

        //################################################################################
        #region Event Handlers

        private void DragAndDrop(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            ReleaseCapture();
            SendMessage(ParentControl.Handle, c_WmNclButtonDown, c_HtCaption, 0);
        }

        //private void ParentControl_SizeChanged(object sender, System.EventArgs e)
        //{
        //    var borderSpacing = 2 * Style.FormBorder + 2 * Style.PanelSpacing;

        //    if (ParentControl.Width > Width + borderSpacing)
        //    {
        //        //UpdateControls(TitleLabel);
        //    }
        //}

        #endregion
    }
}
