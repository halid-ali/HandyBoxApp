using HandyBoxApp.ColorScheme;
using HandyBoxApp.ColorScheme.Colors;
using HandyBoxApp.CustomComponents;
using HandyBoxApp.Utilities;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace HandyBoxApp.UserControls
{
    public class HourPanel : UserControl
    {
        private delegate void HourUpdateCallback(object sender, HourUpdateEventArgs args);

        //################################################################################
        #region Constructor

        public HourPanel(Control parentControl)
        {
            ParentControl = parentControl;
            DoubleClick += HourPanel_DoubleClick;

            Worker.DoWork += HourTicking;
            Worker.RunWorkerCompleted += HourTickingCompleted;

            InitializeComponent();
            OrderControls();

            StartHourTicking();
        }

        #endregion

        //################################################################################
        #region Properties

        private Control ParentControl { get; }

        private ImageButton QuickSwitchButton { get; set; }

        private Label FunctionText { get; } = new Label();

        private TextBox HourText { get; } = new TextBox();

        private FlowLayoutPanel HourContainer { get; } = new FlowLayoutPanel();

        private ImageButton FunctionButton { get; set; }

        private FlowLayoutPanel ContainerPanel { get; } = new FlowLayoutPanel();

        private BackgroundWorker Worker { get; } = new BackgroundWorker();

        private bool IsTicking { get; set; }

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

            #region Hour TextBox

            FunctionText.Name = "HourText";
            FunctionText.Text = Formatter.FormatString("Stopped:", Pad.Right, 9);
            FunctionText.AutoSize = true;
            FunctionText.BorderStyle = BorderStyle.None;
            FunctionText.Padding = new Padding(Style.PanelPadding);
            FunctionText.Margin = new Padding(0, 0, Style.PanelSpacing, 0);
            FunctionText.Font = new Font(new FontFamily(Style.FontName), Style.PanelFontSize, FontStyle.Bold);
            Painter<Black>.Paint(FunctionText, PaintMode.Normal);
            FunctionText.DoubleClick += FunctionText_DoubleClick;

            var hourTextGuideLabel = new Label
            {
                AutoSize = true,
                Text = new string(' ', 12),
                Margin = new Padding(0),
                Padding = new Padding(Style.PanelPadding),
                Font = new Font(new FontFamily(Style.FontName), Style.PanelFontSize, FontStyle.Bold)
            };

            HourText.Text = "00:00.00";
            HourText.ReadOnly = true;
            HourText.AutoSize = false;
            HourText.Margin = new Padding(0);
            HourText.BorderStyle = BorderStyle.None;
            HourText.Size = hourTextGuideLabel.PreferredSize;
            HourText.TextAlign = HorizontalAlignment.Center;
            HourText.Font = new Font(new FontFamily(Style.FontName), Style.PanelFontSize + 2, FontStyle.Bold);
            Painter<Blue>.Paint(HourText, PaintMode.Normal);

            HourContainer.Width = FunctionText.PreferredWidth + Style.PanelSpacing + HourText.Width;
            HourContainer.Height = FunctionText.PreferredHeight;
            HourContainer.Padding = new Padding(0);
            HourContainer.Margin = new Padding(0, 0, Style.PanelSpacing, 0);
            HourContainer.BorderStyle = BorderStyle.None;
            HourContainer.FlowDirection = FlowDirection.LeftToRight;
            HourContainer.Controls.Add(FunctionText);
            HourContainer.Controls.Add(HourText);

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

            ContainerPanel.Controls.Add(HourContainer);
            ContainerPanel.Controls.Add(FunctionButton);
            Controls.Add(ContainerPanel);

            ContainerPanel.ResumeLayout(false);
            ContainerPanel.PerformLayout();
            ResumeLayout(false);
        }

        private void HourTicking(object sender, DoWorkEventArgs args)
        {
            IsTicking = true;
        }

        private void HourTickingCompleted(object sender, RunWorkerCompletedEventArgs args)
        {
            IsTicking = false;
        }

        private void UpdateHour(object sender, HourUpdateEventArgs args)
        {
            if (FunctionText.InvokeRequired)
            {
                HourUpdateCallback callback = UpdateHour;
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

                ContainerPanel.Width += control.Width - Style.PanelSpacing;
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

        private void HourPanel_DoubleClick(object sender, EventArgs e)
        {

        }

        private void FunctionText_DoubleClick(object sender, EventArgs e)
        {
            if (IsTicking)
            {
                var text = FunctionText.Text.Trim().Equals("Remains:") ? "Elapsed:" : "Remains:";
                FunctionText.Text = Formatter.FormatString(text, Pad.Right, 9);
            }
        }

        #endregion
    }
}
