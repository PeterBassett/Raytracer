using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.Rendering.FileTypes;

namespace Raytracer.Rendering.PixelSamplers
{
    interface IPixelSampler
    {
        Colour SamplePixel(Renderer renderer, int x, int y);
    }
}
