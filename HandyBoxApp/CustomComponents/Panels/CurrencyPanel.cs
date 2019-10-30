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

        public CurrencyPanel(ICurrency currency, Control parentControl, bool isVertical) : this(currency, parentControl, Constants.DefaultRefreshRate, isVertical)
        {
        }

        public CurrencyPanel(ICurrency currency, Control parentControl, int refreshRate, bool isVertical) : base(parentControl, isVertical)
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

        private bool IsActive { get; set; }

        #endregion

        //################################################################################
        #region Overrides

        protected override void InitializeComponents()
        {
            //------------------------------------------------------------
            #region Panel Initialization

            //todo: move border initialization to the base class and parameterized the color
            Border = new Border(Color.FromArgb(22, 22, 22), 1);

            #endregion

            //------------------------------------------------------------
            #region Currency Name Label Initialization

            SetLabel<Black>(NameLabel, PaintMode.Normal, Currency.Name);
            Controls.Add(NameLabel);

            #endregion

            //------------------------------------------------------------
            #region Currency Value Label Initialization

            //todo: adjustable floating count after comma
            SetLabel<Black>(ValueLabel, PaintMode.Normal, CurrencyFormatter.Format(0, "tr-TR", "TL"));
            Controls.Add(ValueLabel);

            #endregion

            //------------------------------------------------------------
            #region Function Switch Button Initialization

            void Action(ClickImageButton button)
            {
                button.Click += (sender, args) =>
                {
                    SlideFunctionsPanel();
                };
            }
            FunctionSwitch = new ClickImageButton(Action, "»");
            FunctionSwitch.SetToolTip("Expand functions.");
            Controls.Add(FunctionSwitch);

            #endregion

            //------------------------------------------------------------
            #region Functions Initialization

            void Action1(ClickImageButton button)
            {
                Painter<Red>.Paint(button, PaintMode.Light);
                button.Click += (sender, args) =>
                {
                    MessageBox.Show($@"{Currency.Name}: Func_1");
                };
            }
            AddFunction(Action1, "1");

            void Action2(ClickImageButton button)
            {
                Painter<Green>.Paint(button, PaintMode.Light);
                button.Click += (sender, args) =>
                {
                    MessageBox.Show($@"{Currency.Name}: Func_2");
                };
            }
            AddFunction(Action2, "2");

            void Action3(ClickImageButton button)
            {
                Painter<Blue>.Paint(button, PaintMode.Light);
                button.Click += (sender, args) =>
                {
                    MessageBox.Show($@"{Currency.Name}: Func_3");
                };
            }
            AddFunction(Action3, "3");

            #endregion
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

                ValueLabel.Text = CurrencyFormatter.Format(currencySummary.Actual, "tr-TR", "TL");
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

        private void SlideFunctionsPanel()
        {
            IsActive = !IsActive;

            var x = ContainerPanel.Location.X;
            var y = ContainerPanel.Location.Y;
            var slide = ContainerPanel.Width + Style.PanelSpacing/* + 200*/;

            if (IsActive)
            {
                //expand the main form's visible area
                ParentControl.Width += slide;
                FunctionSwitch.Text = "«";

                for (int i = 0; i < slide; i++)
                {
                    ContainerPanel.Location = new Point(++x, y);
                    ContainerPanel.Update();
                }
            }
            else
            {
                for (int i = 0; i < slide; i++)
                {
                    ContainerPanel.Location = new Point(--x, y);
                    ContainerPanel.Update();
                }

                //narrow the main form's visible area
                ParentControl.Width -= slide;
                FunctionSwitch.Text = "»";
            }
        }

        #endregion
    }
}
