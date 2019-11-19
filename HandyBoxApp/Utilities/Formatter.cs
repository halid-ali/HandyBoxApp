using System.Globalization;

namespace HandyBoxApp.Utilities
{
    internal class Formatter
    {
        internal static string FormatDouble(double value, Pad padding, int totalWidth)
        {
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture(Constants.DefaultCultureCode);
            cultureInfo.NumberFormat.CurrencySymbol = Constants.DefaultCurrencyCode;
            cultureInfo.NumberFormat.CurrencyPositivePattern = 3;

            var result = string.Format(cultureInfo, Constants.Precision, value);

            return FormatString(result, padding, totalWidth);
        }

        internal static string FormatString(string value, Pad padding, int totalWidth)
        {
            if (padding == Pad.Right)
            {
                return value.PadRight(totalWidth, Constants.DefaultPaddingChar);
            }

            return value.PadLeft(totalWidth, Constants.DefaultPaddingChar);
        }
    }

    internal enum Pad
    {
        Left,
        Right
    }
}
