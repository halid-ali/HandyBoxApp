using HandyBoxApp.StockExchange.EventArgs;

using System;

namespace HandyBoxApp.StockExchange.Interfaces
{
    public interface IStockService
    {
        event EventHandler<StockUpdateEventArgs> StockUpdated;

        IStockInfo GetStockInfo { get; }

        void GetStockData();
    }
}
