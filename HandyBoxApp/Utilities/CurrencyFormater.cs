using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandyBoxApp.Utilities
{
    internal class CurrencyFormater
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
