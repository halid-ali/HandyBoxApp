using HandyBoxApp.ColorScheme;
using HandyBoxApp.CustomComponents;

using System;
using System.Drawing;
using System.Windows.Forms;
using HandyBoxApp.ColorScheme.Colors;

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

        public ImageButton(Action<Control> action, Bitmap labelImage = null)
        {
            InitializeComponent(labelImage);

            action?.Invoke(TextLabel);
        }

        #endregion

        //################################################################################
        #region Properties

        private Label TextLabel { get; } = new Label();

        #endregion

        //################################################################################
        #region Internal Members

        internal void SetImage(Bitmap bitmap, Color backColor)
        {
            TextLabel.BackgroundImage = bitmap;
            SetColor(backColor);
        }

        internal void SetToolTip(string toolTipText)
        {
            m_ToolTip.SetToolTip(TextLabel, toolTipText);
        }

        internal void SetColor(Color color)
        {
            Painter<None>.Paint(TextLabel, color);
        }

        #endregion

        //################################################################################
        #region Private Members

        private void InitializeComponent(Bitmap labelImage)
        {
            SuspendLayout();

            #region TextLabel

            TextLabel.Text = " ";
            TextLabel.AutoSize = true;

            if (labelImage != null)
            {
                TextLabel.BackgroundImage = labelImage;
                TextLabel.BackgroundImageLayout = ImageLayout.Stretch;
            }
            else
            {
                TextLabel.Text = "X";
            }

            TextLabel.Margin = new Padding(0);
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
