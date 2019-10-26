using HandyBoxApp.CurrencyService;
using HandyBoxApp.CustomComponents.Buttons;
using HandyBoxApp.CustomComponents.Panels.Base;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace HandyBoxApp.CustomComponents.Panels
{
    internal sealed class CurrencyPanel : DynamicPanel
    {
        //################################################################################
        #region Constructor

        public CurrencyPanel(ICurrency currency, Control parentControl) : this(currency, parentControl, 1000)
        {
        }

        public CurrencyPanel(ICurrency currency, Control parentControl, int refreshRate) : base(parentControl)
        {
            Currency = currency;
            RefreshRate = refreshRate;

            InitializeComponents();
        }

        #endregion

        //################################################################################
        #region Properties

        private ICurrency Currency { get; set; }

        private int RefreshRate { get; set; }

        private CustomLabel CurrencyName { get; set; }

        private CustomLabel CurrencyValue { get; set; }

        private ClickImageButton FunctionSwitch { get; set; }

        #endregion

        //################################################################################
        #region Overrides

        protected override void InitializeComponents()
        {
            //Initialize Panel
            Border = new Border(Color.Turquoise, 1);
            Paint += PaintBorder;

            //Initialize Currency Name
            CurrencyName = new CustomLabel(this, Currency.Name);
            Controls.Add(CurrencyName);

            //Initialize Currency Value
            CurrencyValue = new CustomLabel(this, @"6,4123 TL");
            Controls.Add(CurrencyValue);

            //Initialize function switch button
            void Action()
            {
                MessageBox.Show("Currency function clicked");
            }

            FunctionSwitch = new ClickImageButton(this, Action, "Show functions");
            Controls.Add(FunctionSwitch);

            //Initialize Function Buttons
            //AddFunction(Action, buttonImage, "Show functions");

            Size = GetPanelDimensions();
        }

        protected override void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
