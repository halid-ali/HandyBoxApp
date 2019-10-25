namespace HandyBoxApp.CurrencyService
{
    internal interface ICurrencyService
    {
        string Name { get; }

        string Tag { get; }

        string SourceUrl { get; }
    }
}
