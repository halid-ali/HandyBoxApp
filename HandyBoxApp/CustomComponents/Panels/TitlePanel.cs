using HandyBoxApp.ColorScheme;
using HandyBoxApp.ColorScheme.Colors;
using HandyBoxApp.CustomComponents.Buttons;
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
            SizeChanged += UpdatePanelContent;
            InitializeComponents();
        }

        #endregion

        //################################################################################
        #region Properties

        private ClickImageButton LogoLabel { get; set; }

        private ClickImageButton TitleLabel { get; set; }

        private ClickImageButton CloseLabel { get; set; }

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

            void Logo_Action(ClickImageButton button)
            {
                button.DoubleClick += (sender, arg) => 
                {
                    ParentControl.Visible = false;
                };
            }
            LogoLabel = new ClickImageButton(this, Logo_Action, "L");
            LogoLabel.SetToolTip("Handy Box App v2.4");
            LogoLabel.SetColor<Blue>(PaintMode.Light);
            Controls.Add(LogoLabel);

            #endregion

            //------------------------------------------------------------
            #region Title Label Initialization

            void Title_Action(ClickImageButton button)
            {
                button.MouseDown += DragAndDrop;
            }
            TitleLabel = new ClickImageButton(this, Title_Action, "Handy Box App");
            TitleLabel.SetColor<Black>(PaintMode.Dark);
            Controls.Add(TitleLabel);

            #endregion

            //------------------------------------------------------------
            #region Close Label Initialization

            void Close_Action(ClickImageButton button)
            {
                button.Click += (sender, args) =>
                {
                    ((MainForm)ParentControl).Close();
                    CustomApplicationContext.Exit();
                };
            }
            CloseLabel = new ClickImageButton(this, Close_Action, "X");
            CloseLabel.SetToolTip("Close");
            CloseLabel.SetColor<Red>(PaintMode.Dark);
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
    }
}
