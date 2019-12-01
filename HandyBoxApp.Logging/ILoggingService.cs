using System;
using System.Collections.Generic;

namespace HandyBoxApp.Logging
{
    public interface ILoggingService
    {
        string LogPath { get; }

        void ClearLogs();

        IEnumerable<LogEntry> GetLogs(LogType type);

        void Debug(string message);

        void Error(string message, Exception exception);

        void Info(string message);
    }
}
