using HandyBoxApp.ColorScheme;
using HandyBoxApp.ColorScheme.Colors;
using HandyBoxApp.Utilities;

using System;
using System.Drawing;
using System.Windows.Forms;

namespace HandyBoxApp.CustomComponents.Buttons
{
    internal class ClickImageButton : Label
    {
        //################################################################################
        #region Constructor

        public ClickImageButton(Control parentControl, Action action, string tooltip)
        {
            ParentControl = parentControl;
            Action = action;

            InitializeComponent();

            var toolTip = new ToolTip();
            toolTip.SetToolTip(this, tooltip);

            Click += Button_Click;
        }

        #endregion

        //################################################################################
        #region Properties

        private Control ParentControl { get; set; }

        private Action Action { get; set; }

        #endregion

        //################################################################################
        #region Event Handlers

        private void Button_Click(object sender, System.EventArgs e)
        {
            Action();
        }

        #endregion

        //################################################################################
        #region Private Members

        private void InitializeComponent()
        {
            AutoSize = true;
            Text = @"»";
            Painter<Green>.Paint(this, PaintMode.Light);
            Padding = new Padding(Style.PanelPadding);
            Font = new Font(new FontFamily("Consolas"), Style.PanelFontSize, FontStyle.Bold);
            Visible = true;

            Width = Height;
            Location = CustomControlHelper.SetHorizontalLocation(ParentControl);
        }

        #endregion
    }
}
