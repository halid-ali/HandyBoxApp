using HandyBoxApp.ColorScheme;
using HandyBoxApp.ColorScheme.Colors;
using HandyBoxApp.CustomComponents;
using HandyBoxApp.Timer;
using HandyBoxApp.Utilities;

using System;
using System.Drawing;
using System.Windows.Forms;

namespace HandyBoxApp.UserControls
{
    public class TimerPanel : UserControl
    {
        private delegate void TimerUpdateCallback(object sender, TimerUpdateEventArgs args);

        private const string Initial = "00:00.00";
        private const string Elapsed = "Elapsed:";
        private const string Remains = "Remains:";
        private const string Stopped = "Stopped:";
        private const string Paused = "Paused:";

        //################################################################################
        #region Constructor

        public TimerPanel(Control parentControl)
        {
            ParentControl = parentControl;

            InitializeComponent();
            OrderControls();
        }

        #endregion

        //################################################################################
        #region Properties

        private Control ParentControl { get; }

        private Label FunctionText { get; } = new Label();

        private TextBox TimerText { get; } = new TextBox();

        private FlowLayoutPanel TimerContainer { get; } = new FlowLayoutPanel();

        private ImageButton FunctionButton { get; set; }

        private FlowLayoutPanel ContainerPanel { get; } = new FlowLayoutPanel();

        private Timer.Timer WorkTimer { get; set; }

        private TimerMode Mode { get; set; } = TimerMode.Elapsed;

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

            #region Timer TextBox

            FunctionText.Name = "TimerText";
            FunctionText.Text = Formatter.FormatString(Stopped, Pad.Right, 9);
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

            TimerText.Text = Initial;
            TimerText.ReadOnly = true;
            TimerText.AutoSize = false;
            TimerText.TabStop = false;
            TimerText.Margin = new Padding(0);
            TimerText.BorderStyle = BorderStyle.None;
            TimerText.Size = hourTextGuideLabel.PreferredSize;
            TimerText.TextAlign = HorizontalAlignment.Center;
            TimerText.Font = new Font(new FontFamily(Style.FontName), Style.PanelFontSize + 2, FontStyle.Bold);
            Painter<Blue>.Paint(TimerText, PaintMode.Normal);
            TimerText.DoubleClick += TimerText_DoubleClick;
            TimerText.KeyPress += TimerText_KeyPress;
            TimerText.LostFocus += TimerText_LostFocus;

            TimerContainer.Width = FunctionText.PreferredWidth + Style.PanelSpacing + TimerText.Width;
            TimerContainer.Height = FunctionText.PreferredHeight;
            TimerContainer.Padding = new Padding(0);
            TimerContainer.Margin = new Padding(0, 0, Style.PanelSpacing, 0);
            TimerContainer.BorderStyle = BorderStyle.None;
            TimerContainer.FlowDirection = FlowDirection.LeftToRight;
            TimerContainer.Controls.Add(FunctionText);
            TimerContainer.Controls.Add(TimerText);

            #endregion

            #region Function Button

            void SlideAction(Control button)
            {
                button.Click += (sender, args) =>
                {

                };
            }

            var functionButtonText = 
            FunctionButton = new ImageButton(SlideAction, ">") { Margin = new Padding(0) };
            FunctionButton.SetToolTip("Open panel");
            FunctionButton.SetColor<Red>(PaintMode.Dark);

            #endregion

            #region Timer Panel

            Name = "TimerPanel";
            AutoSize = true;
            Margin = new Padding(0, 0, 0, Style.PanelSpacing);
            BackColor = Color.FromArgb(100, 100, 100);
            BorderStyle = BorderStyle.FixedSingle;
            AutoScaleMode = AutoScaleMode.Font;

            #endregion

            ContainerPanel.Controls.Add(TimerContainer);
            ContainerPanel.Controls.Add(FunctionButton);
            Controls.Add(ContainerPanel);

            ContainerPanel.ResumeLayout(false);
            ContainerPanel.PerformLayout();
            ResumeLayout(false);
        }

        private void UpdateTimer(object sender, TimerUpdateEventArgs args)
        {
            if (FunctionText.InvokeRequired)
            {
                TimerUpdateCallback callback = UpdateTimer;
                Invoke(callback, this, args);
            }
            else
            {
                if (Mode == TimerMode.Elapsed)
                {
                    TimerText.Text = $"{string.Format("{0:D2}", args.ElapsedTime.Hours)}:" +
                                     $"{string.Format("{0:D2}", args.ElapsedTime.Minutes)}." +
                                     $"{string.Format("{0:D2}", args.ElapsedTime.Seconds)}";
                }
                else
                {
                    TimerText.Text = $"{string.Format("{0:D2}", args.RemainingTime.Hours)}:" +
                                     $"{string.Format("{0:D2}", args.RemainingTime.Minutes)}." +
                                     $"{string.Format("{0:D2}", args.RemainingTime.Seconds)}";
                }
            }
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

        private bool VerifyTimerText(string timerValue, out DateTime startTime)
        {
            startTime = DateTime.MinValue;

            //verify digit count
            if (timerValue.Length != 6)
            {
                return false;
            }

            //verify first digit couple that they represent a valid hour
            int hourValue;
            if (!int.TryParse(timerValue.Substring(0, 2), out hourValue))
            {
                return false;
            }

            //verify second digit couple that they represent a valid minute
            int minuteValue;
            if (!int.TryParse(timerValue.Substring(2, 2), out minuteValue))
            {
                return false;
            }

            //verify third digit couple that they represent a valid second
            int secondValue;
            if (!int.TryParse(timerValue.Substring(4, 2), out secondValue))
            {
                return false;
            }

            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;
            var day = DateTime.Now.Day;

            startTime = new DateTime(year, month, day, hourValue, minuteValue, secondValue);

            return true;
        }

        #endregion

        //################################################################################
        #region Event Handlers

        private void TimerText_DoubleClick(object sender, EventArgs e)
        {
            TimerText.ReadOnly = false;
        }

        private void TimerText_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

            DateTime startTime;

            if (e.KeyChar == (char)13)
            {
                if (VerifyTimerText(TimerText.Text, out startTime))
                {
                    TimerText.ReadOnly = true;
                    FunctionText.Text = Formatter.FormatString(Elapsed, Pad.Right, 9);
                    Painter<Blue>.Paint(TimerText, PaintMode.Light);

                    WorkTimer = new Timer.Timer(startTime);
                    WorkTimer.TimerUpdated += UpdateTimer;
                    WorkTimer.Start();
                }
                else
                {
                    TimerText.Text = Initial;
                }
            }
        }

        private void TimerText_LostFocus(object sender, EventArgs e)
        {
            TimerText.ReadOnly = true;
        }

        private void FunctionText_DoubleClick(object sender, EventArgs e)
        {
            if (WorkTimer.IsStarted)
            {
                string modeText;

                if (Mode == TimerMode.Elapsed)
                {
                    Mode = TimerMode.Remaining;
                    modeText = Remains;
                }
                else
                {
                    Mode = TimerMode.Elapsed;
                    modeText = Elapsed;
                }

                FunctionText.Text = Formatter.FormatString(modeText, Pad.Right, 9);
            }
        }

        #endregion
    }
}
