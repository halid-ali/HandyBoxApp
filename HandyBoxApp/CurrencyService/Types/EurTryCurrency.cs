using System.Security.Policy;

namespace HandyBoxApp.CurrencyService.Types
{
    internal struct EurTryCurrency : ICurrencyService
    {
        //################################################################################
        #region Fields

        private readonly Url m_SourceUrl;

        #endregion

        //################################################################################
        #region Constructor

        public EurTryCurrency(string urlLink)
        {
            m_SourceUrl = new Url(urlLink);
        }

        #endregion

        //################################################################################
        #region ICurrencyService Members

        string ICurrencyService.Name => "EUR/TRY";

        string ICurrencyService.Tag => "EURTRY";

        Url ICurrencyService.SourceUrl => m_SourceUrl;

        #endregion
    }
}
