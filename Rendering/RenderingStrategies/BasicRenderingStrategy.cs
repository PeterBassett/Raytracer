using System.Diagnostics;
using System.Threading.Tasks;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.PixelSamplers;
using Raytracer.Rendering.Renderers;
using System.Threading;

namespace Raytracer.Rendering.RenderingStrategies
{
    class BasicRenderingStrategy : ParallelOptionsBase, IRenderingStrategy
    {
        private readonly IPixelSampler _pixelSampler;

        public BasicRenderingStrategy(IPixelSampler pixelSampler, bool multiThreaded, CancellationToken cancellationToken)
            : base(multiThreaded, cancellationToken)
        {
            Debug.Assert(pixelSampler != null, "pixelSampler != null");
            _pixelSampler = pixelSampler;
        }

        public void RenderScene(IRenderer renderer, IBmp frameBuffer)
        {
            var options = GetThreadingOptions();
            
            frameBuffer.BeginWriting();
            Parallel.For(0, frameBuffer.Size.Width, options, (x, state) =>
            {
                for (int y = 0; y < frameBuffer.Size.Height; y++)
                {
                    if (_cancellationToken.IsCancellationRequested)
                    {
                        state.Break();
                        return;
                    }

                    frameBuffer.SetPixel(x, y, _pixelSampler.SamplePixel(renderer, x, y));
                }

                RaiseOnCompletedScanLine(x, frameBuffer.Size.Width);
            });
            frameBuffer.EndWriting();
        }
    }
}
