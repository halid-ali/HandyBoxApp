using System.Globalization;

namespace HandyBoxApp.Utilities
{
    internal class CurrencyFormatter
    {
        internal static string Format(double amount, string cultureCode, string currencyCode)
        {
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture(cultureCode);
            cultureInfo.NumberFormat.CurrencySymbol = currencyCode;
            cultureInfo.NumberFormat.CurrencyPositivePattern = 3;

            return string.Format(cultureInfo, Constants.Precision, amount);
        }
    }
}
