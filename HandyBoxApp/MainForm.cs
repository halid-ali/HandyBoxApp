using HandyBoxApp.CurrencyService;
using HandyBoxApp.CurrencyService.Types;
using HandyBoxApp.CustomComponents;
using HandyBoxApp.CustomComponents.Panels;
using HandyBoxApp.Utilities;

using System.Drawing;
using System.Windows.Forms;

namespace HandyBoxApp
{
    public partial class MainForm : Form
    {
        //################################################################################
        #region Fields

        private int m_LargestPanelWidth;

        #endregion

        //################################################################################
        #region Constructor

        public MainForm()
        {
            Paint += PaintBorder;
            ControlAdded += OnControlAdd;

            InitializeComponent();
            InitializePanels();
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
            SetFormDimensions();
            SetFormPosition();
        }

        private void PaintBorder(object sender, PaintEventArgs e)
        {
            Rectangle borderRectangle = new Rectangle(new Point(0, 0), new Size(Width - 1, Height - 1));
            CreateGraphics().DrawRectangle(new Pen(Color.White, Style.FormBorder), borderRectangle);
        }

        private void OnControlAdd(object sender, ControlEventArgs e)
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

        private void SetFormDimensions()
        {
            Width = 0;
            Height = 0;

            foreach (Control panelControl in Controls)
            {
                if (panelControl.Width > Width)
                {
                    Width = panelControl.Width;
                }

                Height += panelControl.Height + Style.PanelSpacing;
            }

            Width += Style.FormBorder * 2 + Style.PanelSpacing * 2;
            Height += Style.FormBorder;
        }

        private void SetFormPosition()
        {
//#if DEBUG == false
            var marginRight = 30;
            var marginBottom = 60;
            var screenW = Screen.PrimaryScreen.Bounds.Width;
            var screenH = Screen.PrimaryScreen.Bounds.Height;

            Top = screenH - Height - marginBottom;
            Left = screenW - Width - marginRight;
//#endif
        }

        #endregion
    }
}
