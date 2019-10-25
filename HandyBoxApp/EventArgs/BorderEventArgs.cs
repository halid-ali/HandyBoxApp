using System.Drawing;

namespace HandyBoxApp.EventArgs
{
    internal class BorderEventArgs : System.EventArgs
    {
        public BorderEventArgs(Color color, int size)
        {
            Color = color;
            Size = size;
        }

        public Color Color { get; }

        public int Size { get; }
    }
}
