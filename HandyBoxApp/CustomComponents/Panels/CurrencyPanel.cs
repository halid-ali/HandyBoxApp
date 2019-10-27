using HandyBoxApp.ColorScheme;
using HandyBoxApp.ColorScheme.Colors;
using HandyBoxApp.CurrencyService;
using HandyBoxApp.CustomComponents.Buttons;
using HandyBoxApp.CustomComponents.Panels.Base;
using HandyBoxApp.Utilities;

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

        private Label NameLabel { get; } = new Label();

        private Label ValueLabel { get; } = new Label();

        private ClickImageButton FunctionSwitch { get; set; }

        #endregion

        //################################################################################
        #region Overrides

        protected override void InitializeComponents()
        {
            //Initialize Panel
            Border = new Border(Color.White, 1);
            Paint += PaintBorder;

            //Initialize Currency Name
            SetLabel<Red>(NameLabel, Currency.Name);
            Controls.Add(NameLabel);

            //Initialize Currency Value
            SetLabel<Blue>(ValueLabel, @"6,4123 TL");
            Controls.Add(ValueLabel);

            //Initialize function switch button
            void Action()
            {
                MessageBox.Show($@"{Currency.Name} is clicked.");
            }

            FunctionSwitch = new ClickImageButton(this, Action, "Show functions");
            Controls.Add(FunctionSwitch);

            //Initialize Function Buttons
            //AddFunction(Action, buttonImage, "Show functions");

            CustomControlHelper.SetHorizontalLocation(this);
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

        //################################################################################
        #region Private Members

        private void SetLabel<T>(Label label, string text) where T : ColorBase, new()
        {
            label.Text = text;
            label.AutoSize = true;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Padding = new Padding(Style.PanelPadding);
            label.Font = new Font(new FontFamily(Style.FontName), Style.PanelFontSize, FontStyle.Bold);

            Painter<T>.Paint(label, PaintMode.Normal);
        }

        #endregion
    }
}
