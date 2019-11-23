using HandyBoxApp.CustomComponents;
using HandyBoxApp.Properties;
using HandyBoxApp.StockExchange;
using HandyBoxApp.UserControls;

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

            InitializeComponent();
            InitializePanels();
        }

        #endregion

        //################################################################################
        #region Properties

        private TitlePanel TitlePanel { get; set; }

        private StockPanel EurTryStockPanel { get; set; }

        private StockPanel UsdTryStockPanel { get; set; }

        private StockPanel EurUsdStockPanel { get; set; }

        private StockPanel SiemensStockPanel { get; set; }

        private TimerPanel HourPanel { get; set; }

        public LayoutPanel LayoutPanel { get; private set; }

        #endregion

        //################################################################################
        #region Event Handlers

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            SetFormPosition();
            Opacity = Settings.Default.Transparency;
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
            LayoutPanel = new LayoutPanel(this);
            TitlePanel = new TitlePanel(this);

            EurTryStockPanel = new StockPanel(this, StockServiceFactory.CreateService("Yahoo", "EURTRY"));
            UsdTryStockPanel = new StockPanel(this, StockServiceFactory.CreateService("Yahoo", "USDTRY"));
            EurUsdStockPanel = new StockPanel(this, StockServiceFactory.CreateService("Yahoo", "EURUSD"), 5000);
            SiemensStockPanel = new StockPanel(this, StockServiceFactory.CreateService("Yahoo", "SIEMENS"), 10000);

            HourPanel = new TimerPanel(this);

            LayoutPanel.Add(TitlePanel);

            LayoutPanel.Add(EurTryStockPanel);
            LayoutPanel.Add(UsdTryStockPanel);
            LayoutPanel.Add(EurUsdStockPanel);
            LayoutPanel.Add(SiemensStockPanel);

            LayoutPanel.Add(HourPanel);

            Controls.Add(LayoutPanel);

            TitlePanel.CustomContextMenu = CustomContextMenu.Instance;
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
