using System;

namespace HandyBoxApp.Logging
{
    public interface ILoggingService
    {
        void ClearLogs();

        void LogException(Exception exception);

        void LogMessage(string message);
    }
}
