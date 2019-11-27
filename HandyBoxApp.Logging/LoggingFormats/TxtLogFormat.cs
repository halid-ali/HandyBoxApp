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

        private const int c_TabWidth = 4;
        private const int c_LogWidth = 9;
        private const int c_DateWidth = 21;
        private const char c_PaddingChar = ' ';
        private const string c_DateTimeFormat = "yyyy.MM.dd - HH:mm.ss";
        private const string c_LogFileName = "handyboxapp-logs.txt";

        private readonly string m_LogFilePath;
        private readonly IList<LogEntry> m_LogEntries = new List<LogEntry>();

        #endregion

        public TxtLogFormat()
        {
            m_LogFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), c_LogFileName);
        }

        //################################################################################
        #region ILoggingService Members

        string ILoggingService.LogPath => m_LogFilePath;

        void ILoggingService.ClearLogs()
        {
            m_LogEntries.Clear();
            DeleteLogs(m_LogFilePath);
        }

        IEnumerable<LogEntry> ILoggingService.GetLogs(LogType type)
        {
            return m_LogEntries.Where(x => x.Type == type);
        }

        void ILoggingService.Debug(string message)
        {
            var debugLogEntry = new LogEntry(LogType.Debug, message, null);
            m_LogEntries.Add(debugLogEntry);

            var formattedLog = FormatLogMessage(debugLogEntry);
            WriteLogs(formattedLog, m_LogFilePath);
        }

        void ILoggingService.Error(string message, Exception exception)
        {
            var errorLogEntry = new LogEntry(LogType.Error, message, exception);
            m_LogEntries.Add(errorLogEntry);

            var formattedLog = FormatLogMessage(errorLogEntry);
            WriteLogs(formattedLog, m_LogFilePath);
        }

        void ILoggingService.Info(string message)
        {
            var infoLogEntry = new LogEntry(LogType.Info, message, null);
            m_LogEntries.Add(infoLogEntry);

            var formattedLog = FormatLogMessage(infoLogEntry);
            WriteLogs(formattedLog, m_LogFilePath);
        }

        #endregion

        //################################################################################
        #region Private Members

        private string FormatLogMessage(LogEntry logEntry)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(GetFormattedDateTimeNow());
            stringBuilder.Append(GetSeparator());
            stringBuilder.Append($"[{logEntry.Type}]".PadRight(c_LogWidth, c_PaddingChar));
            stringBuilder.Append(GetSeparator());
            stringBuilder.AppendLine(logEntry.Message);

            if (logEntry.Exception != null)
            {
                FormatLogException(stringBuilder, logEntry.Exception, 1); //first level of exception
            }

            return stringBuilder.ToString();
        }

        private void FormatLogException(StringBuilder stringBuilder, Exception exception, int level)
        {
            stringBuilder.Append(GetEmptyIndent(level));
            stringBuilder.AppendLine(exception.Message);
            stringBuilder.Append(GetEmptyIndent(level));
            stringBuilder.AppendLine(exception.StackTrace);

            if (exception.InnerException != null)
            {
                FormatLogException(stringBuilder, exception.InnerException, ++level);
            }
        }

        private string GetFormattedDateTimeNow()
        {
            return DateTime.Now.ToString(c_DateTimeFormat);
        }

        private string GetSeparator(int level = 1)
        {
            return new string(' ', c_TabWidth * level);
        }

        private string GetEmptyIndent(int level)
        {
            return new string(' ', c_DateWidth + c_TabWidth + c_LogWidth + GetSeparator(level).Length);
        }

        #endregion
    }
}
