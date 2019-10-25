using System.Security.Policy;

namespace HandyBoxApp.CurrencyService.Types
{
    internal struct UsdEurCurrency : ICurrencyService
    {
        //################################################################################
        #region Fields

        private readonly Url m_SourceUrl;

        #endregion

        //################################################################################
        #region Constructor

        public UsdEurCurrency(string urlLink)
        {
            m_SourceUrl = new Url(urlLink);
        }

        #endregion

        //################################################################################
        #region ICurrencyService Members

        string ICurrencyService.Name => "USD/TRY";

        string ICurrencyService.Tag => "USDTRY";

        Url ICurrencyService.SourceUrl => m_SourceUrl;

        #endregion
    }
}
