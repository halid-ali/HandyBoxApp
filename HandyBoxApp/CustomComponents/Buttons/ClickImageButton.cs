using System;
using System.Drawing;
using System.Windows.Forms;

namespace HandyBoxApp.CustomComponents.Buttons
{
    internal class ClickImageButton : Label
    {
        //################################################################################
        #region Fields

        private readonly Action m_Action;
        private readonly Bitmap m_Image;

        #endregion

        //################################################################################
        #region Constructor

        public ClickImageButton(Action action, Bitmap image, string tooltip)
        {
            m_Action = action;
            m_Image = image;

            var toolTip = new ToolTip();
            toolTip.SetToolTip(this, tooltip);

            InitializeComponent();

            Click += Button_Click;
        }

        #endregion

        //################################################################################
        #region Event Handlers

        private void Button_Click(object sender, System.EventArgs e)
        {
            m_Action();
        }

        #endregion

        //################################################################################
        #region Private Members

        private void InitializeComponent()
        {
            BackgroundImage = m_Image;
        }

        #endregion
    }
}
