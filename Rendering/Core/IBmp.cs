using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.Rendering.FileTypes;
using Raytracer.MathTypes;

namespace Raytracer.Rendering.Core
{
    interface IBmp
    {        
        Colour GetPixel(int lX, int lY);
        void SetPixel(int lX, int lY, Colour colour);
        Size Size { get; }
    }
}
