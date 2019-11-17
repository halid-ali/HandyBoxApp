using HandyBoxApp.StockExchange.Interfaces;
using HandyBoxApp.StockExchange.StockService;

using System;

namespace HandyBoxApp.StockExchange
{
    public class StockServiceFactory
    {
        //################################################################################
        #region Constructor

        private StockServiceFactory() { }

        #endregion

        //################################################################################
        #region Public Static Members

        public static IStockService CreateService(string serviceName)
        {
            switch (serviceName)
            {
                case "Yahoo":
                    return new YahooStockService();

                default:
                    throw new ArgumentException("Invalid stock serviceType argument.");
            }
        }

        #endregion
    }
}
