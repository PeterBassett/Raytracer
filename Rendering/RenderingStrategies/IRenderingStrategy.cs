using Raytracer.Rendering.Renderers;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.RenderingStrategies
{
    interface IRenderingStrategy
    {        
        event ParallelOptionsBase.CompletedScanLine OnCompletedScanLine;

        void RenderScene(IRenderer renderer, IBmp frameBuffer);
    }
}
