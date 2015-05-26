using Size = Raytracer.MathTypes.Size;

namespace Raytracer.Rendering.Core
{
    class Bmp : IBmp
    {
        Colour[] _colours;
        private Size _size;

        public Bmp(int lWidth, int lHeight)
        {
            Init(lWidth, lHeight);
        }

        private void Init(int width, int height)
        {
            Destroy();

            _size = new Size(width, height);

            _colours = new Colour[_size.Width * _size.Height];
        }

        private void Destroy()
        {
            _size = new Size(0, 0);
            _colours = null;
        }

        public void SetPixel(int lX, int lY, Colour colour)
        {
            _colours[(lY * _size.Width) + lX] = colour;
        }

        public Colour GetPixel(int lX, int lY)
        {
            return _colours[(lY * _size.Width) + lX];
        }

        public Size Size
        {
            get { return _size; }
        }

        public void BeginWriting()
        {
        }

        public void EndWriting()
        {
        }
    }
}
