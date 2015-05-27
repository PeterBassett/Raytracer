using System.Diagnostics;
using System.Threading.Tasks;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.PixelSamplers;
using Raytracer.Rendering.Renderers;
using System.Linq;
using Raytracer.Rendering.Synchronisation;

namespace Raytracer.Rendering.RenderingStrategies
{
    class RowRenderingStrategy : ParallelOptionsBase, IRenderingStrategy
    {
        private readonly IPixelSampler _pixelSampler;
        
        public RowRenderingStrategy(IPixelSampler pixelSampler, bool multiThreaded, CancellationToken cancellationToken)
            : base(multiThreaded, cancellationToken)
        {
            _pixelSampler = pixelSampler;   
        }

        public void RenderScene(IRenderer renderer, IBmp frameBuffer)
        {
            var options = GetThreadingOptions();         
         
            for (int x = 0; x < frameBuffer.Size.Width; x++)
			{
                frameBuffer.BeginWriting();

                Parallel.For(0, frameBuffer.Size.Height, (y, state) =>
                {               
                    if (_cancellationToken.IsCancellationRequested)
                    {
                        state.Break();
                        return;
                    }

                    var colour = _pixelSampler.SamplePixel(renderer, x, y);

                    frameBuffer.SetPixel(x, y, colour);
                });

                RaiseOnCompletedScanLine(x, frameBuffer.Size.Width);

                frameBuffer.EndWriting();
            }
        }
    }
}
