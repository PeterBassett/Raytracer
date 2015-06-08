using System.Diagnostics;
using System.Threading.Tasks;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.PixelSamplers;
using Raytracer.Rendering.Renderers;
using Raytracer.Rendering.Synchronisation;
using System;

namespace Raytracer.Rendering.RenderingStrategies
{
    class GridRenderingStrategy : ParallelOptionsBase, IRenderingStrategy
    {
        private readonly IPixelSampler _pixelSampler;

        public GridRenderingStrategy(IPixelSampler pixelSampler, bool multiThreaded, CancellationToken cancellationToken)
            : base(multiThreaded, cancellationToken)
        {
            Debug.Assert(pixelSampler != null, "pixelSampler != null");
            _pixelSampler = pixelSampler;
        }

        public void RenderScene(IRenderer renderer, IBmp frameBuffer)
        {
            _pixelSampler.Initialise();

            int threads = Environment.ProcessorCount * 10;
            int dx = frameBuffer.Size.Width;
            int dy = frameBuffer.Size.Height;
            int nx = threads;
            int ny = 1;

            while ((nx & 0x1) == 0 && 2 * dx * ny < dy * nx)
            {
                nx >>= 1;
                ny <<= 1;
            }

            frameBuffer.BeginWriting();

            var options = new ParallelOptions() { MaxDegreeOfParallelism = _multiThreaded ? threads : 1 };
            
            Parallel.For(0, threads, options, (threadId, state) =>
            {
                var xStart = nx * threadId;
                var yStart = ny * threadId;

                for (int x = ; x < length; x++)
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
                }

                RaiseOnCompletedScanLine(x, frameBuffer.Size.Width);

            });

            frameBuffer.EndWriting();
        }
    }
}
