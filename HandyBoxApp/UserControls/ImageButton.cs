using HandyBoxApp.ColorScheme;
using HandyBoxApp.CustomComponents;

using System;
using System.Drawing;
using System.Windows.Forms;

namespace HandyBoxApp.UserControls
{
    public class ImageButton : UserControl
    {
        //################################################################################
        #region Fields

        private readonly ToolTip m_ToolTip = new ToolTip();

        #endregion

        //################################################################################
        #region Constructor

        public ImageButton(Action<Control> action, string labelText)
        {
            InitializeComponent(labelText);

            action?.Invoke(TextLabel);
        }

        #endregion

        //################################################################################
        #region Properties

        private Label TextLabel { get; } = new Label();

        #endregion

        //################################################################################
        #region Internal Members

        internal void SetToolTip(string toolTipText)
        {
            m_ToolTip.SetToolTip(TextLabel, toolTipText);
        }

        internal void SetColor<T>(PaintMode paintMode) where T : ColorBase, new()
        {
            Painter<T>.Paint(TextLabel, paintMode);
        }

        #endregion

        //################################################################################
        #region Private Members

        private void InitializeComponent(string labelText)
        {
            SuspendLayout();

            #region TextLabel

            TextLabel.AutoSize = true;
            TextLabel.Text = labelText;
            TextLabel.Margin=new Padding(0);
            TextLabel.Height = TextLabel.Width;
            TextLabel.TextAlign = ContentAlignment.MiddleCenter;
            TextLabel.Padding = new Padding(Style.PanelPadding);
            TextLabel.Font = new Font(new FontFamily(Style.FontName), Style.PanelFontSize, FontStyle.Bold);

            #endregion

            #region ImageButton

            Size = TextLabel.PreferredSize;

            #endregion

            Controls.Add(TextLabel);

            ResumeLayout(false);
        }

        #endregion
    }
}
