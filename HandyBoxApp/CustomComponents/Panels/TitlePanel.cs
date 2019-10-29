using HandyBoxApp.ColorScheme;
using HandyBoxApp.ColorScheme.Colors;
using HandyBoxApp.CustomComponents.Buttons;
using HandyBoxApp.CustomComponents.Panels.Base;

using System.Drawing;
using System.Windows.Forms;

namespace HandyBoxApp.CustomComponents.Panels
{
    internal sealed class TitlePanel : DraggablePanel
    {
        //################################################################################
        #region Constructor

        public TitlePanel(Control parentControl, bool isVertical) : base(parentControl, isVertical)
        {
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
        #region Internal Members

        internal void UpdatePanelContent(int width)
        {
            Width = width;
            TitleLabel.AutoSize = false;
            TitleLabel.Height = LogoLabel.Height;
            TitleLabel.Width = Width - LogoLabel.Width - CloseLabel.Width - Style.FormBorder * 2 - Controls.Count * Style.PanelSpacing - 1;

            CloseLabel.Location = new Point(TitleLabel.Location.X + TitleLabel.Width + Style.PanelSpacing, TitleLabel.Location.Y);
        }

        #endregion

        //################################################################################
        #region Overrides

        protected override void InitializeComponents()
        {
            //------------------------------------------------------------
            #region Panel Initialization

            Name = "TitlePanel";
            Border = new Border(Color.FromArgb(22, 22, 22), 1);

            #endregion

            //------------------------------------------------------------
            #region Logo Label Initialization

            void LogoAction(ClickImageButton button)
            {
                button.DoubleClick += (sender, arg) =>
                {
                    ParentControl.Visible = false;
                };
            }
            LogoLabel = new ClickImageButton(LogoAction, "L");
            LogoLabel.SetToolTip("Handy Box App v2.4");
            LogoLabel.SetColor<Blue>(PaintMode.Light);
            Controls.Add(LogoLabel);

            #endregion

            //------------------------------------------------------------
            #region Title Label Initialization

            void TitleAction(ClickImageButton button)
            {
                button.MouseDown += DragAndDrop;
            }
            TitleLabel = new ClickImageButton(TitleAction, "Handy Box App");
            TitleLabel.SetColor<Black>(PaintMode.Dark);
            Controls.Add(TitleLabel);

            #endregion

            //------------------------------------------------------------
            #region Close Label Initialization

            void CloseAction(ClickImageButton button)
            {
                button.Click += (sender, args) =>
                {
                    ((MainForm)ParentControl).Close();
                    CustomApplicationContext.Exit();
                };
            }
            CloseLabel = new ClickImageButton(CloseAction, "X");
            CloseLabel.SetToolTip("Close");
            CloseLabel.SetColor<Red>(PaintMode.Dark);
            Controls.Add(CloseLabel);

            #endregion
        }

        #endregion
    }
}
