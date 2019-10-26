using System.Drawing;

namespace HandyBoxApp.CustomComponents
{
    internal struct Border
    {
        //################################################################################
        #region Constructor

        public Border(Color color, int size)
        {
            Color = color;
            Size = size;
        }

        #endregion

        //################################################################################
        #region Properties

        public Color Color { get; set; }

        public int Size { get; set; }

        #endregion
    }
}
