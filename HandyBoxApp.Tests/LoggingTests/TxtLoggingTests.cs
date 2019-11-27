using HandyBoxApp.Logging;
using NUnit.Framework;
using System.Linq;

namespace HandyBoxApp.Tests.LoggingTests
{
    [TestFixture]
    public class TxtLoggingTests
    {
        [Test]
        public void CreateInfoLog_GetInfoLogs_ReturnsInfoLogs()
        {
            //arrange

            var log = LogServiceFactory.CreateService(LogFormat.Txt);

            //act

            log.Info("test message");
            var logList = log.GetLogs(LogType.Info);

            //assert

            Assert.That(logList.ToArray().Count(), Is.EqualTo(1));
        }

        [Test]
        public void CreateInfoLog_GetErrorLogs_ReturnsZero()
        {
            //arrange

            var log = LogServiceFactory.CreateService(LogFormat.Txt);

            //act

            log.Info("test message");
            var logList = log.GetLogs(LogType.Error);

            //assert

            Assert.That(logList.ToArray().Count(), Is.EqualTo(0));
        }
    }
}
