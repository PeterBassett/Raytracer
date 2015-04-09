using Raytracer.MathTypes;
using Raytracer.Rendering.FileTypes;

namespace Raytracer.Rendering.Renderers
{
    interface IRenderer
    {
        void RenderScene(IBmp frameBuffer);
        Colour ComputeSample(Vector2 pixelCoordinate);
    }
}
