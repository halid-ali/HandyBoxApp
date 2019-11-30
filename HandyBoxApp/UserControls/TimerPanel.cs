using HandyBoxApp.ColorScheme;
using HandyBoxApp.ColorScheme.Colors;
using HandyBoxApp.CustomComponents;
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

        private const string c_Initial = "00:00:00";
        private readonly TimerHelper m_TimerHelper = new TimerHelper();

        #endregion

        //################################################################################
        #region Constructor

        public TimerPanel()
        {
            m_TimerHelper.WriteSettings("CONSTRUCTOR");

            InitializeComponent();
            OrderControls();

            InitializeTimerSettingsAndStartTimer();
        }

        #endregion

        //################################################################################
        #region Properties

        private Label FunctionText { get; } = new Label();

        private TextBox TimerText { get; } = new TextBox();

        private FlowLayoutPanel TimerContainer { get; } = new FlowLayoutPanel();

        private ImageButton FunctionButton { get; set; }

        private FlowLayoutPanel ContainerPanel { get; } = new FlowLayoutPanel();

        private Timer WorkTimer { get; set; }

        private FunctionMode ModeFunction { get; set; } = FunctionMode.Elapsed;

        private TimerMode ModeTimer { get; set; }

        private TimeSpan ElapsedTime { get; set; }

        private TimeSpan RemainingTime { get; set; }

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
            FunctionText.Text = Formatter.FormatTimerFunction(FunctionMode.Stopped);
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

            TimerText.Text = c_Initial;
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
                    if (((MouseEventArgs)args).Button == MouseButtons.Left)
                    {
                        if (WorkTimer != null)
                        {
                            if (WorkTimer.IsStarted)
                            {
                                WorkTimer.Pause();
                                FunctionButton.SetImage(Resources.Play);
                                FunctionText.Text = Formatter.FormatTimerFunction(FunctionMode.Paused);

                                Settings.Default.Save();
                            }
                            else if (WorkTimer.IsPaused)
                            {
                                WorkTimer.Start();
                                FunctionButton.SetImage(Resources.Pause);
                                FunctionText.Text = Formatter.FormatTimerFunction(ModeFunction);

                                Settings.Default.Save();
                            }

                            TimerText.HideSelection = true;
                            button.Focus();
                        }
                    }
                };
            }

            FunctionButton = new ImageButton(PauseAction, Resources.Stop)
            {
                Margin = new Padding(0),
                ContextMenu = new ContextMenu { MenuItems = { new MenuItem("Stop", (s, a) => StopTimer()) } }
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

        private void InitializeTimerSettingsAndStartTimer()
        {
            var savedTime = Settings.Default.StartTime;

            if (DateTime.Now.Subtract(savedTime) < TimeSpan.FromDays(1))
            {
                StartTimer(savedTime);
            }
        }

        private void StartTimer(DateTime startTime)
        {
            startTime = m_TimerHelper.GetTestingStartTime(startTime, 0);

            //adjust timer text
            TimerText.ReadOnly = true;
            Painter<Blue>.Paint(TimerText, PaintMode.Light);

            //adjust function text
            FunctionText.Text = Formatter.FormatTimerFunction(FunctionMode.Elapsed);

            //adjust function button
            FunctionButton.SetImage(Resources.Pause);

            WorkTimer = new Timer(startTime);
            WorkTimer.TimerUpdated += Timer_Update;
            WorkTimer.TimerStarted += Timer_Start;
            WorkTimer.TimerStopped += Timer_Stop;
            WorkTimer.TimerPaused += Timer_Pause;
            WorkTimer.Start();
        }

        private void StopTimer()
        {
            //adjust timer text
            TimerText.Text = c_Initial;
            TimerText.ReadOnly = true;
            Painter<Blue>.Paint(TimerText, PaintMode.Normal);

            //adjust function text
            FunctionText.Text = Formatter.FormatTimerFunction(FunctionMode.Stopped);

            //adjust function button
            FunctionButton.SetImage(Resources.Stop);

            //adjust mode
            ModeFunction = FunctionMode.Elapsed;

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

            Settings.Default.StartTime = DateTime.MinValue;
            Settings.Default.Save();
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
                    //when overtime reaches to 2 hours, stop timer
                    if (args.Overtime.Hours >= 2)
                    {
                        StopTimer();
                        return;
                    }

                    //set mode and adjust timer values only once
                    if (ModeFunction != FunctionMode.Overtime)
                    {
                        ModeFunction = FunctionMode.Overtime;
                        Painter<Green>.Paint(TimerText, PaintMode.Dark);
                        FunctionText.Text = Formatter.FormatTimerFunction(FunctionMode.Overtime);
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

                    TimerText.Text = Formatter.FormatTimeSpan(args.Overtime);
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
                //todo: timer start
                m_TimerHelper.WriteTimerAction($"Timer Started:{ElapsedTime}");
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
                //todo: timer start
                m_TimerHelper.WriteTimerAction($"Timer Stopped:{ElapsedTime}");
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
                //todo: timer start
                m_TimerHelper.WriteTimerAction($"Timer Paused:{ElapsedTime}");
            }
        }

        #endregion

        //################################################################################
        #region Form Event Handlers

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

                if (m_TimerHelper.ValidateTime(timeText, out DateTime startTime))
                {
                    Settings.Default.StartTime = startTime;
                    Settings.Default.Save();

                    StartTimer(startTime);
                }
                else
                {
                    TimerText.Text = c_Initial;
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
                if (ModeFunction == FunctionMode.Elapsed)
                {
                    ModeFunction = FunctionMode.Remains;
                }
                else if (ModeFunction == FunctionMode.Remains)
                {
                    ModeFunction = FunctionMode.Elapsed;
                }
                else
                {
                    ModeFunction = FunctionMode.Overtime;
                }

                FunctionText.Text = Formatter.FormatTimerFunction(ModeFunction);
            }
        }

        #endregion
    }
}
