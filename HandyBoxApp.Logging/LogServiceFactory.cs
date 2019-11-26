using HandyBoxApp.Logging.LoggingFormats;

using System;

namespace HandyBoxApp.Logging
{
    public class LogServiceFactory
    {
        public static ILoggingService CreateService(LogFormat format)
        {
            switch (format)
            {
                case LogFormat.Txt:
                    return new TxtLogFormat();

                case LogFormat.Xml:
                    return new XmlLogFormat();

                default:
                    throw new ArgumentException("Invalid log format.");
            }
        }
    }
}
