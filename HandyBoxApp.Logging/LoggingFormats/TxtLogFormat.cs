using System;
using System.Diagnostics;
using System.IO;
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
        private const string c_DateTimeFormat = "dd.MM.yyyy - HH:mm.ss";

        #endregion

        //################################################################################
        #region ILoggingService Members

        void ILoggingService.ClearLogs()
        {
            ClearFileContent();
        }

        void ILoggingService.LogException(Exception exception)
        {
            var stringBuilder = new StringBuilder();
            FormatLogException(stringBuilder, exception);
            LogToFile(stringBuilder);
        }

        void ILoggingService.LogMessage(string message)
        {
            var stringBuilder = new StringBuilder();
            FormatLogMessage(stringBuilder, message);
            LogToFile(stringBuilder);
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

        private void LogToFile(StringBuilder stringBuilder)
        {
            using (Stream fileStream = LogStream)
            {
                byte[] buffer = Encoding.ASCII.GetBytes(stringBuilder.ToString());
                fileStream.Write(buffer, 0, buffer.Length);
                fileStream.Close();
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
