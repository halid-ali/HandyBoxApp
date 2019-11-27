using System.IO;

namespace HandyBoxApp.Logging.LoggingFormats
{
    internal class LogFormatBase
    {
        protected void WriteLogs()
        {

        }

        protected void ClearFileContent()
        {
            lock (this)
            {
                //File.WriteAllText(LogPath, string.Empty); 
            }
        }
    }
}
