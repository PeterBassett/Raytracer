using System;

namespace Raytracer.Rendering.Core
{
    class Texture
    {
        private readonly IBmp _bmp;
        public Texture(IBmp bmp)
        {
            _bmp = bmp;
        }

        public Colour Sample(double u, double v)
        {
            //mirror the coordinates
            u = 1.0 - u;
            v = 1.0 - v;

            u = u * _bmp.Size.Width - 0.5;
            v = v * _bmp.Size.Height - 0.5;

            var x = (int)Math.Floor(u);
            var y = (int)Math.Floor(v);

            double uRatio = u - x;
            double vRatio = v - y;
            double uOpposite = 1 - uRatio;
            double vOpposite = 1 - vRatio;

            x = x % _bmp.Size.Width;
            y = y % _bmp.Size.Height;

            if (x < 0)
                x += _bmp.Size.Width;

            if (y < 0)
                y += _bmp.Size.Height;

            return (GetPixelSafe(x, y) * uOpposite + GetPixelSafe(x + 1, y) * uRatio) * vOpposite +
                    (GetPixelSafe(x, y + 1) * uOpposite + GetPixelSafe(x + 1, y + 1) * uRatio) * vRatio;
        }

        private Colour GetPixelSafe(int x, int y)
        {
            var maxX = _bmp.Size.Width - 1;
            var maxY = _bmp.Size.Height - 1;

            x = Math.Min(maxX, Math.Max(0, x));
            y = Math.Min(maxY, Math.Max(0, y));

            return _bmp.GetPixel(x, y);
        }
    }
}
