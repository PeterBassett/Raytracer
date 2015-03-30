using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raytracer.Rendering.FileTypes
{
    class Bmp : IBmp
    {
        Colour[] m_pColours = null;
        public int Width = 0;
        public int Height = 0;

        public Bmp(int lWidth, int lHeight)
        {
            Init(lWidth, lHeight);
        }

        public Bmp(System.Drawing.Bitmap other)
        {            
            Init(other.Width, other.Height);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var col = other.GetPixel(x, y);
                    SetPixel(x, y, 
                        new Colour(col.R / 255.0,
                                   col.G / 255.0,
                                   col.B / 255.0));
                }
            }
        }

        public void Init(int lWidth, int lHeight)
        {
            Destroy();

            Width = lWidth;
            Height = lHeight;
            m_pColours = new Colour[lWidth * lHeight];
        }

        private void Destroy()
        {
            Width = 0;
            Height = 0;
            m_pColours = null;            
        }

        public void SetPixel(int lX, int lY, Colour colour)
        {
            m_pColours[(lY * Width) + lX] = colour;
        }

        public Colour GetPixel(int lX, int lY)
        {
            return m_pColours[(lY * Width) + lX];
        }
    }
}
