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

            EuroCurrencyPanel = new CurrencyPanel(null, ContainerPanel);
            EuroCurrencyPanel.BackColor = Color.Crimson;
            EuroCurrencyPanel.Visible = true;

            ContainerPanel.Controls.Add(EuroCurrencyPanel);

            Controls.Add(ContainerPanel);
        }

        private void SetContainerPanelDimensions()
        {
            foreach (Control panelControl in ContainerPanel.Controls)
            {

            }
        }

        private void SetFormDimensions()
        {
            Height = ContainerPanel.Height;
            Width = ContainerPanel.Width;
        }

        #endregion
    }
}
