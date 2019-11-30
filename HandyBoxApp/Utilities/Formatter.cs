using HandyBoxApp.WorkTimer;

using System;
using System.Globalization;

namespace HandyBoxApp.Utilities
{
    internal class Formatter
    {
        internal static string FormatCurrency(double value, Pad padding, int totalWidth)
        {
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture(Constants.DefaultCultureCode);
            cultureInfo.NumberFormat.CurrencySymbol = Constants.DefaultCurrencyCode;
            cultureInfo.NumberFormat.CurrencyPositivePattern = 3;

            var result = string.Format(cultureInfo, Constants.Precision, value);

            return FormatString(result, padding, totalWidth);
        }

        internal static string FormatChangeRate(double value, Pad padding, int totalWidth)
        {
            var result = $"Change rate: %{value:F4}";

            return FormatString(result, padding, totalWidth);
        }

        internal static string FormatString(string value, Pad padding, int totalWidth, char paddingChar = Constants.DefaultPaddingChar)
        {
            if (padding == Pad.Right)
            {
                return value.PadRight(totalWidth, paddingChar);
            }

            return value.PadLeft(totalWidth, paddingChar);
        }

        internal static string FormatTimerFunction(FunctionMode mode)
        {
            return FormatString($"{mode.ToString()}:", Pad.Right, 9);
        }

        internal static string FormatTimeSpan(TimeSpan timeSpan)
        {
            return $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
        }

        internal static string FormatDateTime(DateTime dateTime)
        {
            return $"{dateTime.Hour:D2}:{dateTime.Minute:D2}:{dateTime.Second:D2}";
        }
    }

    internal enum Pad
    {
        Left,
        Right
    }
}
