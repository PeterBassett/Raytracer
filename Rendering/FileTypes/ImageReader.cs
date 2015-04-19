using System;
using System.Drawing;
using System.Drawing.Imaging;
using Raytracer.Rendering.Core;
using Size = Raytracer.MathTypes.Size;

namespace Raytracer.Rendering.FileTypes
{
    static class ImageReader
    {
        public static Bmp Read(System.Drawing.Bitmap other)
        {            
            var bmp = new Bmp(other.Width, other.Height);

            ReadImage(other, bmp);

            return bmp;
        }

        private static void ReadImage(System.Drawing.Bitmap other, Bmp bmp)
        {
            var reader = GetPixelReader(other);

            if (reader != null)
                ReadImageFast(bmp, other);
            else
                ReadSlowFallback(bmp, other);
        }

        private static void ReadImageFast(Bmp bmp, System.Drawing.Bitmap other)
        {
            var depth = Image.GetPixelFormatSize(other.PixelFormat);
            var components = depth / 8;

            var data = other.LockBits(new Rectangle(0, 0, other.Width, other.Height), ImageLockMode.ReadWrite, other.PixelFormat);
            
            unsafe
            {
                var getPixel = GetPixelReader(other);
                
                for (int y = 0; y < data.Height; y++)
                    for (int x = 0; x < data.Width; x++)
                        bmp.SetPixel(x, y, getPixel(data, components, x, y));
            }
           
            other.UnlockBits(data);
        }

        private static void ReadSlowFallback(Bmp bmp, System.Drawing.Bitmap other)
        {
            for (int x = 0; x < bmp.Size.Width; x++)
            {
                for (int y = 0; y < bmp.Size.Height; y++)
                {
                    var col = other.GetPixel(x, y);

                    bmp.SetPixel(x, y,
                        new Colour(col.R / 255.0,
                                   col.G / 255.0,
                                   col.B / 255.0));
                }
            }
        }

        private static unsafe Func<BitmapData, int, int, int, Colour> GetPixelReader(Bitmap data)
        {
            switch (data.PixelFormat)
            {
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format24bppRgb:
                    return GetPixel32and24;
                case PixelFormat.Format8bppIndexed:
                    return (BitmapData bmpData, int components, int x, int y) => GetPixel8BppIndexed(bmpData, components, x, y, data);
                default:
                    throw new ArgumentOutOfRangeException("depth is not supported");
            }
        }

        private static unsafe Colour GetPixel32and24(BitmapData bmpData, int components, int x, int y)
        {
            byte* row = (byte*)bmpData.Scan0 + (y * bmpData.Stride);
            int index = (x * components);

            return new Colour(row[index + 2] / 255.0,
                              row[index + 1] / 255.0,
                              row[index] / 255.0);
        }

        private static unsafe Colour GetPixel8BppIndexed(BitmapData bmpData, int components, int x, int y, Bitmap data)
        {
            byte* row = (byte*)bmpData.Scan0 + (y * bmpData.Stride);
            int index = row[x];
            var col = data.Palette.Entries[index];
            return new Colour(col.R / 255.0, col.G / 255.0, col.B / 255.0);        
        }
    }
}
