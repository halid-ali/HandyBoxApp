using System;

namespace HandyBoxApp.WorkTimer
{
    [Serializable]
    public enum FunctionMode
    {
        Elapsed,
        Remains,
        Stopped,
        Paused,
        Overtime
    }

    [Serializable]
    public enum TimerMode
    {
        Started,
        Stopped,
        Paused
    }
}
