using HandyBoxApp.EventArgs;

using System;

namespace HandyBoxApp.CurrencyService
{
    internal delegate void CurrencyUpdateCallback(object sender, CurrencyUpdatedEventArgs args);

    interface ICurrencyService
    {
        event EventHandler<CurrencyUpdatedEventArgs> OnCurrencyUpdated;

        CurrencySummaryData PreviousCurrencyData { get; set; }

        void GetUpdatedRateData();
    }
}
