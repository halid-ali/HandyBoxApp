using System;
using System.ComponentModel;
using System.Threading;

namespace HandyBoxApp.WorkTimer
{
    internal class Timer : IDisposable
    {
        //################################################################################
        #region Fields

        private readonly TimeSpan m_LunchBreak = new TimeSpan(0, 45, 0);
        private readonly TimeSpan m_BaseWorkhour = new TimeSpan(8, 0, 0);

        private bool m_DisposedValue; // To detect redundant calls
        private readonly BackgroundWorker m_Worker = new BackgroundWorker();

        private event EventHandler<TimerUpdateEventArgs> TimerUpdate;

        #endregion

        //################################################################################
        #region Constructor

        public Timer(DateTime startTime)
        {
            StartTime = startTime;
            StartTimeWithLunchBreak = StartTime.Add(m_LunchBreak);
            FinishTime = StartTime.Add(m_BaseWorkhour).Add(m_LunchBreak);

            Worker.WorkerReportsProgress = false;
            Worker.WorkerSupportsCancellation = true;
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

        private DateTime StartTimeWithLunchBreak { get; }

        private DateTime FinishTime { get; set; }

        private BackgroundWorker Worker => m_Worker;

        #endregion

        //################################################################################
        #region Internal Members

        internal void Start()
        {
            ResetFlags();
            IsStarted = true;

            Worker.RunWorkerAsync();
        }

        internal void Stop()
        {
            ResetFlags();
            IsStopped = true;
        }

        internal void Pause()
        {
            Worker.CancelAsync();
        }

        #endregion

        //################################################################################
        #region BackgroundWorker Members

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (IsStarted)
            {
                if (sender is BackgroundWorker bgWorker && bgWorker.CancellationPending)
                {
                    ResetFlags();
                    IsPaused = true;
                    e.Cancel = true;
                    return;
                }

                OnTimerUpdate();
                Thread.Sleep(1000);
            }
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                //log pause
                return;
            }

            if (e.Error != null)
            {
                //log error
            }

            ResetFlags();
            IsStopped = true;

            StartTime = new DateTime();
            FinishTime = new DateTime();
        }

        #endregion

        //################################################################################
        #region Private Members

        private void OnTimerUpdate()
        {
            var elapsedTime = DateTime.Now.Subtract(StartTimeWithLunchBreak);
            var remainingTime = FinishTime.Subtract(DateTime.Now);
            var overtime = DateTime.Now.Subtract(FinishTime);

            var args = new TimerUpdateEventArgs(elapsedTime, remainingTime, overtime);
            Volatile.Read(ref TimerUpdate)?.Invoke(this, args);
        }

        private void ResetFlags()
        {
            IsStarted = false;
            IsStopped = false;
            IsPaused = false;
        }

        #endregion

        //################################################################################
        #region IDisposable Members

        protected virtual void Dispose(bool disposing)
        {
            if (!m_DisposedValue)
            {
                if (disposing)
                {
                    m_Worker.Dispose();
                }

                m_DisposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        #endregion
    }
}
