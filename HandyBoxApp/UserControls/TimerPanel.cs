using HandyBoxApp.ColorScheme;
using HandyBoxApp.ColorScheme.Colors;
using HandyBoxApp.CustomComponents;
using HandyBoxApp.Logging;
using HandyBoxApp.Properties;
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
        //################################################################################
        #region Delegates

        private delegate void TimerUpdateCallback(object sender, TimerUpdateEventArgs args);
        private delegate void TimerStateChangeCallback(object sender, EventArgs args);

        #endregion

        //################################################################################
        #region Fields

        private const string c_InitialTime = "00:00:00";
        private readonly TimerHelper m_TimerHelper = new TimerHelper();

        #endregion

        //################################################################################
        #region Constructor

        public TimerPanel()
        {
            InitializeComponent();
            OrderControls();

            Log.Info("WorkTimer is loaded.");

            var savedTime = Settings.Default.StartTime;
            var savedMode = Settings.Default.ModeTimer;
            InitializeSettingsAndStartTimer(savedTime, savedMode);
        }

        #endregion

        //################################################################################
        #region Properties

        internal FunctionMode ModeFunction { get; private set; } = FunctionMode.Elapsed;

        internal TimerMode ModeTimer { get; private set; }

        internal TimeSpan ElapsedTime { get; private set; }

        internal TimeSpan RemainingTime { get; private set; }

        internal TimeSpan OverTime { get; private set; } = new TimeSpan(0, 0, 0);

        private Label FunctionText { get; } = new Label();

        private TextBox TimerText { get; } = new TextBox();

        private FlowLayoutPanel TimerContainer { get; } = new FlowLayoutPanel();

        private ImageButton FunctionButton { get; set; }

        private FlowLayoutPanel ContainerPanel { get; } = new FlowLayoutPanel();

        private Timer WorkTimer { get; set; }

        private ILoggingService Log { get; } = LogServiceFactory.CreateService(LogFormat.Txt);

        #endregion

        //################################################################################
        #region Public Members

        public override string ToString()
        {
            return $"{ModeFunction}: {TimerText.Text}";
        }

        #endregion

        //################################################################################
        #region Private Members

        #region Timer Panel Initialization

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

            FunctionText.Name = $"TimerText";
            FunctionText.Text = Formatter.FormatMode($"{TimerMode.Stopped}");
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

            TimerText.Text = c_InitialTime;
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
                button.Click += FunctionButton_Click;
            }

            void ManualStopAction(object sender, EventArgs args)
            {
                if (ModeTimer == TimerMode.Stopped) return;

                Log.Info("Stop has been triggered manually. WorkTimer will stop.");
                StopTimer();
                TimerStopAdjustments();
            }

            FunctionButton = new ImageButton(PauseAction, Resources.Stop)
            {
                Margin = new Padding(0),
                ContextMenu = new ContextMenu { MenuItems = { new MenuItem("Stop", ManualStopAction) } }
            };
            FunctionButton.SetToolTip("Pause/Resume timer");
            FunctionButton.SetBackgroundColor(Color.FromArgb(152, 0, 47));

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

        #region Timer Start/Stop and Initialization

        private void InitializeSettingsAndStartTimer(DateTime startTime, TimerMode timerMode)
        {
            ModeTimer = timerMode;
            ModeFunction = Settings.Default.ModeFunction;

            switch (ModeTimer)
            {
                case TimerMode.Started:
                    Log.Info("WorkTimer in Start mode");
                    StartTimer(m_TimerHelper.GetTestingStartTime(startTime, 0)); //testing purpose
                    break;

                case TimerMode.Paused:
                    Log.Info("WorkTimer in Pause mode");
                    TimerText.Text = Formatter.FormatTimeSpan(Settings.Default.PauseTime);
                    TimerPauseAdjustments();
                    break;

                case TimerMode.Stopped:
                    Log.Info("WorkTimer in Stop mode");
                    TimerStopAdjustments();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void StartTimer(DateTime startTime)
        {
            WorkTimer = new Timer(startTime);
            WorkTimer.TimerUpdated += Timer_Update;
            WorkTimer.TimerStarted += Timer_Start;
            WorkTimer.TimerStopped += Timer_Stop;
            WorkTimer.TimerPaused += Timer_Pause;
            WorkTimer.Start();
        }

        private void StopTimer()
        {
            if (WorkTimer != null)
            {
                WorkTimer.Stop();
                WorkTimer.TimerUpdated -= Timer_Update;
                WorkTimer.TimerStarted -= Timer_Start;
                WorkTimer.TimerStopped -= Timer_Stop;
                WorkTimer.TimerPaused -= Timer_Pause;
                WorkTimer.Dispose();
                WorkTimer = null;
            }
        }

        #endregion

        #region Timer Panel Adjustments

        private void TimerStartAdjustments()
        {
            FunctionText.Text = Formatter.FormatMode($"{ModeFunction}");
            TimerText.ReadOnly = true;
            Painter<Blue>.Paint(TimerText, PaintMode.Light);
            FunctionButton.SetImage(Resources.Pause);
        }

        private void TimerPauseAdjustments()
        {
            FunctionText.Text = Formatter.FormatMode($"{ModeTimer}");
            TimerText.ReadOnly = true;
            Painter<Blue>.Paint(TimerText, PaintMode.Normal);
            FunctionButton.SetImage(Resources.Play);
        }

        private void TimerStopAdjustments()
        {
            FunctionText.Text = Formatter.FormatMode($"{ModeTimer}");
            TimerText.ReadOnly = true;
            TimerText.Text = c_InitialTime;
            Painter<Blue>.Paint(TimerText, PaintMode.Dark);
            FunctionButton.SetImage(Resources.Stop);
        }

        private void TimerOvertimeAdjustments()
        {
            FunctionText.Text = Formatter.FormatMode($"{ModeFunction}");
            TimerText.ReadOnly = true;
        }

        #endregion

        private void CheckOvertime()
        {
            //set mode and adjust timer values only once
            if (ModeFunction != FunctionMode.Overtime)
            {
                Log.Info("WorkTimer in Overtime mode");
                ModeFunction = FunctionMode.Overtime;
                Settings.Default.ModeFunction = ModeFunction;
                Settings.Default.Save();

                TimerOvertimeAdjustments();
            }

            //when overtime reaches to 2 hours, stop timer
            if (OverTime.Hours >= 2)
            {
                Log.Info("Deadline has been reached. WorkTimer will stop.");
                StopTimer();
                return;
            }

            //change color of TimerText if not changed
            if (OverTime > TimeSpan.FromMinutes(90))
            {
                Painter<Red>.Paint(TimerText, PaintMode.Dark);

                //display reminder after 90 minutes of overtime for every defined time slot
                if (OverTime.Minutes % Constants.TimerReminderInterval == 0 &&
                    OverTime.Seconds == 0)
                {
                    var lastMinutes = 60 - OverTime.Minutes;
                    Log.Debug($"Approaching deadline: Last {lastMinutes} minutes.");

                    var message = $"Last {lastMinutes} minutes for leaving the office.";
                    BalloonTip.Show("Work Hour Deadline", message, ToolTipIcon.Info, 2000);
                }
            }
        }

        #endregion

        //################################################################################
        #region Timer Event Handlers

        private void Timer_Update(object sender, TimerUpdateEventArgs args)
        {
            if (InvokeRequired)
            {
                TimerUpdateCallback callback = Timer_Update;
                Invoke(callback, this, args);
            }
            else
            {
                if (args.Overtime.Ticks < 0)
                {
                    ElapsedTime = args.ElapsedTime;
                    RemainingTime = args.RemainingTime;
                    TimerText.Text = Formatter.FormatTimeSpan(ModeFunction == FunctionMode.Elapsed ? ElapsedTime : RemainingTime);
                }
                else //overtime block
                {
                    OverTime = args.Overtime;
                    CheckOvertime();
                    TimerText.Text = Formatter.FormatTimeSpan(OverTime);
                }
            }
        }

        private void Timer_Start(object sender, EventArgs args)
        {
            if (InvokeRequired)
            {
                TimerStateChangeCallback callback = Timer_Start;
                Invoke(callback, this, args);
            }
            else
            {
                ModeTimer = TimerMode.Started;
                Settings.Default.ModeTimer = ModeTimer;
                Settings.Default.ModeFunction = ModeFunction;
                Settings.Default.StartTime = WorkTimer.StartTime;
                Settings.Default.Save();

                TimerStartAdjustments();

                Log.Info("WorkTimer has been started.");
                Log.Info($"Start date/time: {WorkTimer.StartTime}");
                Log.Info($"Function mode : {ModeFunction}");
            }
        }

        private void Timer_Pause(object sender, EventArgs args)
        {
            if (InvokeRequired)
            {
                TimerStateChangeCallback callback = Timer_Pause;
                Invoke(callback, this, args);
            }
            else
            {
                ModeTimer = TimerMode.Paused;
                Settings.Default.ModeTimer = ModeTimer;
                Settings.Default.ModeFunction = ModeFunction;
                Settings.Default.PauseTime = TimeSpan.Parse(TimerText.Text);
                Settings.Default.Save();

                TimerPauseAdjustments();

                Log.Info("WorkTimer has been paused.");
                Log.Info($"Start time: {WorkTimer.StartTime.TimeOfDay}");
                Log.Info($"Pause time: {Settings.Default.PauseTime}");
                Log.Info($"Function mode : {ModeFunction}");
            }
        }

        private void Timer_Stop(object sender, EventArgs args)
        {
            if (InvokeRequired)
            {
                TimerStateChangeCallback callback = Timer_Stop;
                Invoke(callback, this, args);
            }
            else
            {
                ModeTimer = TimerMode.Stopped;
                ModeFunction = FunctionMode.Elapsed; //default function mode

                Settings.Default.ModeTimer = ModeTimer;
                Settings.Default.ModeFunction = ModeFunction;
                Settings.Default.StartTime = DateTime.MinValue;
                Settings.Default.PauseTime = TimeSpan.MinValue;
                Settings.Default.Save();

                TimerStopAdjustments();

                Log.Info("WorkTimer has been stopped.");
            }
        }

        #endregion

        //################################################################################
        #region Form Event Handlers

        private void TimerText_DoubleClick(object sender, EventArgs e)
        {
            if (ModeTimer != TimerMode.Started)
            {
                TimerText.ReadOnly = false;
            }
        }

        private void TimerText_KeyPress(object sender, KeyPressEventArgs e)
        {
            var timeText = TimerText.Text;
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

            if (e.KeyChar == (char)13)
            {
                if (ModeTimer == TimerMode.Paused)
                {
                    StopTimer();
                }

                if (m_TimerHelper.ValidateTime(timeText, out DateTime startTime))
                {
                    InitializeSettingsAndStartTimer(startTime, TimerMode.Started);
                }
                else
                {
                    Log.Error($"Time entered could not be verified. Value: {{{timeText}}}", null);
                    TimerText.Text = c_InitialTime;
                }
            }
        }

        private void TimerText_LostFocus(object sender, EventArgs e)
        {
            TimerText.ReadOnly = true;
        }

        private void FunctionText_DoubleClick(object sender, EventArgs e)
        {
            if (ModeTimer == TimerMode.Started)
            {
                if (ModeFunction == FunctionMode.Elapsed)
                {
                    Log.Info("WorkTimer function is changed as 'Remains'");
                    ModeFunction = FunctionMode.Remains;
                }
                else if (ModeFunction == FunctionMode.Remains)
                {
                    Log.Info("WorkTimer function is changed as 'Elapsed'");
                    ModeFunction = FunctionMode.Elapsed;
                }
                else
                {
                    ModeFunction = FunctionMode.Overtime;
                }

                FunctionText.Text = Formatter.FormatMode($"{ModeFunction}");

                Settings.Default.ModeFunction = ModeFunction;
                Settings.Default.Save();
            }
        }

        private void FunctionButton_Click(object sender, EventArgs e)
        {
            if (((MouseEventArgs)e).Button == MouseButtons.Left)
            {
                if (ModeTimer != TimerMode.Stopped)
                {
                    if (ModeTimer == TimerMode.Started)
                    {
                        WorkTimer.Pause();
                    }
                    else if (ModeTimer == TimerMode.Paused)
                    {
                        InitializeSettingsAndStartTimer(Settings.Default.StartTime, TimerMode.Started);
                    }

                    TimerText.HideSelection = true;
                    ((Control)sender).Focus();
                }
            }
        }

        #endregion
    }
}
