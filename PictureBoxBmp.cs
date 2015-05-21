using System;
using System.Drawing;
using System.Windows.Forms;
using Raytracer.Extensions;
using Raytracer.Rendering.Core;
using Raytracer.FileTypes;

namespace Raytracer
{
    class PictureBoxBmp : IBmp, IDisposable
    {
        Bitmap _bitmap = null;
        readonly PictureBox _pictureBox = null;
        System.Drawing.Imaging.BitmapData _bmpdata = null;
        readonly Raytracer.MathTypes.Size _size;

        public PictureBoxBmp(PictureBox box)
        {
            if (box == null)
                throw new ArgumentNullException();

            _pictureBox = box;

            _size = new MathTypes.Size(box.Width, box.Height);
        }

        public Colour GetPixel(int lX, int lY)
        {
            unsafe
            {
                byte* row = (byte*)_bmpdata.Scan0 + ((_bmpdata.Height - lY - 1) * _bmpdata.Stride);

                return new Colour(row[(lX * 4) + 2] / 255.0, row[(lX * 4) + 1] / 255.0, row[lX * 4] / 255.0);
            }
        }

        public void SetPixel(int lX, int lY, Colour colour)
        {
            var col = colour.ToColor();
            unsafe
            {
                byte* row = (byte*)_bmpdata.Scan0 + ((_bmpdata.Height - lY - 1) * _bmpdata.Stride);

                row[lX * 4] = col.B;  // B
                row[(lX * 4) + 1] = col.G;  // G
                row[(lX * 4) + 2] = col.R;  // R
                row[(lX * 4) + 3] = 255;    // A
            }
        }

        public void BeginWriting()
        {
            _pictureBox.UIThread(() =>
            {
                _bitmap = new Bitmap(_pictureBox.Width, _pictureBox.Height);
            });

            _bmpdata = _bitmap.LockBits(new Rectangle(0, 0, _bitmap.Width, _bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadWrite, _bitmap.PixelFormat);
        }

        public void EndWriting()
        {
            if (_bitmap == null || _bmpdata == null)
                return;

            _bitmap.UnlockBits(_bmpdata);

            _pictureBox.UIThread(() =>
            {
                _pictureBox.Image = _bitmap;
            });

            _bitmap = null;
            _bmpdata = null;
        }

        public void Dispose()
        {
            if (_bmpdata != null)
                EndWriting();

            if (_bitmap != null)
            {
                _bitmap.Dispose();
                _bitmap = null;
            }
        }

        public MathTypes.Size Size
        {
            get { return _size; }
        }
    }
}
