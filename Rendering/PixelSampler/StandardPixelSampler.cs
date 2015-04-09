using Raytracer.Rendering.FileTypes;
using Raytracer.MathTypes;
using Raytracer.Rendering.Renderers;

namespace Raytracer.Rendering.PixelSamplers
{
    class StandardPixelSampler : IPixelSampler
    {
        public Colour SamplePixel(IRenderer renderer, int x, int y)
        {            
            return renderer.ComputeSample(new Vector2(x + 0.5f, y + 0.5f));
        }
    }
}
