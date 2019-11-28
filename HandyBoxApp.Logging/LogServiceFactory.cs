using HandyBoxApp.Logging.LoggingFormats;

using System;

namespace HandyBoxApp.Logging
{
    public class LogServiceFactory
    {
        //################################################################################
        #region Public Members

        public static ILoggingService CreateService(LogFormat format)
        {
            switch (format)
            {
                case LogFormat.Txt:
                    return new TxtLogFormat();

                case LogFormat.Xml:
                    throw new NotImplementedException();

                default:
                    throw new ArgumentException("Invalid log format.");
            }
        }

        #endregion

        //################################################################################
        #region Internal Members

        internal static ILoggingService CreateTestService(LogFormat format, string logFileName)
        {
            switch (format)
            {
                case LogFormat.Txt:
                    return new TxtLogFormat(logFileName);

                case LogFormat.Xml:
                    throw new NotImplementedException();

                default:
                    throw new ArgumentException("Invalid log format.");
            }
        }

        #endregion
    }
}
