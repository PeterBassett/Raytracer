using System;
using System.Drawing;
using System.Drawing.Imaging;
using Raytracer.Rendering.Core;
using Size = Raytracer.MathTypes.Size;

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

            //ReadImage(other);
            
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

        private void ReadImage(System.Drawing.Bitmap other)
        {
            // get source bitmap pixel format size
            var depth = Image.GetPixelFormatSize(other.PixelFormat);

            // Check if bpp (Bits Per Pixel) is 8, 24, or 32
            if (depth != 8 && depth != 24 && depth != 32)
                throw new ArgumentException("Only 8, 24 and 32 bpp images are supported.");            

            var data = other.LockBits(new Rectangle(0, 0, other.Width, other.Height), ImageLockMode.ReadWrite, other.PixelFormat);
            
            unsafe
            {
                //var getPixel = GetPixelReader(depth);

                for (int y = 0; y < data.Height; ++y)
                {
                    for (int x = 0; x < data.Width; ++x)
                    {
                        SetPixel(x, y, GetPixel((byte*)data.Scan0, depth, x, y));
                    }
                }                
            }
           
            other.UnlockBits(data);
        }
        /*
        private unsafe Func<byte*, int, int, Colour> GetPixelReader(int depth)
        {
            switch (depth)
            {
                case 32:
                return GetPixel32;
                case 24:
                return GetPixel24;
                case 8:
                return GetPixel8;
                default:
            }
        }*/
        
        private unsafe Colour GetPixel(byte* data, int depth, int x, int y)
        {
            // Get color components count
            int cCount = depth / 8;

            // Get start index of the specified pixel
            int i = ((y * this.Size.Width) + x) * cCount;

            switch (depth)
            {
                case 32: // For 32 bpp get Red, Green, Blue and Alpha
                {
                    var b = data[i];
                    var g = data[i + 1];
                    var r = data[i + 2];
                        
                    return new Colour(r/255.0, g/255.0, b/255.0);
                }
                case 24: // For 24 bpp get Red, Green and Blue
                {
                    var b = data[i];
                    var g = data[i + 1];
                    var r = data[i + 2];
                    return new Colour(r/255.0, g/255.0, b/255.0);
                }
                case 8: // For 8 bpp get color value (Red, Green and Blue values are the same)
                {
                    var c = data[i];
                    return new Colour(c / 255f);
                }
                default:
                    throw new ArgumentOutOfRangeException("depth");
            }
        }
        /*
        private unsafe Colour GetPixel32(byte* data, int x, int y)
        {
            // Get start index of the specified pixel
            int i = ((y * this.Size.Width) + x) * 4;

            var b = data[i];
            var g = data[i + 1];
            var r = data[i + 2];

            return new Colour(r / 255.0, g / 255.0, b / 255.0);
        }

        private unsafe Colour GetPixel24(byte* data, int x, int y)
        {
            // Get start index of the specified pixel
            int i = ((y * this.Size.Width) + x) * 3;

            var b = data[i];
            var g = data[i + 1];
            var r = data[i + 2];
            return new Colour(r / 255.0, g / 255.0, b / 255.0);
        }
        private unsafe Colour GetPixel8(byte* data, int x, int y)
        {
            // Get start index of the specified pixel
            int i = ((y * this.Size.Width) + x);

            var c = data[i];
            return new Colour(c / 255f);
        }
        */

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
