using HandyBoxApp.ColorScheme;
using HandyBoxApp.ColorScheme.Colors;
using HandyBoxApp.CustomComponents;
using HandyBoxApp.Utilities;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace HandyBoxApp.UserControls
{
    internal class HourUpdateEventArgs : EventArgs
    {
        internal TimeSpan ElapsedTime { get; set; }
    }

    public class HourPanel : UserControl
    {
        private delegate void HourUpdateCallback(object sender, HourUpdateEventArgs args);

        //################################################################################
        #region Constructor

        public HourPanel(Control parentControl)
        {
            ParentControl = parentControl;

            Worker.DoWork += TickTockHour;
            Worker.RunWorkerCompleted += TickTockHourCompleted;

            InitializeComponent();
            OrderControls();

            StartHourTicking();
        }

        #endregion

        //################################################################################
        #region Properties

        private Control ParentControl { get; }

        private ImageButton QuickSwitchButton { get; set; }

        private Label ValueLabel { get; } = new Label();

        private ImageButton FunctionButton { get; set; }

        private FlowLayoutPanel ContainerPanel { get; } = new FlowLayoutPanel();

        private BackgroundWorker Worker { get; } = new BackgroundWorker();

        private bool IsTickCancelled { get; set; }

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

            #region Quick Switch Button

            void SwitchAction(Control button)
            {
                button.Click += (sender, args) =>
                {

                };
            }

            QuickSwitchButton = new ImageButton(SwitchAction, "S") { Margin = new Padding(0, 0, Style.PanelSpacing, 0) };
            QuickSwitchButton.SetToolTip("Switch hour");
            QuickSwitchButton.SetColor<Red>(PaintMode.Dark);

            #endregion

            #region Value Label

            ValueLabel.Name = "ValueLabel";
            ValueLabel.Text = SetHourText("00:00.00");
            ValueLabel.AutoSize = true;
            ValueLabel.Margin = new Padding(0, 0, Style.PanelSpacing, 0);
            ValueLabel.Padding = new Padding(Style.PanelPadding);
            ValueLabel.TextAlign = ContentAlignment.MiddleCenter;
            ValueLabel.Font = new Font(new FontFamily(Style.FontName), Style.PanelFontSize, FontStyle.Bold);
            Painter<Black>.Paint(ValueLabel, PaintMode.Normal);

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

            #region Hour Panel

            Name = $"HourPanel";
            AutoSize = true;
            Margin = new Padding(0, 0, 0, Style.PanelSpacing);
            BackColor = Color.FromArgb(100, 100, 100);
            BorderStyle = BorderStyle.FixedSingle;
            AutoScaleMode = AutoScaleMode.Font;

            #endregion

            ContainerPanel.Controls.Add(QuickSwitchButton);
            ContainerPanel.Controls.Add(ValueLabel);
            ContainerPanel.Controls.Add(FunctionButton);
            Controls.Add(ContainerPanel);

            ContainerPanel.ResumeLayout(false);
            ContainerPanel.PerformLayout();
            ResumeLayout(false);
        }

        private void TickTockHour(object sender, DoWorkEventArgs args)
        {
            
        }

        private void TickTockHourCompleted(object sender, RunWorkerCompletedEventArgs args)
        {

        }

        private void UpdateStockData(object sender, HourUpdateEventArgs args)
        {
            if (ValueLabel.InvokeRequired)
            {
                HourUpdateCallback callback = UpdateStockData;
                Invoke(callback, this, args);
            }
            else
            {
                
            }
        }

        private void StartHourTicking()
        {
            Worker.RunWorkerAsync();
        }

        private void StopHourTicking()
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

        private string SetHourText(string value)
        {
            var paddingWidth = (20 - value.Length) / 2;
            value = Formatter.FormatString(value, Pad.Left, value.Length + paddingWidth);
            value = Formatter.FormatString(value, Pad.Right, value.Length + paddingWidth);

            return value;
        }

        #endregion

        //################################################################################
        #region Event Handlers



        #endregion
    }
}
