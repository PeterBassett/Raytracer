using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.Rendering.FileTypes;
using System.Windows.Forms;
using System.Drawing;
using Raytracer.Extensions;
using Raytracer.Rendering;

namespace Raytracer
{
    class PictureBoxBmp : IBmp, IDisposable
    {
        Bitmap bitmap = null;
        PictureBox pictureBox = null;
        System.Drawing.Imaging.BitmapData bmpdata = null;

        public PictureBoxBmp(PictureBox box)
        {
            if (box == null)
                throw new ArgumentNullException();

            pictureBox = box;           
        }

        public void Init(int width, int height)
        {
            pictureBox.UIThread(() =>
            {
                pictureBox.Width = width;
                pictureBox.Height = height;
                bitmap = new Bitmap(pictureBox.Width, pictureBox.Height);
            });
            

            bmpdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadWrite, bitmap.PixelFormat);
        }

        public Colour GetPixel(int lX, int lY)
        {
            throw new NotImplementedException();
        }

        public void SetPixel(int lX, int lY, Colour colour)
        {
            var col = colour.ToColor();
            unsafe
            {
                byte* row = (byte*)bmpdata.Scan0 + ((bmpdata.Height - lY - 1) * bmpdata.Stride);

                row[lX * 4] =       col.B;  // B
                row[(lX * 4) + 1] = col.G;  // G
                row[(lX * 4) + 2] = col.R;  // R
                row[(lX * 4) + 3] = 255;    // A
            }
        }

        public void Render()
        {
            bitmap.UnlockBits(bmpdata);

            pictureBox.UIThread(() =>
            {
                pictureBox.Image = bitmap;
            });
        }

        public void Dispose()
        {
            if (bitmap != null)
            {
                bitmap.UnlockBits(bmpdata);

                bitmap.Dispose();
                bitmap = null;
            }
        }
    }
}
