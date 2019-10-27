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
        #region Constructor

        public MainForm()
        {
            InitializeComponent();
            InitializePanels();
        }

        #endregion

        //################################################################################
        #region Properties

        private CurrencyPanel EuroCurrencyPanel { get; set; }

        private TitlePanel TitlePanel { get; set; }

        #endregion

        //################################################################################
        #region Event Handlers

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            SetFormDimensions();

            Paint += PaintBorder;
        }

        #endregion

        //################################################################################
        #region Private Members

        private void InitializePanels()
        {
            TitlePanel = new TitlePanel(this);
            TitlePanel.Visible = true;
            TitlePanel.Location = CustomControlHelper.SetLocation(this);
            Controls.Add(TitlePanel);

            EuroCurrencyPanel = new CurrencyPanel(new EurTryCurrency(CurrencyUrls.YahooEurTry), this);
            EuroCurrencyPanel.Visible = true;
            EuroCurrencyPanel.Location = CustomControlHelper.SetLocation(this);
            Controls.Add(EuroCurrencyPanel);
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

            Width = width;
            Height = height;
        }

        private void PaintBorder(object sender, PaintEventArgs e)
        {
            Rectangle borderRectangle = new Rectangle(new Point(0, 0), new Size(Width, Height));
            CreateGraphics().DrawRectangle(new Pen(Color.White, Style.FormBorder), borderRectangle);
        }

        #endregion
    }
}
