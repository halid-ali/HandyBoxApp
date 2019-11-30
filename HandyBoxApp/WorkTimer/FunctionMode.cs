using System;

namespace HandyBoxApp.WorkTimer
{
    [Serializable]
    internal enum FunctionMode
    {
        Elapsed,
        Remains,
        Stopped,
        Paused,
        Overtime
    }
}
