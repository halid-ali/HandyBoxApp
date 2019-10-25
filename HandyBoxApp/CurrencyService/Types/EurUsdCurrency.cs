using System.Security.Policy;

namespace HandyBoxApp.CurrencyService.Types
{
    internal struct EurUsdCurrency : ICurrency
    {
        //################################################################################
        #region Fields

        private readonly Url m_SourceUrl;

        #endregion

        //################################################################################
        #region Constructor

        public EurUsdCurrency(string urlLink)
        {
            m_SourceUrl = new Url(urlLink);
        }

        #endregion

        //################################################################################
        #region ICurrencyService Members

        string ICurrency.Name => "EUR/USD";

        string ICurrency.Tag => "EURUSD";

        Url ICurrency.SourceUrl => m_SourceUrl;

        #endregion
    }
}
