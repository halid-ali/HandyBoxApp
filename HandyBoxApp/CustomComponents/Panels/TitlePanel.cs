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

            SizeChanged += UpdatePanelContent;

            InitializeComponents();
        }

        #endregion

        //################################################################################
        #region Properties

        private Label LogoLabel { get; }

        private Label TitleLabel { get; }

        private Label CloseLabel { get; }

        #endregion

        //################################################################################
        #region Overrides

        protected override void InitializeComponents()
        {
            //------------------------------------------------------------
            #region Panel Initialization

            Name = "TitlePanel";
            Border = new Border(Color.FromArgb(22, 22, 22), 1);
            Paint += PaintBorder;

            #endregion

            //------------------------------------------------------------
            #region Logo Label Initialization

            SetLabel<Green>(LogoLabel, PaintMode.Light, "LogoLabel", "L");
            LogoLabel.DoubleClick += LogoLabel_DoubleClick;
            Controls.Add(LogoLabel);

            #endregion

            //------------------------------------------------------------
            #region Title Label Initialization

            SetLabel<Black>(TitleLabel, PaintMode.Dark, "TitleLabel", "Handy Box App");
            TitleLabel.MouseDown += DragAndDrop;
            Controls.Add(TitleLabel);

            #endregion

            //------------------------------------------------------------
            #region Close Label Initialization

            SetLabel<Red>(CloseLabel, PaintMode.Dark, "CloseLabel", "X");
            CloseLabel.Click += CloseLabel_Click;
            Controls.Add(CloseLabel);

            #endregion

            CustomControlHelper.SetHorizontalLocation(this);
            Size = GetPanelDimensions();
        }

        protected override void UpdatePanelContent(object sender, System.EventArgs e)
        {
            TitleLabel.AutoSize = false;
            TitleLabel.Width = Width - LogoLabel.Width - CloseLabel.Width - (Controls.Count + 1) * Style.PanelSpacing - Border.Size * 2;
            TitleLabel.Height = LogoLabel.Height;
            CustomControlHelper.SetHorizontalLocation(this);
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
            ((MainForm)ParentControl).Close();
            CustomApplicationContext.Exit();
        }

        #endregion

        //################################################################################
        #region Private Members

        private void SetLabel<T>(Label label, PaintMode paintMode, string name, string text) where T : ColorBase, new()
        {
            label.Name = name;
            label.Text = text;
            label.Visible = true;
            label.AutoSize = true;
            label.Padding = new Padding(Style.PanelPadding);
            label.Font = new Font(new FontFamily(Style.FontName), Style.PanelFontSize, FontStyle.Bold);
            Painter<T>.Paint(label, paintMode);
        }

        #endregion
    }
}
