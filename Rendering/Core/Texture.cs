using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.Core
{
    class Texture
    {
        private IBmp _bmp;
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

            int x = (int)Math.Floor(u);
            int y = (int)Math.Floor(v);

            double u_ratio = u - x;
            double v_ratio = v - y;
            double u_opposite = 1 - u_ratio;
            double v_opposite = 1 - v_ratio;

            x = x % _bmp.Size.Width;
            y = y % _bmp.Size.Height;

            if (x < 0)
                x += _bmp.Size.Width;

            if (y < 0)
                y += _bmp.Size.Height;

            return (GetPixelSafe(x, y) * u_opposite + GetPixelSafe(x + 1, y) * u_ratio) * v_opposite +
                    (GetPixelSafe(x, y + 1) * u_opposite + GetPixelSafe(x + 1, y + 1) * u_ratio) * v_ratio;
        }

        private Colour GetPixelSafe(int x, int y)
        {
            int maxX = _bmp.Size.Width - 1;
            int maxY = _bmp.Size.Height - 1;

            x = Math.Min(maxX, Math.Max(0, x));
            y = Math.Min(maxY, Math.Max(0, y));

            return _bmp.GetPixel(x, y);
        }
    }
}
