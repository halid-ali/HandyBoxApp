using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HandyBoxApp.CustomComponents.Panels.Base
{
    internal abstract class DraggablePanel : CustomBasePanel
    {
        //################################################################################
        #region Constants

        private const int c_WmNclButtonDown = 0xA1;
        private const int c_HtCaption = 0x2;

        #endregion

        //################################################################################
        #region DLL Imports

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        #endregion

        //################################################################################
        #region Constructor

        protected DraggablePanel(Control parentControl) : base(parentControl)
        {

        }

        #endregion

        //################################################################################
        #region Abstract Methods

        protected void DragAndDrop(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Parent.Handle, c_WmNclButtonDown, c_HtCaption, 0);
            }
        }

        #endregion
    }
}
