using System;

namespace HandyBoxApp.Logging.LoggingFormats
{
    internal class XmlLogFormat : LogFormatBase, ILoggingService
    {
        //################################################################################
        #region ILoggingService Members

        void ILoggingService.ClearLogs()
        {
            ClearFileContent();
        }

        void ILoggingService.LogException(Exception exception)
        {
            throw new NotImplementedException();
        }

        void ILoggingService.LogMessage(string message)
        {
            throw new NotImplementedException();
        }

        #endregion

        //################################################################################
        #region Private Members



        #endregion
    }
}
