using HandyBoxApp.StockExchange.EventArgs;
using HandyBoxApp.StockExchange.Interfaces;
using HandyBoxApp.StockExchange.Stock;

using HtmlAgilityPack;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace HandyBoxApp.StockExchange.StockService
{
    internal class YahooStockService : StockServiceBase, IStockService
    {
        //################################################################################
        #region IStockService Members

        event EventHandler<StockUpdateEventArgs> IStockService.StockUpdated
        {
            add => StockUpdate += value;
            remove => StockUpdate -= value;
        }

        void IStockService.GetStockData(IStockInfo stock)
        {
            var htmlDoc = ReadHtmlDocument(stock.SourceUrl);
            var parsedHtml = ParseHtmlDocument(htmlDoc);

            if (parsedHtml.Count != 2)
            {
                throw new HtmlWebException("Yahoo: Parsed html contains wrong node count.");
            }

            var actualData = GetActualData(parsedHtml[0]);
            var changeRate = GetChangeRate(parsedHtml[1]);

            var stockData = new StockData
            {
                ActualData = actualData,
                ChangeRate = changeRate
            };

            PreviousStockData = stockData;
            OnStockUpdate(stockData);
        }

        #endregion

        //################################################################################
        #region Private Members

        private List<HtmlNode> ParseHtmlDocument(HtmlDocument document)
        {
            var divList = document.DocumentNode.Descendants("div");

            foreach (var currentDiv in divList)
            {
                if (currentDiv.Attributes.Contains("id"))
                {
                    if (currentDiv.GetAttributeValue("id", string.Empty).Equals("quote-header-info"))
                    {
                        return currentDiv.Descendants("span")
                            .Where(span => span.GetAttributeValue("class", string.Empty).Contains("Trsdu(0.3s)"))
                            .ToList();
                    }
                }
            }

            throw new HtmlWebException("Yahoo: Stock data cannot be parsed.");
        }

        private double GetActualData(HtmlNode node)
        {
            double actualData;
            var actualDataText = node.InnerText;

            try
            {
                actualData = double.Parse(actualDataText);
            }
            catch (FormatException)
            {
                actualData = PreviousStockData.ActualData;
            }

            return actualData;
        }

        private double GetChangeRate(HtmlNode node)
        {
            double changeRate;
            var changeRateText = ParseChangeRate(node);

            try
            {
                changeRate = double.Parse(changeRateText);
            }
            catch (FormatException)
            {
                changeRate = PreviousStockData.ChangeRate;
            }

            return changeRate;
        }

        private static string ParseChangeRate(HtmlNode node)
        {
            var innerText = node.InnerText;
            var rateMatch = Regex.Match(innerText, @"(\d.\d*%)");
            var changeRate = Regex.Replace(rateMatch.Value, "%", "");

            return changeRate;
        }

        #endregion
    }
}
