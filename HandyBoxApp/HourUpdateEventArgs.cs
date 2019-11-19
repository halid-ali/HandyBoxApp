using System;

namespace HandyBoxApp
{
    internal class HourUpdateEventArgs
    {
        internal TimeSpan ElapsedTime { get; set; }

        internal TimeSpan RemainingTime { get; set; }
    }
}