using HandyBoxApp.StockExchange.Stock;

namespace HandyBoxApp.StockExchange.EventArgs
{
    public class StockUpdateEventArgs
    {
        //################################################################################
        #region Constructor

        public StockUpdateEventArgs(StockData stockData)
        {
            StockData = stockData;
        }

        #endregion

        //################################################################################
        #region Properties

        public StockData StockData { get; }

        #endregion
    }
}
