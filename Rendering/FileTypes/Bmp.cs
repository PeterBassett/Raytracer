using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raytracer.Rendering.FileTypes
{
    class Bmp : IBmp
    {
        Colour[] m_pColours = null;
        private int _width = 0;
        private int _height = 0;

        public Bmp(int lWidth, int lHeight)
        {
            Init(lWidth, lHeight);
        }

        public Bmp(System.Drawing.Bitmap other)
        {            
            Init(other.Width, other.Height);

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    var col = other.GetPixel(x, y);
                    SetPixel(x, y, 
                        new Colour(col.R / 255.0,
                                   col.G / 255.0,
                                   col.B / 255.0));
                }
            }
        }

        public void Init(int width, int height)
        {
            Destroy();

            _width = width;
            _height = height;

            m_pColours = new Colour[width * height];
        }

        private void Destroy()
        {
            _width = 0;
            _height = 0;
            m_pColours = null;            
        }

        public void SetPixel(int lX, int lY, Colour colour)
        {
            m_pColours[(lY * _width) + lX] = colour;
        }

        public Colour GetPixel(int lX, int lY)
        {
            var c = m_pColours[(lY * _width) + lX];
            float r;
            if (c == new Colour(31f / 255, 34f / 255, 43f / 255))
                r = c.Red;

            return c;
        }

        public int Width
        {
            get { return _width; }
        }

        public int Height
        {
            get { return _height; }
        }
    }
}
