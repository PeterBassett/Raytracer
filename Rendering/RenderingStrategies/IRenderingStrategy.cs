using Raytracer.Rendering.Renderers;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.RenderingStrategies
{
    interface IRenderingStrategy
    {
        event ParallelOptionsBase.RenderingStarted OnRenderingStarted;
        event ParallelOptionsBase.CompletedPercentageDelta OnCompletedPercentageDelta;
        event ParallelOptionsBase.RenderingComplete OnRenderingComplete;

        void RenderScene(IRenderer renderer, IBmp frameBuffer);
    }
}
