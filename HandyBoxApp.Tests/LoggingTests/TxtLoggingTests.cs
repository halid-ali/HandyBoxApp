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
            var txtFormat = LogServiceFactory.CreateService(LogFormat.Txt);

            txtFormat.LogMessage("test message");
        }
    }
}
