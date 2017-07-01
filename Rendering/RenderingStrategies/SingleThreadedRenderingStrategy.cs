using System.Diagnostics;
using System.Threading.Tasks;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.PixelSamplers;
using Raytracer.Rendering.Renderers;
using Raytracer.Rendering.Synchronisation;

namespace Raytracer.Rendering.RenderingStrategies
{
    class SingleThreadedRenderingStrategy : ParallelOptionsBase, IRenderingStrategy
    {
        private readonly IPixelSampler _pixelSampler;

        public SingleThreadedRenderingStrategy(IPixelSampler pixelSampler, CancellationToken cancellationToken)
            : base(false, cancellationToken)
        {
            _pixelSampler = pixelSampler;
        }

        public void RenderScene(IRenderer renderer, Buffer frameBuffer)
        {
            _pixelSampler.Initialise();

            var options = GetThreadingOptions();

            RaiseRenderingStarted();
            frameBuffer.BeginWriting();
            for (int x = 0; x < frameBuffer.Size.Width; x++)
            {
                if (_cancellationToken.IsCancellationRequested)
                    break;

                for (int y = 0; y < frameBuffer.Size.Height; y++)
                {
                    if (_cancellationToken.IsCancellationRequested)
                        break;

                    _pixelSampler.SamplePixel(renderer, x, y, frameBuffer);
                }

                RaiseOnCompletedPercentageDelta(frameBuffer.Size.Height / (double)(frameBuffer.Size.Width * frameBuffer.Size.Height) * 100.0);
            }

            frameBuffer.EndWriting();
            RaiseRenderingComplete();
        }
    }
}
