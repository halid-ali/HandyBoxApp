using HandyBoxApp.EventArgs;

using HtmlAgilityPack;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace HandyBoxApp.CurrencyService.Services
{
    internal class Yahoo : ICurrencyService
    {
        //################################################################################
        #region Fields

        private readonly string m_CurrencySourceUrl;
        private CurrencySummaryData m_PreviousSummary;

        private event EventHandler<CurrencyUpdatedEventArgs> CurrencyUpdated;

        #endregion

        //################################################################################
        #region Constructor

        public Yahoo(string currencySourceUrl)
        {
            m_CurrencySourceUrl = currencySourceUrl;
            m_PreviousSummary = new CurrencySummaryData();
        }

        #endregion

        //################################################################################
        #region ICurrencyService Members

        event EventHandler<CurrencyUpdatedEventArgs> ICurrencyService.OnCurrencyUpdated
        {
            add => CurrencyUpdated += value;
            remove => CurrencyUpdated -= value;
        }

        void ICurrencyService.GetUpdatedRateData()
        {
            try
            {
                //todo: implement your own business logic to read html data instead of using third party APIs
                var doc = new HtmlDocument();
                doc.LoadHtml(ReadHtmlData());

                var summaryData = ReadSummaryData(doc);

                var currencySummary = new CurrencySummaryData
                {
                    Actual = GetActualCurrency(doc),
                    PreviousClose = TryParseCurrencyText(summaryData["previous-close"], m_PreviousSummary.PreviousClose),
                    Open = TryParseCurrencyText(summaryData["open"], m_PreviousSummary.Open),
                    DayRangeLow = GetDayRangeLow(summaryData["day-range"]),
                    DayRangeHigh = GetDayRangeHigh(summaryData["day-range"]),
                    YearRangeLow = GetYearRangeLow(summaryData["year-range"]),
                    YearRangeHigh = GetYearRangeHigh(summaryData["year-range"])
                };

                m_PreviousSummary = currencySummary;
                OnCurrencyUpdated(currencySummary);
            }
            catch (Exception e)
            {
                //todo: handle exception and log data
                throw new Exception("Exception on rate fetching.", e);
            }
        }

        #endregion

        //################################################################################
        #region Event Callbacks

        protected virtual void OnCurrencyUpdated(CurrencySummaryData summaryData)
        {
            var args = new CurrencyUpdatedEventArgs { CurrencySummary = summaryData };

            CurrencyUpdated?.Invoke(this, args);
        }

        #endregion

        //################################################################################
        #region Private Members

        #region Html Data Readers

        private string ReadHtmlData()
        {
            string html = string.Empty;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(m_CurrencySourceUrl);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                    if (stream != null)
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            html = reader.ReadToEnd();
                        }
            }
            catch (Exception)
            {
                //todo: handle exception and log data
                return string.Empty;
            }

            return html;
        }

        private IDictionary<string, string> ReadSummaryData(HtmlDocument doc)
        {
            var summaryData = new Dictionary<string, string>
            {
                {"previous-close", string.Empty},
                {"open", string.Empty},
                {"day-range", string.Empty},
                {"year-range", string.Empty}
            };

            var divList = doc.DocumentNode.Descendants("div");

            foreach (var currentDiv in divList)
            {
                if (currentDiv.Attributes.Contains("id"))
                {
                    if (currentDiv.GetAttributeValue("id", string.Empty).Equals("quote-summary"))
                    {
                        HtmlNode[] tdList = currentDiv.Descendants("td").ToArray();
                        summaryData["previous-close"] = tdList[1].InnerText;
                        summaryData["open"] = tdList[3].InnerText;
                        summaryData["day-range"] = tdList[7].InnerText;
                        summaryData["year-range"] = tdList[9].InnerText;
                    }
                }
            }

            return summaryData;
        }

        #endregion

        #region Rate Summary Getters

        private double GetActualCurrency(HtmlDocument doc)
        {
            string currencyText = string.Empty;
            var divList = doc.DocumentNode.Descendants("div");

            foreach (var currentDiv in divList)
            {
                if (currentDiv.Attributes.Contains("id"))
                {
                    if (currentDiv.GetAttributeValue("id", string.Empty).Equals("quote-header-info"))
                    {
                        var spanList = currentDiv.Descendants("span");
                        foreach (var currentSpan in spanList)
                        {
                            if (currentSpan.GetAttributeValue("class", string.Empty).Contains("Trsdu(0.3s)"))
                            {
                                currencyText = currentSpan.InnerText;
                                if (!string.IsNullOrEmpty(currencyText))
                                    break;
                            }
                        }
                    }
                }
            }

            return TryParseCurrencyText(currencyText, m_PreviousSummary.Actual);
        }

        private double GetDayRangeLow(string currencyText)
        {
            return TryParseCurrencyText(TrySplitCurrencyText(currencyText, isHigh: false), m_PreviousSummary.DayRangeLow);
        }

        private double GetDayRangeHigh(string currencyText)
        {
            return TryParseCurrencyText(TrySplitCurrencyText(currencyText, isHigh: true), m_PreviousSummary.DayRangeHigh);
        }

        private double GetYearRangeLow(string currencyText)
        {
            return TryParseCurrencyText(TrySplitCurrencyText(currencyText, isHigh: false), m_PreviousSummary.YearRangeLow);
        }

        private double GetYearRangeHigh(string currencyText)
        {
            return TryParseCurrencyText(TrySplitCurrencyText(currencyText, isHigh: true), m_PreviousSummary.YearRangeHigh);
        }

        #endregion

        private string TrySplitCurrencyText(string currencyText, bool isHigh)
        {
            try
            {
                var index = isHigh ? 1 : 0;
                return currencyText.Split('-')[index].Trim();
            }
            catch (Exception)
            {
                //todo: handle exception and log data
                return string.Empty;
            }
        }

        private double TryParseCurrencyText(string currencyText, double previousValue)
        {
            double actualCurrency;

            try
            {
                actualCurrency = double.Parse(currencyText);
            }
            catch (Exception)
            {
                //todo: handle exception and log data
                actualCurrency = previousValue;
            }

            return actualCurrency;
        }

        #endregion
    }
}
