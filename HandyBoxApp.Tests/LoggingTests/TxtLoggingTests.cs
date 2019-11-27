using HandyBoxApp.Logging;
using NUnit.Framework;

namespace HandyBoxApp.Tests.LoggingTests
{
    [TestFixture]
    public class TxtLoggingTests
    {
        [Test]
        public void TxtFormat_LogMessage()
        {
            var log = LogServiceFactory.CreateService(LogFormat.Txt);
            log.Info("test message");

            var logList = log.GetLogs(LogType.Info);
        }
    }
}
