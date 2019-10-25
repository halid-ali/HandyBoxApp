using HandyBoxApp.EventArgs;

using System;

namespace HandyBoxApp.CurrencyService
{
    interface ICurrencyService
    {
        event EventHandler<CurrencyUpdatedEventArgs> OnCurrencyUpdated;

        void GetUpdatedRateData();
    }
}
