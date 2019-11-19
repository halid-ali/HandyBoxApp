using HandyBoxApp.ColorScheme;
using HandyBoxApp.ColorScheme.Colors;
using HandyBoxApp.CustomComponents;
using HandyBoxApp.StockExchange.EventArgs;
using HandyBoxApp.StockExchange.Interfaces;
using HandyBoxApp.StockExchange.Stock;
using HandyBoxApp.Utilities;

using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace HandyBoxApp.UserControls
{
    public class StockPanel : UserControl
    {
        private delegate void StockUpdateCallback(object sender, StockUpdateEventArgs args);

        //################################################################################
        #region Constructor

        public StockPanel(Control parentControl, IStockService stockService, int refreshRate = Constants.DefaultRefreshRate)
        {
            ParentControl = parentControl;
            StockService = stockService;
            RefreshRate = refreshRate;

            Worker.DoWork += FetchStockData;
            Worker.RunWorkerCompleted += FetchStockDataCompleted;

            InitializeComponent();
            OrderControls();

            StartStockDataFetching();
        }

        #endregion

        //################################################################################
        #region Properties

        private Control ParentControl { get; }

        private IStockService StockService { get; }

        private Label NameLabel { get; } = new Label();

        private Label ValueLabel { get; } = new Label();

        private ImageButton FunctionButton { get; set; }

        private FlowLayoutPanel ContainerPanel { get; } = new FlowLayoutPanel();

        private BackgroundWorker Worker { get; } = new BackgroundWorker();

        private bool IsFetchCancelled { get; set; }

        private StockData PreviousStockData { get; set; }

        private int RefreshRate { get; }

        private ToolTip ToolTip { get; } = new ToolTip();

        #endregion

        //################################################################################
        #region Private Members

        private void InitializeComponent()
        {
            ContainerPanel.SuspendLayout();
            SuspendLayout();

            #region Container Panel

            ContainerPanel.Margin = new Padding(0);
            ContainerPanel.Padding = new Padding(0);
            ContainerPanel.Location = new Point(0, 0);
            ContainerPanel.FlowDirection = FlowDirection.LeftToRight;

            #endregion

            #region Name Label

            NameLabel.Name = "NameLabel";
            NameLabel.Text = Formatter.FormatString(StockService.GetStockInfo.Name, Pad.Right, 9);
            //NameLabel.Width = 90;
            NameLabel.AutoSize = true;
            NameLabel.Margin = new Padding(0, 0, Style.PanelSpacing, 0);
            NameLabel.Padding = new Padding(Style.PanelPadding);
            NameLabel.TextAlign = ContentAlignment.MiddleLeft;
            NameLabel.Font = new Font(new FontFamily(Style.FontName), Style.PanelFontSize, FontStyle.Bold);
            Painter<Black>.Paint(NameLabel, PaintMode.Normal);

            #endregion

            #region Value Label

            ValueLabel.Name = "ValueLabel";
            ValueLabel.Text = Formatter.FormatString("#,#### TL", Pad.Left, 12);
            //ValueLabel.Width = 100;
            ValueLabel.AutoSize = true;
            ValueLabel.Margin = new Padding(0, 0, Style.PanelSpacing, 0);
            ValueLabel.Padding = new Padding(Style.PanelPadding);
            ValueLabel.TextAlign = ContentAlignment.MiddleRight;
            ValueLabel.Font = new Font(new FontFamily(Style.FontName), Style.PanelFontSize, FontStyle.Bold);
            Painter<Green>.Paint(ValueLabel, PaintMode.Light);

            #endregion

            #region Function Button

            void SlideAction(Control button)
            {
                button.Click += (sender, args) =>
                {

                };
            }

            FunctionButton = new ImageButton(SlideAction, ">") { Margin = new Padding(0) };
            FunctionButton.SetToolTip("Open panel");
            FunctionButton.SetColor<Red>(PaintMode.Dark);

            #endregion

            #region Stock Panel

            Name = $"StockPanel_{StockService.GetStockInfo.Tag}";
            AutoSize = true;
            Margin = new Padding(0, 0, 0, Style.PanelSpacing);
            BackColor = Color.FromArgb(100, 100, 100);
            BorderStyle = BorderStyle.FixedSingle;
            AutoScaleMode = AutoScaleMode.Font;

            #endregion

            ContainerPanel.Controls.Add(NameLabel);
            ContainerPanel.Controls.Add(ValueLabel);
            ContainerPanel.Controls.Add(FunctionButton);
            Controls.Add(ContainerPanel);

            ContainerPanel.ResumeLayout(false);
            ContainerPanel.PerformLayout();
            ResumeLayout(false);
        }

        private void FetchStockData(object sender, DoWorkEventArgs args)
        {
            IsFetchCancelled = true;
            StockService.StockUpdated += UpdateStockData;

            try
            {
                while (IsFetchCancelled)
                {
                    StockService.GetStockData();
                    Thread.Sleep(RefreshRate);
                }
            }
            finally
            {
                StockService.StockUpdated -= UpdateStockData;
            }
        }

        private void FetchStockDataCompleted(object sender, RunWorkerCompletedEventArgs args)
        {

        }

        private void UpdateStockData(object sender, StockUpdateEventArgs args)
        {
            if (ValueLabel.InvokeRequired)
            {
                StockUpdateCallback callback = UpdateStockData;
                Invoke(callback, this, args);
            }
            else
            {
                var stockData = args.StockData;

                if (stockData.ActualData > PreviousStockData.ActualData)
                {
                    Painter<Green>.Paint(ValueLabel, PaintMode.Light);
                }
                else if (stockData.ActualData < PreviousStockData.ActualData)
                {
                    Painter<Red>.Paint(ValueLabel, PaintMode.Light);
                }

                ValueLabel.Text = Formatter.FormatDouble(args.StockData.ActualData, Pad.Left, 12);
                ToolTip.SetToolTip(ValueLabel, $"Change Rate: %{args.StockData.ChangeRate}");
                PreviousStockData = stockData;
            }
        }

        private void StartStockDataFetching()
        {
            Worker.RunWorkerAsync();
        }

        private void StopStockDataFetching()
        {

        }

        private void OrderControls()
        {
            ContainerPanel.Width = ContainerPanel.Controls.Count * 2 - 1;
            ContainerPanel.Height = 0;

            foreach (Control control in ContainerPanel.Controls)
            {
                if (control.PreferredSize.Height > ContainerPanel.Height)
                {
                    ContainerPanel.Height = control.PreferredSize.Height;
                }

                //ContainerPanel.Width += control.Width - Style.PanelSpacing;
                ContainerPanel.Width += control.PreferredSize.Width - Style.PanelSpacing;
            }

            Height = ContainerPanel.Height;
            Width = ContainerPanel.Width;
        }

        #endregion

        //################################################################################
        #region Event Handlers



        #endregion
    }
}
