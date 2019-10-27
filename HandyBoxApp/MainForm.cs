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

        private MainContainerPanel ContainerPanel { get; set; }

        private CurrencyPanel EuroCurrencyPanel { get; set; }

        private TitlePanel TitlePanel { get; set; }

        #endregion

        //################################################################################
        #region Event Handlers

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            SetContainerPanelDimensions();
            SetContainerPanelLocation();
            SetFormDimensions();

            Paint += PaintBorder;
        }

        #endregion

        //################################################################################
        #region Private Members

        private void InitializePanels()
        {
            ContainerPanel = new MainContainerPanel(this);
            ContainerPanel.SendToBack();
            ContainerPanel.Visible = true;

            TitlePanel = new TitlePanel(ContainerPanel);
            TitlePanel.Visible = true;
            TitlePanel.Location = CustomControlHelper.SetLocation(ContainerPanel);
            ContainerPanel.Controls.Add(TitlePanel);

            EuroCurrencyPanel = new CurrencyPanel(new EurTryCurrency(CurrencyUrls.YahooEurTry), ContainerPanel);
            EuroCurrencyPanel.Visible = true;
            EuroCurrencyPanel.Location = CustomControlHelper.SetLocation(ContainerPanel);
            ContainerPanel.Controls.Add(EuroCurrencyPanel);


            Controls.Add(ContainerPanel);
        }

        private void SetContainerPanelDimensions()
        {
            var width = 0;
            var height = 0;

            foreach (Control panelControl in ContainerPanel.Controls)
            {
                if (panelControl.Width > width)
                {
                    width = panelControl.Width;
                }

                height += panelControl.Height;
            }

            ContainerPanel.Width = width;
            ContainerPanel.Height = height;
        }

        private void SetContainerPanelLocation()
        {
            int x = Style.FormBorder;
            ContainerPanel.Location = new Point(x, x);
        }

        private void SetFormDimensions()
        {
            Height = ContainerPanel.Height + Style.FormBorder * 2;

            //todo: additional value (100) will be calculated according to most function owner panel
            Width = ContainerPanel.Width + 100;
        }

        private void PaintBorder(object sender, PaintEventArgs e)
        {
            Rectangle borderRectangle = new Rectangle(new Point(0, 0), new Size(Width, Height));
            CreateGraphics().DrawRectangle(new Pen(Color.White, Style.FormBorder), borderRectangle);
        }

        #endregion
    }
}
