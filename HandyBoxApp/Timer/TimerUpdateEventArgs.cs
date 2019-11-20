using System;

namespace HandyBoxApp.Timer
{
    internal class TimerUpdateEventArgs
    {
        public TimerUpdateEventArgs(TimeSpan elapsedTime, TimeSpan remainingTime)
        {
            ElapsedTime = elapsedTime;
            RemainingTime = remainingTime;
        }

        internal TimeSpan ElapsedTime { get; }

        internal TimeSpan RemainingTime { get; }
    }
}