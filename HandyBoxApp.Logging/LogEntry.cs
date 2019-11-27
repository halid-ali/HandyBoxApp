using System;

namespace HandyBoxApp.Logging
{
    public class LogEntry
    {
        public LogEntry(LogType type, string message, Exception exception)
        {
            Type = type;
            Message = message;
            Exception = exception;
        }

        public LogType Type { get; }

        public string Message { get; }

        public Exception Exception { get; }
    }
}
