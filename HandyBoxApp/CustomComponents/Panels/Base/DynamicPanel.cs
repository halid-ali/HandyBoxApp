using HandyBoxApp.CustomComponents.Buttons;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace HandyBoxApp.CustomComponents.Panels.Base
{
    internal abstract class DynamicPanel : CustomBasePanel
    {
        //################################################################################
        #region Constructor

        protected DynamicPanel(Control parentControl, bool isVertical) : base(parentControl, isVertical)
        {
            FunctionList = new List<ClickImageButton>();

            BackgroundWorker = new BackgroundWorker();
            InitializeBackgroundWorker();
        }

        #endregion

        //################################################################################
        #region Properties

        protected BackgroundWorker BackgroundWorker { get; }

        protected ContainerPanel ContainerPanel { get; private set; }

        protected List<ClickImageButton> FunctionList { get; }

        protected bool IsStopped { get; set; }

        #endregion

        //################################################################################
        #region Abstract Methods

        protected abstract void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e);

        protected abstract void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e);

        #endregion

        //################################################################################
        #region Internal Members

        internal void InitializeFunctionPanel()
        {
            ContainerPanel = new ContainerPanel(this, false);

            foreach (Control control in FunctionList)
            {
                ContainerPanel.Controls.Add(control);
            }
            ContainerPanel.Location = GetLocation();

            ParentControl.Controls.Add(ContainerPanel);
        }

        #endregion

        private Point GetLocation()
        {
            var x = Location.X + ParentControl.Width - ContainerPanel.Width - (Style.PanelSpacing * 2 + Style.FormBorder * 2);
            var y = Location.Y;

            return new Point(x, y);
        }

        //################################################################################
        #region Protected Members

        protected void AddFunction(Action<ClickImageButton> action, string labelText)
        {
            FunctionList.Add(new ClickImageButton(action, labelText));
        }

        #endregion

        //################################################################################
        #region Private Members

        private void InitializeBackgroundWorker()
        {
            BackgroundWorker.WorkerReportsProgress = false;
            BackgroundWorker.WorkerSupportsCancellation = false;

            BackgroundWorker.DoWork += BackgroundWorker_DoWork;
            BackgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
        }

        #endregion
    }
}
