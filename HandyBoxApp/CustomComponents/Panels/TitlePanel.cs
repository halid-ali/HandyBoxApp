using HandyBoxApp.ColorScheme;
using HandyBoxApp.ColorScheme.Colors;
using HandyBoxApp.CustomComponents.Panels.Base;
using HandyBoxApp.Utilities;

using System.Drawing;
using System.Windows.Forms;

namespace HandyBoxApp.CustomComponents.Panels
{
    internal sealed class TitlePanel : DraggablePanel
    {
        //################################################################################
        #region Constructor

        public TitlePanel(Control parentControl) : base(parentControl)
        {
            LogoLabel = new Label();
            CloseLabel = new Label();
            TitleLabel = new Label();

            InitializeComponents();
        }

        #endregion

        //################################################################################
        #region Properties

        private Label LogoLabel { get; set; }

        private Label TitleLabel { get; set; }

        private Label CloseLabel { get; set; }

        #endregion

        //################################################################################
        #region Overrides

        protected override void InitializeComponents()
        {
            #region Panel Initialization

            Name = "TitlePanel";
            Paint += PaintBorder;

            #endregion

            #region Image Label Initialization

            LogoLabel.Name = "LogoLabel";
            LogoLabel.Text = @"X";
            LogoLabel.Visible = true;
            LogoLabel.AutoSize = true;
            LogoLabel.Cursor = Cursors.Default;
            Painter<Green>.Paint(LogoLabel, PaintMode.Light);
            LogoLabel.Padding = new Padding(Style.PanelPadding);
            LogoLabel.Font = new Font(new FontFamily("Consolas"), Style.PanelFontSize, FontStyle.Bold);
            LogoLabel.DoubleClick += LogoLabel_DoubleClick;
            LogoLabel.Location = CustomControlHelper.SetLocation(this);
            Controls.Add(LogoLabel);

            #endregion

            #region Title Label Initialization

            TitleLabel.Name = "TitleLabel";
            TitleLabel.Text = @"Handy Box";
            TitleLabel.Visible = true;
            TitleLabel.AutoSize = true;
            Painter<Black>.Paint(TitleLabel, PaintMode.Light);
            TitleLabel.Padding = new Padding(Style.PanelPadding);
            TitleLabel.Font = new Font(new FontFamily("Consolas"), Style.PanelFontSize, FontStyle.Bold);
            TitleLabel.MouseDown += DragAndDrop;
            TitleLabel.Location = CustomControlHelper.SetLocation(this);
            Controls.Add(TitleLabel);

            #endregion

            #region Close Label Initialization

            CloseLabel.Name = "CloseLabel";
            CloseLabel.AutoSize = true;
            CloseLabel.Text = @"X";
            Painter<Green>.Paint(CloseLabel, PaintMode.Light);
            CloseLabel.Padding = new Padding(Style.PanelPadding);
            CloseLabel.Font = new Font(new FontFamily("Consolas"), Style.PanelFontSize, FontStyle.Bold);
            CloseLabel.Visible = true;
            CloseLabel.Click += CloseLabel_Click;
            CloseLabel.Location = CustomControlHelper.SetLocation(this);
            Controls.Add(CloseLabel);

            #endregion

            Size = GetPanelDimensions();
        }

        #endregion

        //################################################################################
        #region Event Handlers

        private void LogoLabel_DoubleClick(object sender, System.EventArgs e)
        {
            ParentControl.Visible = false;
        }

        private void CloseLabel_Click(object sender, System.EventArgs e)
        {
            CustomApplicationContext.Exit();
        }

        #endregion
    }
}
