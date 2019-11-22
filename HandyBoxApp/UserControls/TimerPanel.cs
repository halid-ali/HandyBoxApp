using HandyBoxApp.ColorScheme;
using HandyBoxApp.ColorScheme.Colors;
using HandyBoxApp.CustomComponents;
using HandyBoxApp.Utilities;
using HandyBoxApp.WorkTimer;

using System;
using System.Drawing;
using System.Windows.Forms;

using Timer = HandyBoxApp.WorkTimer.Timer;

namespace HandyBoxApp.UserControls
{
    public class TimerPanel : UserControl
    {
        private delegate void TimerUpdateCallback(object sender, TimerUpdateEventArgs args);

        //################################################################################
        #region Constants

        private const string Initial = "00:00.00";
        private const string Elapsed = "Elapsed:";
        private const string Remains = "Remains:";
        private const string Stopped = "Stopped:";
        private const string Paused = "Paused:";
        private const string Overtime = "Overtime:";

        #endregion

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

        private Timer WorkTimer { get; set; }

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

            void PauseAction(Control button)
            {
                button.Click += (sender, args) =>
                {
                    if (WorkTimer != null)
                    {
                        if (WorkTimer.IsStarted)
                        {
                            WorkTimer.Pause();
                            FunctionButton.SetText("R");
                        }
                        else if (WorkTimer.IsPaused)
                        {
                            WorkTimer.Start();
                            FunctionButton.SetText("P");
                        }

                        TimerText.HideSelection = true;
                        button.Focus();
                    }
                };
            }

            FunctionButton = new ImageButton(PauseAction, "S") { Margin = new Padding(0) };
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
                if (args.Overtime.Ticks < 0)
                {
                    TimerText.Text = Formatter.FormatHour(Mode == TimerMode.Elapsed ? args.ElapsedTime : args.RemainingTime);
                }
                else
                {
                    //when overtime reaches to 2 hours, stop timer
                    if (args.Overtime.Hours >= 2)
                    {
                        StopTimer();
                        return;
                    }

                    //set mode and adjust timer values only once
                    if (Mode != TimerMode.Overtime)
                    {
                        Mode = TimerMode.Overtime;
                        Painter<Green>.Paint(TimerText, PaintMode.Dark);
                        FunctionText.Text = Formatter.FormatString(Overtime, Pad.Right, 9);
                    }

                    //change color of TimerText if not changed
                    if (args.Overtime > TimeSpan.FromMinutes(90))
                    {
                        Painter<Red>.Paint(TimerText, PaintMode.Dark);
                    }

                    //display reminder after 90 minutes of overtime for every defined time slot
                    if (args.Overtime.Minutes % Constants.TimerReminderInterval == 0 &&
                        args.Overtime.Seconds == 0)
                    {
                        var message = $"Last {60 - args.Overtime.Minutes} minutes for leaving the office.";
                        BalloonTip.Show("Work Hour Deadline", message, ToolTipIcon.Info, 2000);
                    }

                    TimerText.Text = Formatter.FormatHour(args.Overtime);
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
            if (timerValue.Length != 6) return false;

            //verify first digit couple that they represent a valid hour
            if (!int.TryParse(timerValue.Substring(0, 2), out int hourValue)) return false;
            if (hourValue < 0 || hourValue > 23) return false;

            //verify second digit couple that they represent a valid minute
            if (!int.TryParse(timerValue.Substring(2, 2), out int minuteValue)) return false;
            if (minuteValue < 0 || minuteValue > 59) return false;

            //verify third digit couple that they represent a valid second
            if (!int.TryParse(timerValue.Substring(4, 2), out int secondValue)) return false;
            if (secondValue < 0 || secondValue > 59) return false;

            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;
            var day = DateTime.Now.Day;

            startTime = new DateTime(year, month, day, hourValue, minuteValue, secondValue);

            return true;
        }

        private void StartTimer(DateTime startTime)
        {
            //adjust timer text
            TimerText.ReadOnly = true;
            Painter<Blue>.Paint(TimerText, PaintMode.Light);

            //adjust function text
            FunctionText.Text = Formatter.FormatString(Elapsed, Pad.Right, 9);

            //adjust function button
            FunctionButton.SetText("P");
            Painter<Red>.Paint(FunctionButton, PaintMode.Normal);

#if false
            //startTime = DateTime.Now.Subtract(new TimeSpan(8, 44, 57)); //3 seconds before overtime
            //startTime = DateTime.Now.Subtract(new TimeSpan(9, 14, 57)); //3 seconds before between overtime and reminder
            //startTime = DateTime.Now.Subtract(new TimeSpan(10, 14, 57)); //3 seconds before reminder
            //startTime = DateTime.Now.Subtract(new TimeSpan(10, 24, 57)); //3 seconds before between reminder and deadline
            //startTime = DateTime.Now.Subtract(new TimeSpan(10, 44, 57)); //3 seconds before deadline
#endif

            WorkTimer = new Timer(startTime);
            WorkTimer.TimerUpdated += UpdateTimer;
            WorkTimer.Start();
        }

        private void StopTimer()
        {
            //adjust timer text
            TimerText.Text = Initial;
            TimerText.ReadOnly = true;
            Painter<Blue>.Paint(TimerText, PaintMode.Normal);

            //adjust function text
            FunctionText.Text = Formatter.FormatString(Stopped, Pad.Right, 9);

            //adjust function button
            FunctionButton.SetText("S");
            Painter<Red>.Paint(FunctionButton, PaintMode.Dark);

            //adjust mode
            Mode = TimerMode.Elapsed;

            WorkTimer.Stop();
            WorkTimer.TimerUpdated -= UpdateTimer;
            WorkTimer.Dispose();
            WorkTimer = null;
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
            var timeText = TimerText.Text;
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

            if (e.KeyChar == (char)13)
            {
                if (WorkTimer != null && (WorkTimer.IsStarted || WorkTimer.IsPaused))
                {
                    StopTimer();
                }

                if (VerifyTimerText(timeText, out DateTime startTime))
                {
                    StartTimer(startTime);
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
            if (WorkTimer != null && WorkTimer.IsStarted)
            {
                string modeText;

                if (Mode == TimerMode.Elapsed)
                {
                    Mode = TimerMode.Remaining;
                    modeText = Remains;
                }
                else if (Mode == TimerMode.Remaining)
                {
                    Mode = TimerMode.Elapsed;
                    modeText = Elapsed;
                }
                else
                {
                    Mode = TimerMode.Overtime;
                    modeText = Overtime;
                }

                FunctionText.Text = Formatter.FormatString(modeText, Pad.Right, 9);
            }
        }

        #endregion
    }
}
