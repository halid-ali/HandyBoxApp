namespace HandyBoxApp.ColorScheme
{
    internal abstract class ColorBase
    {
        internal abstract ColorSet Light { get; }

        internal abstract ColorSet Normal { get; }

        internal abstract ColorSet Dark { get; }
    }
}
