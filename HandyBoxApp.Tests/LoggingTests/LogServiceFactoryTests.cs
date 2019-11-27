using HandyBoxApp.Logging;
using HandyBoxApp.Logging.LoggingFormats;

using NUnit.Framework;
using System;

namespace HandyBoxApp.Tests.LoggingTests
{
    [TestFixture]
    public class LogServiceFactoryTests
    {
        [Test]
        public void CreateService_TxtLogFormat_ReturnsNewService()
        {
            var txtService = LogServiceFactory.CreateService(LogFormat.Txt);

            Assert.That(txtService is TxtLogFormat, Is.True);
        }

        [Test]
        public void CreateService_XmlLogFormat_ThrowsException()
        {
            void test() => LogServiceFactory.CreateService(LogFormat.Xml);

            Assert.Throws(typeof(NotImplementedException), test);
        }
    }
}
