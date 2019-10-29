using HandyBoxApp.CustomComponents.Panels.Base;
using System;
using System.Windows.Forms;

namespace HandyBoxApp.CustomComponents.Panels
{
    internal sealed class ContainerPanel : StaticPanel
    {
        public ContainerPanel(Control parentControl, bool isVertical) : base(parentControl, isVertical)
        {
        }

        protected override void InitializeComponents()
        {
            throw new NotImplementedException();
        }
    }
}
