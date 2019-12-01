using HandyBoxApp.Properties;

using System;
using System.IO;

namespace HandyBoxApp.WorkTimer
{
    internal class TimerHelper
    {
        internal bool ValidateTime(string timerValue, out DateTime startTime)
        {
            startTime = DateTime.MinValue;

            //verify digit count
            if (timerValue.Length != 6) return false;

            //verify first digit couple that they represent a valid hour
            if (!int.TryParse(timerValue.Substring(0, 2), out int hourValue)) return false;
            if (hourValue < 0 || hourValue > 23) return false;

            //verify second digit couple that they represent a valid minute
            if (!int.TryParse(timerValue.Substring(2, 2), out int minuteValue)) return false;
            if (minuteValue < 0 || minuteValue > 59) return false;

            //verify third digit couple that they represent a valid second
            if (!int.TryParse(timerValue.Substring(4, 2), out int secondValue)) return false;
            if (secondValue < 0 || secondValue > 59) return false;

            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;
            var day = DateTime.Now.Day;

            startTime = new DateTime(year, month, day, hourValue, minuteValue, secondValue);

            return true;
        }

        #region Testing Members

        internal DateTime GetTestingStartTime(DateTime startTime, int phase)
        {
            switch (phase)
            {
                case 1: //1 hour before overtime
                    return DateTime.Now.Subtract(new TimeSpan(7, 0, 0));

                case 2: //3 seconds before overtime
                    return DateTime.Now.Subtract(new TimeSpan(8, 44, 57));

                case 3: //3 seconds before between overtime and reminder
                    return DateTime.Now.Subtract(new TimeSpan(9, 14, 57));

                case 4: //3 seconds before reminder
                    return DateTime.Now.Subtract(new TimeSpan(10, 14, 57));

                case 5: //3 seconds before between reminder and deadline
                    return DateTime.Now.Subtract(new TimeSpan(10, 24, 57));

                case 6: //3 seconds before deadline
                    return DateTime.Now.Subtract(new TimeSpan(10, 44, 57));

                default:
                    return startTime;
            }
        }

        internal void WriteSettings(string caller)
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "settings.txt");

            //clear file content
            File.WriteAllText(path, string.Empty);

            //write settings
            File.AppendAllLines(path, new[] { $"CALLER: {caller}" });
            File.AppendAllLines(path, new[] { "----------------------------------------" });

            File.AppendAllLines(path, new[] { $"StartTime: {Settings.Default.StartTime}" });
            File.AppendAllLines(path, new[] { $"PauseTime: {Settings.Default.PauseTime}" });
            File.AppendAllLines(path, new[] { $"TimerMode: {Settings.Default.ModeTimer}" });
            File.AppendAllLines(path, new[] { $"FunctionMode: {Settings.Default.ModeFunction}" });

            File.AppendAllLines(path, new[] { "########################################" });
        }

        internal void WriteTimerAction(string log)
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "timer-action.txt");

            File.AppendAllLines(path, new[] { $"{log}" });
        }

        #endregion
    }
}
