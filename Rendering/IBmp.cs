using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.Rendering.FileTypes;

namespace Raytracer.Rendering
{
    interface IBmp
    {
        void Init(int width, int height);
        Colour GetPixel(int lX, int lY);
        void SetPixel(int lX, int lY, Colour colour);
    }
}
