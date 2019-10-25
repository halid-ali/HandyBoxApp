using System.Security.Policy;

namespace HandyBoxApp.CurrencyService.Types
{
    internal struct EurUsdCurrency : ICurrencyService
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

        string ICurrencyService.Name => "EUR/USD";

        string ICurrencyService.Tag => "EURUSD";

        Url ICurrencyService.SourceUrl => m_SourceUrl;

        #endregion
    }
}
