using System;
using System.ComponentModel;
using System.Threading;

namespace HandyBoxApp.WorkTimer
{
    internal class Timer
    {
        //################################################################################
        #region Fields

        private readonly TimeSpan m_LunchBreak = new TimeSpan(0, 45, 0);
        private readonly TimeSpan m_BaseWorkhour = new TimeSpan(8, 0, 0);
        private readonly TimeSpan m_MaximumOverwork = new TimeSpan(2, 0, 0);

        private event EventHandler<TimerUpdateEventArgs> TimerUpdate;

        #endregion

        //################################################################################
        #region Constructor

        public Timer(DateTime startTime)
        {
            StartTime = startTime;
            StartTimeWithLunchBreak = StartTime.Add(m_LunchBreak);
            FinishTime = StartTime.Add(m_BaseWorkhour).Add(m_LunchBreak);
            DeadlineTime = FinishTime.Add(m_MaximumOverwork);

            Worker.DoWork += Worker_DoWork;
            Worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        }

        #endregion

        //################################################################################
        #region Properties

        internal bool IsStarted { get; private set; }

        internal bool IsStopped { get; private set; }

        internal bool IsPaused { get; private set; }

        internal event EventHandler<TimerUpdateEventArgs> TimerUpdated
        {
            add => TimerUpdate += value;
            remove => TimerUpdate -= value;
        }

        private DateTime StartTime { get; set; }

        private DateTime StartTimeWithLunchBreak { get; set; }

        private DateTime FinishTime { get; set; }

        private DateTime DeadlineTime { get; set; }

        private BackgroundWorker Worker { get; } = new BackgroundWorker();

        #endregion

        //################################################################################
        #region Internal Members

        internal void Start()
        {
            IsStarted = true;
            IsPaused = false;
            IsStopped = false;

            Worker.RunWorkerAsync();
        }

        internal void Stop()
        {
            StartTime = DateTime.MinValue;
            FinishTime = DateTime.MinValue;
            DeadlineTime = DateTime.MinValue;

            IsStarted = false;
            IsPaused = false;
            IsStopped = true;
        }

        internal void Pause()
        {
            IsStarted = false;
            IsPaused = true;
            IsStopped = false;
        }

        #endregion

        //################################################################################
        #region BackgroundWorker Members

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (IsStarted)
            {
                OnTimerUpdate();
                Thread.Sleep(1000);
            }
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        //################################################################################
        #region Private Members

        private void OnTimerUpdate()
        {
            var elapsedTime = DateTime.Now.Subtract(StartTimeWithLunchBreak);
            var remainingTime = FinishTime.Subtract(DateTime.Now);

            var args = new TimerUpdateEventArgs(elapsedTime, remainingTime);
            Volatile.Read(ref TimerUpdate)?.Invoke(this, args);
        }

        #endregion
    }
}
