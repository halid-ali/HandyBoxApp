using HandyBoxApp.StockExchange.Interfaces;
using HandyBoxApp.StockExchange.Stock;
using HandyBoxApp.StockExchange.StockService;

using System;
using System.Collections.Generic;
using System.Linq;

namespace HandyBoxApp.StockExchange
{
    public class StockServiceFactory
    {
        private static IList<IStockInfo> s_StockInfoList;
        private static readonly StockInfoLoader Loader;

        //################################################################################
        #region Constructor

        static StockServiceFactory()
        {
            Loader = new StockInfoLoader();
        }

        private StockServiceFactory()
        {

        }

        #endregion

        //################################################################################
        #region Public Static Members

        public static IStockService CreateService(string serviceName, string stockName)
        {
            if (s_StockInfoList == null)
            {
                s_StockInfoList = Loader.GetStockList().ToList();
            }

            switch (serviceName)
            {
                case "Yahoo":
                    return new YahooStockService(GetStockInfo(serviceName, stockName));

                default:
                    throw new ArgumentException("Invalid stock serviceType argument.");
            }
        }

        #endregion

        private static IStockInfo GetStockInfo(string serviceName, string stockName)
        {
            var stockInfo = from stock in s_StockInfoList
                            where stock.Tag == stockName && stock.Service == serviceName
                            select stock;

            return stockInfo.First();
        }
    }
}
