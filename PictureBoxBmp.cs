using System;
using System.Drawing;
using System.Windows.Forms;
using Raytracer.Extensions;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.FileTypes;

namespace Raytracer
{
    class PictureBoxBmp : IBmp, IDisposable
    {
        Bitmap bitmap = null;
        PictureBox pictureBox = null;
        System.Drawing.Imaging.BitmapData bmpdata = null;
        Raytracer.MathTypes.Size _size;

        public PictureBoxBmp(PictureBox box)
        {
            if (box == null)
                throw new ArgumentNullException();

            pictureBox = box;

            _size = new Raytracer.MathTypes.Size(box.Width, box.Height);
        }

        public Colour GetPixel(int lX, int lY)
        {
            unsafe
            {
                byte* row = (byte*)bmpdata.Scan0 + ((bmpdata.Height - lY - 1) * bmpdata.Stride);

                return new Colour(row[(lX * 4) + 2] / 255.0, row[(lX * 4) + 1] / 255.0, row[lX * 4] / 255.0);
            }
        }

        public void SetPixel(int lX, int lY, Colour colour)
        {
            var col = colour.ToColor();
            unsafe
            {
                byte* row = (byte*)bmpdata.Scan0 + ((bmpdata.Height - lY - 1) * bmpdata.Stride);

                row[lX * 4] = col.B;  // B
                row[(lX * 4) + 1] = col.G;  // G
                row[(lX * 4) + 2] = col.R;  // R
                row[(lX * 4) + 3] = 255;    // A
            }
        }

        public void BeginWriting()
        {
            pictureBox.UIThread(() =>
            {
                bitmap = new Bitmap(pictureBox.Width, pictureBox.Height);
            });

            bmpdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadWrite, bitmap.PixelFormat);
        }

        public void EndWriting()
        {
            if (bitmap == null || bmpdata == null)
                return;

            bitmap.UnlockBits(bmpdata);

            pictureBox.UIThread(() =>
            {
                pictureBox.Image = bitmap;
            });

            bitmap = null;
            bmpdata = null;
        }

        public void Dispose()
        {
            if (bmpdata != null)
                EndWriting();

            if (bitmap != null)
            {
                bitmap.Dispose();
                bitmap = null;
            }
        }

        public Raytracer.MathTypes.Size Size
        {
            get { return _size; }
        }
    }
}
