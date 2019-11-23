using HandyBoxApp.ColorScheme;
using HandyBoxApp.ColorScheme.Colors;
using HandyBoxApp.CustomComponents;
using HandyBoxApp.Properties;

using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HandyBoxApp.UserControls
{
    public class TitlePanel : UserControl
    {
        //################################################################################
        #region Constants

        private readonly int c_WmNclButtonDown = 0xA1;
        private readonly IntPtr c_HtCaption = (IntPtr)0x2;

        #endregion

        //################################################################################
        #region Native Methods

        private class NativeMethods
        {
            [DllImport("user32.dll")]
            internal static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

            [DllImport("user32.dll")]
            internal static extern bool ReleaseCapture();
        }

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

        internal CustomContextMenu CustomContextMenu
        {
            set
            {
                LogoButton.ContextMenu = value;
            }
        }

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
            TitleLabel.Text = "Handy Box App v2.4  ";
            //TitleLabel.Width = 190 - LogoButton.Width;
            TitleLabel.AutoSize = true;
            //TitleLabel.UseCompatibleTextRendering = true;
            TitleLabel.Margin = new Padding(0, 0, Style.PanelSpacing, 0);
            TitleLabel.Padding = new Padding(Style.PanelPadding);
            TitleLabel.TextAlign = ContentAlignment.MiddleLeft;
            //TitleLabel.Font = new Font(AddFontFromMemory(), Style.PanelFontSize, FontStyle.Regular);
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

        private FontFamily AddFontFromMemory()
        {
            var privateFontCollection = new PrivateFontCollection();

            using (var fontStream = new MemoryStream(Resources.Courier_Prime))
            {
                // create an unsafe memory block for the font data
                IntPtr data = Marshal.AllocCoTaskMem((int)fontStream.Length);

                // create a buffer to read in to
                byte[] fontdata = new byte[fontStream.Length];

                // read the font data from the resource
                fontStream.Read(fontdata, 0, (int)fontStream.Length);

                // copy the bytes to the unsafe memory block
                Marshal.Copy(fontdata, 0, data, (int)fontStream.Length);

                // pass the font to the font collection
                privateFontCollection.AddMemoryFont(data, (int)fontStream.Length);

                // close the resource stream
                fontStream.Close();

                // free up the unsafe memory
                Marshal.FreeCoTaskMem(data);
            }

            return privateFontCollection.Families[0];
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

                //ContainerPanel.Width += control.Width - Style.PanelSpacing;
                ContainerPanel.Width += control.PreferredSize.Width - Style.PanelSpacing;
            }

            Height = ContainerPanel.Height;
            Width = ContainerPanel.Width;
        }

        #endregion

        //################################################################################
        #region Event Handlers

        private void DragAndDrop(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            NativeMethods.ReleaseCapture();
            NativeMethods.SendMessage(ParentControl.Handle, c_WmNclButtonDown, c_HtCaption, (IntPtr)0);
        }

        #endregion
    }
}
