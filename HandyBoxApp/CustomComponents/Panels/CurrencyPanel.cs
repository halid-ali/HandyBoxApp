using HandyBoxApp.ColorScheme;
using HandyBoxApp.ColorScheme.Colors;
using HandyBoxApp.CurrencyService;
using HandyBoxApp.CurrencyService.Services;
using HandyBoxApp.CustomComponents.Buttons;
using HandyBoxApp.CustomComponents.Panels.Base;
using HandyBoxApp.EventArgs;
using HandyBoxApp.Utilities;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace HandyBoxApp.CustomComponents.Panels
{
    internal sealed class CurrencyPanel : DynamicPanel
    {
        //################################################################################
        #region Constructor

        public CurrencyPanel(ICurrency currency, Control parentControl) : this(currency, parentControl, Constants.DefaultRefreshRate)
        {
        }

        public CurrencyPanel(ICurrency currency, Control parentControl, int refreshRate) : base(parentControl)
        {
            Currency = currency;
            RefreshRate = refreshRate;

            InitializeComponents();

            Start();
        }

        #endregion

        //################################################################################
        #region Properties

        private ICurrency Currency { get; }

        private ICurrencyService CurrencyService { get; set; }

        private int RefreshRate { get; }

        private Label NameLabel { get; } = new Label();

        private Label ValueLabel { get; } = new Label();

        private ClickImageButton FunctionSwitch { get; set; }

        #endregion

        //################################################################################
        #region Overrides

        protected override void InitializeComponents()
        {
            //------------------------------------------------------------
            #region Panel Initialization

            Border = new Border(Color.FromArgb(51, 51, 51), 1);
            Paint += PaintBorder;

            #endregion

            //------------------------------------------------------------
            #region Currency Name Label Initialization

            SetLabel<Black>(NameLabel, PaintMode.Normal, Currency.Name);
            Controls.Add(NameLabel);

            #endregion

            //------------------------------------------------------------
            #region Currency Value Label Initialization

            SetLabel<Black>(ValueLabel, PaintMode.Normal, @"0,0000 TL");
            Controls.Add(ValueLabel);

            #endregion

            //------------------------------------------------------------
            #region Function Switch Button Initialization

            void Action()
            {
                MessageBox.Show($@"{Currency.Name} is clicked.");
            }
            FunctionSwitch = new ClickImageButton(this, Action, "Show functions");
            Controls.Add(FunctionSwitch);

            #endregion

            //------------------------------------------------------------
            #region Functions Initialization

            //AddFunction(Action, buttonImage, "Show functions");

            #endregion

            CustomControlHelper.SetHorizontalLocation(this);
            Size = GetPanelDimensions();
        }

        protected override void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            CurrencyService = new Yahoo(Currency.SourceUrl.Value);
            CurrencyService.OnCurrencyUpdated += UpdateCurrency;

            try
            {
                while (!IsStopped)
                {
                    CurrencyService.GetUpdatedRateData();
                    Thread.Sleep(RefreshRate);
                }
            }
            finally
            {
                CurrencyService.OnCurrencyUpdated -= UpdateCurrency;
            }
        }

        protected override void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        //################################################################################
        #region Private Members

        private void Start()
        {
            BackgroundWorker.RunWorkerAsync();
        }

        private void UpdateCurrency(object sender, CurrencyUpdatedEventArgs e)
        {
            if (ValueLabel.InvokeRequired)
            {
                CurrencyUpdateCallback callback = UpdateCurrency;
                Invoke(callback, sender, e);
            }
            else
            {
                CurrencySummaryData currencySummary = e.CurrencySummary;

                if (currencySummary.Actual > CurrencyService.PreviousCurrencyData.Actual)
                {
                    Painter<Green>.Paint(ValueLabel, PaintMode.Light);
                }
                else if (currencySummary.Actual < CurrencyService.PreviousCurrencyData.Actual)
                {
                    Painter<Red>.Paint(ValueLabel, PaintMode.Light);
                }

                ValueLabel.Text = $@"{currencySummary.Actual:F4} TL";
                CurrencyService.PreviousCurrencyData = currencySummary;
            }
        }

        private void SetLabel<T>(Label label, PaintMode paintMode, string text) where T : ColorBase, new()
        {
            label.Text = text;
            label.AutoSize = true;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Padding = new Padding(Style.PanelPadding);
            label.Font = new Font(new FontFamily(Style.FontName), Style.PanelFontSize, FontStyle.Bold);

            Painter<T>.Paint(label, paintMode);
        }

        #endregion
    }
}
