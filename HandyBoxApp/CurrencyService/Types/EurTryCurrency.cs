using System.Security.Policy;

namespace HandyBoxApp.CurrencyService.Types
{
    internal struct EurTryCurrency : ICurrency
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

        string ICurrency.Name => "EUR/TRY";

        string ICurrency.Tag => "EURTRY";

        Url ICurrency.SourceUrl => m_SourceUrl;

        #endregion
    }
}
