using System;

namespace HandyBoxApp.WorkTimer
{
    internal class TimerUpdateEventArgs
    {
        public TimerUpdateEventArgs(TimeSpan elapsedTime, TimeSpan remainingTime, TimeSpan overtime)
        {
            ElapsedTime = elapsedTime;
            RemainingTime = remainingTime;
            Overtime = overtime;
        }

        internal TimeSpan ElapsedTime { get; }

        internal TimeSpan RemainingTime { get; }

        internal TimeSpan Overtime { get; }
    }
}