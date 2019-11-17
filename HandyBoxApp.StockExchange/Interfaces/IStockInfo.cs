namespace HandyBoxApp.StockExchange.Interfaces
{
    public interface IStockInfo
    {
        string Name { get; }

        string Service { get; }

        string Tag { get; }

        string SourceUrl { get; }
    }
}
