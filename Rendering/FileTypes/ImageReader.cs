using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;

namespace Raytracer.Rendering.FileTypes
{
    static class ImageReader
    {
        public static Bmp Read(string path)
        {
            if (path.EndsWith(".tga", StringComparison.InvariantCultureIgnoreCase))
            {
                using (var bmp = Paloma.TargaImage.LoadTargaImage(path))
                    return Read(bmp);
            }
            else
            {
                using (var bmp = new Bitmap(path))
                    return Read(bmp);
            }
        }

        private delegate Colour ReadPixel(BitmapData bmpData, int components, int x, int y);
        public static Bmp Read(Bitmap other)
        {            
            var bmp = new Bmp(other.Width, other.Height);

            ReadImage(other, bmp);

            return bmp;
        }

        private static void ReadImage(Bitmap other, Bmp bmp)
        {
            var reader = GetPixelReader(other);

            if (reader != null)
                ReadImageFast(reader, bmp, other);
            else
                ReadSlowFallback(bmp, other);
        }

        private static void ReadImageFast(ReadPixel getPixel, Bmp bmp, Bitmap other)
        {
            var depth = Image.GetPixelFormatSize(other.PixelFormat);
            var components = depth / 8;

            var data = other.LockBits(new Rectangle(0, 0, other.Width, other.Height), ImageLockMode.ReadWrite, other.PixelFormat);

            try
            {
                Parallel.For(0, data.Height, y =>
                {
                    for (var x = 0; x < data.Width; x++)
                        bmp.SetPixel(x, y, getPixel(data, components, x, y));
                });
            }
            finally
            {
                other.UnlockBits(data);
            }
        }

        private static void ReadSlowFallback(Bmp bmp, Bitmap other)
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

        private static ReadPixel GetPixelReader(Bitmap data)
        {            
            switch (data.PixelFormat)
            {
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format24bppRgb:
                    return GetPixel32And24;
                case PixelFormat.Format8bppIndexed:
                {
                    //store the palette here so we can use it in multiple threads via the lambda below.
                    var palette = data.Palette.Entries.ToArray();

                    // ReSharper disable once RedundantLambdaParameterType
                    return (BitmapData bmpData, int components, int x, int y) =>
                                GetPixel8BppIndexed(bmpData, components, x, y, palette);
                }
                default:
                    return null; // We do not support the specified PixelFormat.
            }
        }

        private static unsafe Colour GetPixel32And24(BitmapData bmpData, int components, int x, int y)
        {
            byte* row = (byte*)bmpData.Scan0 + (y * bmpData.Stride);
            int index = (x * components);

            return new Colour(row[index + 2] / 255.0,
                              row[index + 1] / 255.0,
                              row[index] / 255.0);
        }

        private static unsafe Colour GetPixel8BppIndexed(BitmapData bmpData, int components, int x, int y, Color [] palette)
        {
            byte* row = (byte*)bmpData.Scan0 + (y * bmpData.Stride);
            int index = row[x];
            var col = palette[index];
            return new Colour(col.R / 255.0, col.G / 255.0, col.B / 255.0);        
        }
    }
}
