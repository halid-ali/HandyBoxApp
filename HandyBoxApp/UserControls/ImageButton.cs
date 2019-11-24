using HandyBoxApp.ColorScheme;
using HandyBoxApp.ColorScheme.Colors;
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

        public ImageButton(Action<Control> action, Bitmap labelImage)
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

        internal void SetImage(Bitmap bitmap)
        {
            if (bitmap != null)
            {
                TextLabel.BackgroundImage = bitmap;
                SetBackgroundColor(Color.FromArgb(152, 0, 47));
                //Painter<Black>.Paint(TextLabel, PaintMode.Dark);
            }
            else
            {
                TextLabel.Text = "X";
            }
        }

        internal void SetToolTip(string toolTipText)
        {
            m_ToolTip.SetToolTip(TextLabel, toolTipText);
        }

        internal void SetBackgroundColor(Color color)
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
            TextLabel.BackgroundImage = labelImage;
            TextLabel.BackgroundImageLayout = ImageLayout.Stretch;
            TextLabel.Height = TextLabel.Width;
            TextLabel.Margin = new Padding(0);
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
