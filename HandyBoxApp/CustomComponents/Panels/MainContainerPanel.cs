using HandyBoxApp.CustomComponents.Panels.Base;

using System;
using System.Windows.Forms;

namespace HandyBoxApp.CustomComponents.Panels
{
    internal sealed class MainContainerPanel : StaticPanel
    {
        public MainContainerPanel(Control parentControl) : base(parentControl)
        {
        }

        protected override void InitializeComponents()
        {
            throw new NotImplementedException();
        }

        protected override void PaintBorder(object sender, PaintEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
