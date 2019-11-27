using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HandyBoxApp.Logging.LoggingFormats
{
    internal class TxtLogFormat : LogFormatBase, ILoggingService
    {
        //################################################################################
        #region Fields

        private const int c_TabSize = 4;
        private const int c_DateTimeLength = 21;
        private const char c_PaddingChar = ' ';
        private const string c_DateTimeFormat = "yyyy.MM.dd - HH:mm.ss";
        private const string c_LogFileName = "handyboxapp-logs.txt";

        private readonly IList<LogEntry> m_LogEntries = new List<LogEntry>();

        #endregion

        //################################################################################
        #region ILoggingService Members

        string ILoggingService.LogPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), c_LogFileName);

        void ILoggingService.ClearLogs()
        {
            ClearFileContent();
        }

        IEnumerable<LogEntry> ILoggingService.GetLogs(LogType type)
        {
            return m_LogEntries.Where(x => x.Type == type);
        }

        void ILoggingService.Debug(string message)
        {
            var debugLogEntry = new LogEntry(LogType.Debug, message, null);
            m_LogEntries.Add(debugLogEntry);
        }

        void ILoggingService.Error(string message, Exception exception)
        {
            var errorLogEntry = new LogEntry(LogType.Error, message, exception);
            m_LogEntries.Add(errorLogEntry);
        }

        void ILoggingService.Info(string message)
        {
            var infoLogEntry = new LogEntry(LogType.Info, message, null);
            m_LogEntries.Add(infoLogEntry);
        }

        #endregion

        //################################################################################
        #region Private Members

        private void FormatLogMessage(StringBuilder stringBuilder, string message)
        {
            stringBuilder.Append(GetFormattedDateTimeNow());
            stringBuilder.AppendLine(message);
        }

        private void FormatLogException(StringBuilder stringBuilder, Exception exception, int level = 1, bool isInnerException = false)
        {
            if (!isInnerException)
                stringBuilder.Append(GetFormattedDateTimeNow()); 
            else
                stringBuilder.Append(GetEmptyPadding(level));

            stringBuilder.AppendLine(exception.Message);
            stringBuilder.Append(GetEmptyPadding(level));
            stringBuilder.AppendLine(exception.StackTrace);

            if (exception.InnerException != null)
            {
                FormatLogException(stringBuilder, exception.InnerException, level++, true);
            }
        }

        private string GetFormattedDateTimeNow()
        {
            return DateTime.Now
                .ToString(c_DateTimeFormat)
                .PadRight(GetPaddingWidth(), c_PaddingChar);
        }

        private string GetEmptyPadding(int level)
        {
            return new string(' ', GetPaddingWidth(level));
        }

        private int GetPaddingWidth(int level = 1)
        {
            return c_DateTimeLength + c_TabSize * level;
        }

        #endregion
    }
}
