namespace HandyBoxApp.EventArgs
{
    internal class SlideEventArgs : System.EventArgs
    {
        public SlideEventArgs(int slide, bool isSlide)
        {
            Slide = slide;
            IsSlide = isSlide;
        }

        public int Slide { get; }

        public bool IsSlide { get; }
    }
}
