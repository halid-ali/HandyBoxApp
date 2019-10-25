namespace HandyBoxApp.CurrencyService
{
    internal struct CurrencySummaryData
    {
        public double Actual { get; set; }

        public double PreviousClose { get; set; }

        public double Open { get; set; }

        public double DayRangeLow { get; set; }

        public double DayRangeHigh { get; set; }

        public double YearRangeLow { get; set; }

        public double YearRangeHigh { get; set; }
    }
}
