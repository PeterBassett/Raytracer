using System.Threading.Tasks;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.PixelSamplers;
using Raytracer.Rendering.Renderers;
using System.Linq;
using Raytracer.Rendering.FileTypes;
using System.Threading;

namespace Raytracer.Rendering.RenderingStrategies
{
    class ProgressiveRenderingStrategy : IRenderingStrategy
    {
        private IPixelSampler _pixelSampler;
        private bool _multiThreaded;
        private int _skip;
        private CancellationToken _cancellationToken;

        public ProgressiveRenderingStrategy(IPixelSampler pixelSampler, int initialSkip, bool multiThreaded, CancellationToken cancellationToken)
        {
            _pixelSampler = pixelSampler;
            _multiThreaded = multiThreaded;
            _skip = initialSkip;
            _cancellationToken = cancellationToken;
        }

        public void RenderScene(IRenderer renderer, IBmp frameBuffer)
        {
            var options = new ParallelOptions();
            if (!_multiThreaded)
                options.MaxDegreeOfParallelism = 1;           

            int skip = _skip;
           
            while (skip >= 1)
            {                
                var xs = Enumerable.Range(0, frameBuffer.Size.Width).Where(i => i % skip == 0).ToArray();

                frameBuffer.BeginWriting();

                Parallel.ForEach(xs, options, (x, state, i) =>
                {
                    for (int y = 0; y < frameBuffer.Size.Height; y += skip)
                    {
                        if (_cancellationToken.IsCancellationRequested)
                        {
                            state.Break();
                            return;
                        }

                        var colour = _pixelSampler.SamplePixel(renderer, x, y);

                        SetColourBlock(frameBuffer, skip, x, y, colour);
                    }
                });

                if(!_cancellationToken.IsCancellationRequested)
                    frameBuffer.EndWriting();

                if (_cancellationToken.IsCancellationRequested)
                    return;

                if (skip == 1)
                    skip = 0;
                else
                    skip = skip / 2; 
            }
        }

        private void SetColourBlock(IBmp frameBuffer, int skip, int x, int y, Colour colour)
        {
            for (int xs = x; xs < x + skip && xs < frameBuffer.Size.Width; xs++)
                for (int ys = y; ys < y + skip && ys < frameBuffer.Size.Height; ys++)
                {
                    frameBuffer.SetPixel(xs, ys, colour);
                }
        }
    }
}
