using Raytracer.FileTypes;
using Raytracer.Rendering.Renderers;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.PixelSamplers
{
    interface IPixelSampler
    {
        Colour SamplePixel(IRenderer renderer, int x, int y);
    }
}
