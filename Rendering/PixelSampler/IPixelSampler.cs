using Raytracer.Rendering.FileTypes;
using Raytracer.Rendering.Renderers;

namespace Raytracer.Rendering.PixelSamplers
{
    interface IPixelSampler
    {
        Colour SamplePixel(IRenderer renderer, int x, int y);
    }
}
