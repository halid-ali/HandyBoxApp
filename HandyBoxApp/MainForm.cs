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

            EurUsdCurrencyPanel = new CurrencyPanel(new EurUsdCurrency(CurrencyUrls.YahooEurUsd), this)
            {
                Visible = true,
                Location = CustomControlHelper.SetVerticalLocation(this)
            };
            Controls.Add(EurUsdCurrencyPanel);

            UsdTryCurrencyPanel = new CurrencyPanel(new UsdTryCurrency(CurrencyUrls.YahooUsdTry), this)
            {
                Visible = true,
                Location = CustomControlHelper.SetVerticalLocation(this)
            };
            Controls.Add(UsdTryCurrencyPanel);
        }

        private void SetFormDimensions()
        {
            var width = 0;
            var height = 0;

            foreach (Control panelControl in Controls)
            {
                if (panelControl.Width > width)
                {
                    width = panelControl.Width;
                }

                height += panelControl.Height;
            }

            width += Style.FormBorder * 2;
            height += Style.PanelSpacing + Style.FormBorder * 2;

            Width = width;
            Height = height;
        }

        private void PaintBorder(object sender, PaintEventArgs e)
        {
            Rectangle borderRectangle = new Rectangle(new Point(0, 0), new Size(Width, Height));
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
    }
}
