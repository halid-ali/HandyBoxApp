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
}
