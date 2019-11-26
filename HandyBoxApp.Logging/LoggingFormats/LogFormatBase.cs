using System.IO;

namespace HandyBoxApp.Logging.LoggingFormats
{
    internal class LogFormatBase
    {
        protected Stream LogStream
        {
            get
            {
                var x = GetType().Assembly.GetManifestResourceNames();

                return GetType().Assembly.GetManifestResourceStream("HandyBoxApp.Logging.Resources.TxtLogs.txt");
            }
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
