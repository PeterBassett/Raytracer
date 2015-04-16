using Raytracer.Rendering.Renderers;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.RenderingStrategies
{
    interface IRenderingStrategy
    {
        void RenderScene(IRenderer renderer, IBmp frameBuffer);
    }
}
