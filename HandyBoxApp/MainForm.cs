using HandyBoxApp.CurrencyService;
using HandyBoxApp.CurrencyService.Types;
using HandyBoxApp.CustomComponents.Panels;

using System.Drawing;
using System.Windows.Forms;

namespace HandyBoxApp
{
    public partial class MainForm : Form
    {
        //################################################################################
        #region Fields


        #endregion

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

        #endregion

        //################################################################################
        #region Event Handlers

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            SetContainerPanelDimensions();
            SetFormDimensions();
        }

        #endregion

        //################################################################################
        #region Private Members

        private void InitializePanels()
        {
            ContainerPanel = new MainContainerPanel(this);
            ContainerPanel.SendToBack();
            ContainerPanel.BackColor = Color.Brown;
            ContainerPanel.Visible = true;

            EuroCurrencyPanel = new CurrencyPanel(new EurTryCurrency(CurrencyUrls.YahooEurTry), ContainerPanel);
            EuroCurrencyPanel.AutoSize = true;
            EuroCurrencyPanel.BackColor = Color.Crimson;
            EuroCurrencyPanel.Visible = true;

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

        private void SetFormDimensions()
        {
            Height = ContainerPanel.Height;
            Width = ContainerPanel.Width;
        }

        #endregion
    }
}
