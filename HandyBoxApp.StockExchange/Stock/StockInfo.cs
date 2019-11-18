using HandyBoxApp.StockExchange.Interfaces;

namespace HandyBoxApp.StockExchange.Stock
{
    internal class StockInfo : IStockInfo
    {
        //################################################################################
        #region Fields

        private readonly string m_Name;
        private readonly string m_Service;
        private readonly string m_Tag;
        private readonly string m_Url;

        #endregion

        //################################################################################
        #region Constructor

        public StockInfo(string name, string service, string tag, string url)
        {
            m_Name = name;
            m_Service = service;
            m_Tag = tag;
            m_Url = url;
        }

        #endregion

        //################################################################################
        #region IStock Members

        string IStockInfo.Name => m_Name;

        string IStockInfo.Service => m_Service;

        string IStockInfo.Tag => m_Tag;

        string IStockInfo.SourceUrl => m_Url;

        #endregion

        public override string ToString()
        {
            return $"Name:{m_Name}, Service:{m_Service}, Tag:{m_Tag}";
        }
    }
}
