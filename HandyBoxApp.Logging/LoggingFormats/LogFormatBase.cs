using System.IO;

namespace HandyBoxApp.Logging.LoggingFormats
{
    internal class LogFormatBase
    {
        private static object m_LockObject = new object();

        protected static void WriteLogs(string logs, string path)
        {
            lock (m_LockObject)
            {
                File.AppendAllText(path, logs);
            }
        }

        protected static void DeleteLogs(string path)
        {
            lock (m_LockObject)
            {
                File.WriteAllText(path, string.Empty);
            }
        }
    }
}
