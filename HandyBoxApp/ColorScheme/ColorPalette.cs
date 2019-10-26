namespace HandyBoxApp.ColorScheme
{
    internal class ColorPalette<T> where T : ColorBase, new()
    {
        internal ColorSet Normal => new T().Normal;

        internal ColorSet Light => new T().Light;

        internal ColorSet Dark => new T().Dark;
    }
}
