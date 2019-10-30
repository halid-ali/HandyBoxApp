using HandyBoxApp.CurrencyService;
using HandyBoxApp.CurrencyService.Types;
using HandyBoxApp.CustomComponents.Panels;
using HandyBoxApp.CustomComponents.Panels.Base;
using HandyBoxApp.EventArgs;
using HandyBoxApp.Properties;
using HandyBoxApp.Utilities;

using System.Drawing;
using System.Windows.Forms;

namespace HandyBoxApp
{
    public partial class MainForm : Form
    {
        //################################################################################
        #region Constructor

        public MainForm()
        {
            Closing += OnFormClosed;
            ControlAdded += OnControlAdd;
            ControlRemoved += OnControlRemove;

            InitializeComponent();
            InitializePanels();

            //todo: this call title panel specific, make more generic
            TitlePanel.UpdatePanelContent(LargestContent);
        }

        #endregion

        //################################################################################
        #region Properties

        private TitlePanel TitlePanel { get; set; }

        private CurrencyPanel EurTryCurrencyPanel { get; set; }

        private CurrencyPanel EurUsdCurrencyPanel { get; set; }

        private CurrencyPanel UsdTryCurrencyPanel { get; set; }

        private ContainerPanel ContainerPanel { get; set; }

        private int LargestContent { get; set; }

        private int SlidePanelsCunt { get; set; }

        #endregion

        //################################################################################
        #region Event Handlers

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            ContainerPanel = new ContainerPanel(this, false)
            {
                Location = new Point(0, 0)
            };
            Controls.Add(ContainerPanel);

            SetFormPosition();
            Opacity = Settings.Default.Transparency;
        }

        private void OnControlAdd(object sender, ControlEventArgs e)
        {
            if (e.Control.Width > LargestContent)
            {
                LargestContent = e.Control.Width;
            }

            CustomControlHelper.VerticalAlign(this);

            (e.Control as DynamicPanel)?.InitializeFunctionPanel();
        }

        private void OnControlRemove(object sender, ControlEventArgs e)
        {
            SlidePanelsCunt = 0;
            CustomControlHelper.VerticalAlign(this);
            ContainerPanel.Height = Height;
        }

        private void OnFormSlide(object sender, SlideEventArgs args)
        {
            if (args.IsSlide)
            {
                if (SlidePanelsCunt == 0)
                {
                    Width += args.Slide;
                }

                SlidePanelsCunt++;
            }
            else
            {
                SlidePanelsCunt--;

                if (SlidePanelsCunt == 0)
                {
                    Width += args.Slide;
                }
            }
        }

        private void OnFormClosed(object sender, System.EventArgs e)
        {
            Settings.Default.LastLocation = Location;
            Settings.Default.Save();
        }

        #endregion

        //################################################################################
        #region Private Members

        private void InitializePanels()
        {
            TitlePanel = new TitlePanel(this, false);
            Controls.Add(TitlePanel);

            EurTryCurrencyPanel = new CurrencyPanel(new EurTryCurrency(CurrencyUrls.YahooEurTry), this, false);
            EurTryCurrencyPanel.OnFormSlide += OnFormSlide;
            Controls.Add(EurTryCurrencyPanel);

            UsdTryCurrencyPanel = new CurrencyPanel(new UsdTryCurrency(CurrencyUrls.YahooUsdTry), this, false);
            UsdTryCurrencyPanel.OnFormSlide += OnFormSlide;
            Controls.Add(UsdTryCurrencyPanel);

            EurUsdCurrencyPanel = new CurrencyPanel(new EurUsdCurrency(CurrencyUrls.YahooEurUsd), this, false);
            EurUsdCurrencyPanel.OnFormSlide += OnFormSlide;
            Controls.Add(EurUsdCurrencyPanel);
        }

        private void SetFormPosition()
        {
            var lastLocation = Settings.Default.LastLocation;

            if (lastLocation.X == 0 && lastLocation.Y == 0)
            {
                //set default location
                var marginRight = 30;
                var marginBottom = 60;
                var screenW = Screen.PrimaryScreen.Bounds.Width;
                var screenH = Screen.PrimaryScreen.Bounds.Height;

                Top = screenH - Height - marginBottom;
                Left = screenW - Width - marginRight;

                lastLocation = new Point(Left, Top);
                Settings.Default.LastLocation = lastLocation;
                Settings.Default.Save();
            }

            Location = lastLocation;
        }

        #endregion
    }
}
