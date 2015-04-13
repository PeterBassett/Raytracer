using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.FileTypes
{
    class Bmp : IBmp
    {
        Colour[] m_pColours = null;
        private Size _size;

        public Bmp(int lWidth, int lHeight)
        {
            Init(lWidth, lHeight);
        }

        public Bmp(System.Drawing.Bitmap other)
        {            
            Init(other.Width, other.Height);

            for (int x = 0; x < _size.Width; x++)
            {
                for (int y = 0; y < _size.Height; y++)
                {
                    var col = other.GetPixel(x, y);
                    SetPixel(x, y, 
                        new Colour(col.R / 255.0,
                                   col.G / 255.0,
                                   col.B / 255.0));
                }
            }
        }

        private void Init(int width, int height)
        {
            Destroy();

            _size = new Size(width, height);

            m_pColours = new Colour[_size.Width * _size.Height];
        }

        private void Destroy()
        {
            _size = new Size(0, 0);
            m_pColours = null;
        }

        public void SetPixel(int lX, int lY, Colour colour)
        {
            m_pColours[(lY * _size.Width) + lX] = colour;
        }

        public Colour GetPixel(int lX, int lY)
        {
            return m_pColours[(lY * _size.Width) + lX];
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
