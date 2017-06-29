using Raytracer.Rendering.Renderers;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.PixelSamplers
{
    interface IPixelSampler
    {
        void Initialise();
        void SamplePixel(IRenderer renderer, int x, int y, Buffer buffer);        
    }
}
