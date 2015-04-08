using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.Rendering.FileTypes;
using Raytracer.MathTypes;

namespace Raytracer.Rendering.PixelSamplers
{
    class StandardPixelSampler : IPixelSampler
    {
        public Colour SamplePixel(Renderer renderer, int x, int y)
        {            
            return renderer.ComputeSample(new Vector2(x + 0.5f, y + 0.5f));
        }
    }
}
