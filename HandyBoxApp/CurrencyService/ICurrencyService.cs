using System.Security.Policy;

namespace HandyBoxApp.CurrencyService
{
    internal interface ICurrencyService
    {
        string Name { get; }

        string Tag { get; }

        Url SourceUrl { get; }
    }
}
