using Raytracer.MathTypes;
using Raytracer.Rendering.Renderers;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.PixelSamplers
{
    class StandardPixelSampler : IPixelSampler
    {
        public void SamplePixel(IRenderer renderer, int x, int y, IBuffer buffer)
        {            
            buffer.AddSample(x, y, renderer.ComputeSample(new Vector2(x + 0.5f, y + 0.5f)));
        }

        public void Initialise()
        {
        }
    }
}
