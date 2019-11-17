using HandyBoxApp.ColorScheme;
using HandyBoxApp.ColorScheme.Colors;
using HandyBoxApp.CustomComponents;

using System.Drawing;
using System.Windows.Forms;

namespace HandyBoxApp.UserControls
{
    public class StockPanel : UserControl
    {
        //################################################################################
        #region Constructor

        public StockPanel(Control parentControl)
        {
            ParentControl = parentControl;

            InitializeComponent();
            OrderControls();
        }

        #endregion

        //################################################################################
        #region Properties

        private Control ParentControl { get; }

        private Label NameLabel { get; } = new Label();

        private Label ValueLabel { get; } = new Label();

        private ImageButton FunctionButton { get; set; }

        private FlowLayoutPanel ContainerPanel { get; } = new FlowLayoutPanel();

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
            NameLabel.Text = "EUR/TRY";
            NameLabel.Width = 90;
            NameLabel.Margin = new Padding(0, 0, Style.PanelSpacing, 0);
            NameLabel.Padding = new Padding(Style.PanelPadding);
            NameLabel.TextAlign = ContentAlignment.MiddleLeft;
            NameLabel.Font = new Font(new FontFamily(Style.FontName), Style.PanelFontSize, FontStyle.Bold);
            Painter<Black>.Paint(NameLabel, PaintMode.Normal);

            #endregion

            #region Value Label

            ValueLabel.Name = "ValueLabel";
            ValueLabel.Text = "6.4167 TL";
            ValueLabel.Width = 100;
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

            #region Currency Panel

            Name = "CurrencyPanel";
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

                ContainerPanel.Width += control.Width - Style.PanelSpacing;
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
