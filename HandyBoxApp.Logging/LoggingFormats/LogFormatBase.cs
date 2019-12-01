using System.IO;

namespace HandyBoxApp.Logging.LoggingFormats
{
    internal class LogFormatBase
    {
        protected void WriteLogs(string logs, string path)
        {
            lock (this)
            {
                File.AppendAllText(path, logs);
            }
        }

        protected void DeleteLogs(string path)
        {
            lock (this)
            {
                File.WriteAllText(path, string.Empty);
            }
        }
    }
}
