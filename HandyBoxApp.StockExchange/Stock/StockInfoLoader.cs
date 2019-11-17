using HandyBoxApp.StockExchange.Interfaces;

using System;
using System.Collections.Generic;
using System.Xml;

namespace HandyBoxApp.StockExchange.Stock
{
    internal class StockInfoLoader
    {
        //################################################################################
        #region Fields

        private IList<IStockInfo> m_StockList;

        #endregion

        //################################################################################
        #region Public Members

        public IEnumerable<IStockInfo> GetStockList()
        {
            if (m_StockList == null)
            {
                m_StockList = new List<IStockInfo>();
                LoadStocks();
            }

            return m_StockList;
        }

        #endregion

        //################################################################################
        #region Private Members

        private void LoadStocks()
        {
            var xmlDoc = LoadXmlDocument();
            var serviceList = GetServiceList(xmlDoc);

            ReadStocks(serviceList);
        }

        private XmlDocument LoadXmlDocument()
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(GetType().Assembly.GetManifestResourceStream("HandyTool.Stock.StockServices.xml") ?? throw new InvalidOperationException());

            return xmlDocument;
        }

        private XmlNodeList GetServiceList(XmlDocument xmlDoc)
        {
            var stockServices = xmlDoc.GetElementsByTagName(XmlConstants.ServiceListTag);
            return stockServices[0].ChildNodes;
        }

        private void ReadStocks(XmlNodeList serviceList)
        {
            foreach (XmlNode stockService in serviceList)
            {
                var serviceNodeAttributes = stockService.Attributes;
                var serviceName = serviceNodeAttributes?.GetNamedItem(XmlConstants.NameAttribute).Value;

                foreach (XmlNode stockItem in stockService.ChildNodes)
                {
                    var stockNodeAttributes = stockItem.Attributes;
                    var stockName = stockNodeAttributes?.GetNamedItem(XmlConstants.NameAttribute).Value;
                    var stockTag = stockNodeAttributes?.GetNamedItem(XmlConstants.TagAttribute).Value;
                    var stockUrl = stockNodeAttributes?.GetNamedItem(XmlConstants.UrlAttribute).Value;

                    var stock = new StockInfo(stockName, serviceName, stockTag, stockUrl);
                    m_StockList.Add(stock);
                }
            }
        }

        #endregion
    }
}
