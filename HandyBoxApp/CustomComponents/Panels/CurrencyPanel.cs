using HandyBoxApp.CurrencyService;
using HandyBoxApp.CustomComponents.Panels.Base;
using HandyBoxApp.EventArgs;
using HandyBoxApp.Properties;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace HandyBoxApp.CustomComponents.Panels
{
    internal sealed class CurrencyPanel : DynamicPanel
    {
        //################################################################################
        #region Fields

        private Color m_BorderColor;

        #endregion

        //################################################################################
        #region Constructor

        public CurrencyPanel(ICurrencyService currencyService, Control parentControl) : this(currencyService, parentControl, 1000)
        {
        }

        public CurrencyPanel(ICurrencyService currencyService, Control parentControl, int refreshRate) : base(parentControl)
        {
            CurrencyService = currencyService;
            RefreshRate = refreshRate;

            InitializeComponents();

            Paint += PaintBorder;
        }

        #endregion

        //################################################################################
        #region Properties

        private ICurrencyService CurrencyService { get; set; }

        private int RefreshRate { get; set; }

        private Label CurrencyName { get; set; }

        private Label CurrencyValue { get; set; }

        public Color BorderColor
        {
            get => m_BorderColor;

            set
            {
                m_BorderColor = value;
                PaintBorder(this, null);
            }
        }

        #endregion

        //################################################################################
        #region Overrides

        protected override void InitializeComponents()
        {
            //Initialize Panel
            m_BorderColor = Color.White;

            //Initialize Currency Name
            CurrencyName = new Label();


            //Initialize Currency Value
            CurrencyValue = new Label();


            //Initialize Function Button
            void Action()
            {
                MessageBox.Show("Currency function clicked");
            }

            Bitmap buttonImage = Resource.Close;
            AddFunction(Action, buttonImage, "Show functions");
        }

        protected override void PaintBorder(object sender, PaintEventArgs e)
        {
            var args = new BorderEventArgs(m_BorderColor, 1);
            PaintPanelBorder(sender, args);
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


        #endregion
    }
}
