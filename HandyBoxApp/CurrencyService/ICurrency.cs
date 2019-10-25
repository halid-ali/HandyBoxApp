using System.Security.Policy;

namespace HandyBoxApp.CurrencyService
{
    internal interface ICurrency
    {
        string Name { get; }

        string Tag { get; }

        Url SourceUrl { get; }
    }
}
