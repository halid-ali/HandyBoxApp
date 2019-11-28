using HandyBoxApp.Logging;

using NUnit.Framework;

using System;
using System.IO;
using System.Linq;

namespace HandyBoxApp.Tests.LoggingTests
{
    [TestFixture]
    public class TxtLoggingTests
    {
        //################################################################################
        #region Fields

        private ILoggingService logService;

        #endregion

        //################################################################################
        #region Setup/TearDown

        [SetUp]
        public void Setup()
        {
            logService = LogServiceFactory.CreateTestService(LogFormat.Txt, "handyboxapp-test-logs.txt");
        }

        [TearDown]
        public void TearDown()
        {
            logService.ClearLogs();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            File.Delete(logService.LogPath);
        }

        #endregion

        //################################################################################
        #region Tests

        [Test]
        public void CreateInfoLog_GetInfoLogs_ReturnsInfoLogs()
        {
            //arrange

            logService.Info("info message");

            //act

            var logList = logService.GetLogs(LogType.Info);

            //assert

            Assert.That(logList.ToArray().Count(), Is.EqualTo(1));
        }

        [Test]
        public void CreateErrorLog_GetErrorLogs_ReturnsErrorLogs()
        {
            //arrange

            logService.Error("error message", null);

            //act

            var logList = logService.GetLogs(LogType.Error);

            //assert

            Assert.That(logList.ToArray().Count(), Is.EqualTo(1));
        }

        [Test]
        public void CreateDebugLog_GetDebugLogs_ReturnsDebugLogs()
        {
            //arrange

            logService.Debug("debug message");

            //act

            var logList = logService.GetLogs(LogType.Debug);

            //assert

            Assert.That(logList.ToArray().Count(), Is.EqualTo(1));
        }

        [Test]
        public void CreateInfoLog_GetErrorLogs_ReturnsZero()
        {
            //arrange

            logService.Info("create info log, get error log");

            //act

            var logList = logService.GetLogs(LogType.Error);

            //assert

            Assert.That(logList.ToArray().Count(), Is.EqualTo(0));
        }

        [Test]
        public void CreateInfoLog_GetDebugLogs_ReturnsZero()
        {
            //arrange

            logService.Info("create info log, get debug log");

            //act

            var logList = logService.GetLogs(LogType.Debug);

            //assert

            Assert.That(logList.ToArray().Count(), Is.EqualTo(0));
        }

        [Test]
        public void CreateErrorLog_GetInfoLogs_ReturnsZero()
        {
            //arrange

            logService.Error("create error log, get info log", null);

            //act

            var logList = logService.GetLogs(LogType.Info);

            //assert

            Assert.That(logList.ToArray().Count(), Is.EqualTo(0));
        }

        [Test]
        public void CreateErrorLog_GetDebugLogs_ReturnsZero()
        {
            //arrange

            logService.Error("create error log, get debug log", null);

            //act

            var logList = logService.GetLogs(LogType.Debug);

            //assert

            Assert.That(logList.ToArray().Count(), Is.EqualTo(0));
        }

        [Test]
        public void CreateDebugLog_GetInfoLogs_ReturnsZero()
        {
            //arrange

            logService.Debug("create debug log, get info log");

            //act

            var logList = logService.GetLogs(LogType.Info);

            //assert

            Assert.That(logList.ToArray().Count(), Is.EqualTo(0));
        }

        [Test]
        public void CreateDebugLog_GetErrorLogs_ReturnsZero()
        {
            //arrange

            logService.Debug("create debug log, get error log");

            //act

            var logList = logService.GetLogs(LogType.Error);

            //assert

            Assert.That(logList.ToArray().Count(), Is.EqualTo(0));
        }

        [Test]
        public void CreateErrorLog_WithException_ReturnsErrorLog()
        {
            //arrange

            try
            {
                throw new ArgumentException("invalid argument");
            }
            catch (ArgumentException ex)
            {
                logService.Error("error message", ex);
            }

            //act

            var logList = logService.GetLogs(LogType.Error);

            //assert

            Assert.That(logList.ToArray().Count(), Is.EqualTo(1));
        }

        [Test]
        public void CreateErrorLog_WithInnerException_ReturnsErrorLog()
        {
            //arrange

            try
            {
                throw new ArgumentException("inner exception message");
            }
            catch (ArgumentException innerException)
            {
                try
                {
                    throw new Exception("exception message", innerException);
                }
                catch (Exception exception)
                {
                    logService.Error("error message", exception);
                }
            }

            //act

            var logList = logService.GetLogs(LogType.Error);

            //assert

            Assert.That(logList.ToArray().Count(), Is.EqualTo(1));
        }

        #endregion
    }
}
