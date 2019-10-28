using HandyBoxApp.CurrencyService;
using HandyBoxApp.CurrencyService.Types;
using HandyBoxApp.CustomComponents;
using HandyBoxApp.CustomComponents.Panels;
using HandyBoxApp.Properties;
using HandyBoxApp.Utilities;

using System.Drawing;
using System.Windows.Forms;

namespace HandyBoxApp
{
    public partial class MainForm : Form
    {
        //################################################################################
        #region Fields

        private readonly Panel m_ContainerPanel;
        private int m_LargestPanelWidth;

        #endregion

        //################################################################################
        #region Constructor

        public MainForm()
        {
            m_ContainerPanel = new Panel();

            //Paint += PaintBorder;
            ControlAdded += OnControlAdd;
            Closing += OnFormClosed;

            InitializeComponent();
            InitializePanels();
            InitializeContainerPanel();
        }

        #endregion

        //################################################################################
        #region Properties

        private TitlePanel TitlePanel { get; set; }

        private CurrencyPanel EurTryCurrencyPanel { get; set; }

        private CurrencyPanel EurUsdCurrencyPanel { get; set; }

        private CurrencyPanel UsdTryCurrencyPanel { get; set; }

        #endregion

        //################################################################################
        #region Event Handlers

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            SetContainerPanelDimensions();
            SetMainFormDimensions();
            SetFormPosition();

            Opacity = Settings.Default.Transparency;
        }

        private void PaintBorder(object sender, PaintEventArgs e)
        {
            Rectangle borderRectangle = new Rectangle(new Point(0, 0), new Size(((Control)sender).Width + 1, ((Control)sender).Height + 1));
            CreateGraphics().DrawRectangle(new Pen(Color.Black, Style.FormBorder), borderRectangle);
        }

        private void OnControlAdd(object sender, ControlEventArgs e)
        {
            if (!e.Control.Name.Equals("BG"))
            {
                if (e.Control.Width > m_LargestPanelWidth)
                {
                    m_LargestPanelWidth = e.Control.Width;
                }

                foreach (Control control in Controls)
                {
                    control.Width = m_LargestPanelWidth;
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
            TitlePanel = new TitlePanel(this)
            {
                Visible = true,
                Location = CustomControlHelper.SetVerticalLocation(this)
            };
            Controls.Add(TitlePanel);

            EurTryCurrencyPanel = new CurrencyPanel(new EurTryCurrency(CurrencyUrls.YahooEurTry), this)
            {
                Visible = true,
                Location = CustomControlHelper.SetVerticalLocation(this)
            };
            Controls.Add(EurTryCurrencyPanel);

            UsdTryCurrencyPanel = new CurrencyPanel(new UsdTryCurrency(CurrencyUrls.YahooUsdTry), this)
            {
                Visible = true,
                Location = CustomControlHelper.SetVerticalLocation(this)
            };
            Controls.Add(UsdTryCurrencyPanel);

            EurUsdCurrencyPanel = new CurrencyPanel(new EurUsdCurrency(CurrencyUrls.YahooEurUsd), this)
            {
                Visible = true,
                Location = CustomControlHelper.SetVerticalLocation(this)
            };
            Controls.Add(EurUsdCurrencyPanel);
        }

        private void InitializeContainerPanel()
        {
            m_ContainerPanel.Name = "BG";
            m_ContainerPanel.Location = new Point(1, 1);
            m_ContainerPanel.BackColor = Color.WhiteSmoke;
            m_ContainerPanel.SendToBack();
            m_ContainerPanel.Paint += PaintBorder;

            Controls.Add(m_ContainerPanel);
        }

        private void SetContainerPanelDimensions()
        {
            m_ContainerPanel.Width = 0;
            m_ContainerPanel.Height = 0;

            foreach (Control panelControl in Controls)
            {
                if (!panelControl.Name.Equals("BG"))
                {
                    if (panelControl.Width > m_ContainerPanel.Width)
                    {
                        m_ContainerPanel.Width = panelControl.Width;
                    }

                    m_ContainerPanel.Height += panelControl.Height + Style.PanelSpacing;
                }
            }

            m_ContainerPanel.Width += Style.FormBorder * 2;
            m_ContainerPanel.Height += Style.FormBorder;
        }

        private void SetMainFormDimensions()
        {
            Width = m_ContainerPanel.Width + Style.FormBorder * 2;
            Height = m_ContainerPanel.Height + Style.FormBorder * 2;
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
