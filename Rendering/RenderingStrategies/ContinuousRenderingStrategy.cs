using System.Diagnostics;
using System.Threading.Tasks;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.PixelSamplers;
using Raytracer.Rendering.Renderers;
using Raytracer.Rendering.Synchronisation;

namespace Raytracer.Rendering.RenderingStrategies
{
    class ContinuousRenderingStrategy : ParallelOptionsBase, IRenderingStrategy
    {
        //private readonly IPixelSampler _pixelSampler;
        private readonly IRenderingStrategy _renderingStrategy;
        private double _cumulativePercentageComplete;

        public ContinuousRenderingStrategy(IRenderingStrategy renderingStrategy, bool multiThreaded, CancellationToken cancellationToken)
            : base(multiThreaded, cancellationToken)
        {
            //_pixelSampler = pixelSampler;
            _renderingStrategy = renderingStrategy;

            _cumulativePercentageComplete = 0;

            _renderingStrategy.OnCompletedPercentageDelta += (double d) =>
                {
                    _cumulativePercentageComplete += d;
                    this.RaiseOnCompletedPercentageDelta(d);
                };            
        }

        public void RenderScene(IRenderer renderer, Buffer frameBuffer)
        {
            RaiseRenderingStarted();
            while (!_cancellationToken.IsCancellationRequested)
                _renderingStrategy.RenderScene(renderer, frameBuffer);
            RaiseRenderingComplete();
            /*
            _pixelSampler.Initialise();

            var options = GetThreadingOptions();

            RaiseRenderingStarted();
            frameBuffer.BeginWriting();
            while (!_cancellationToken.IsCancellationRequested)
            {
                Parallel.For(0, frameBuffer.Size.Width, options, (x, state) =>
                {
                    for (int y = 0; y < frameBuffer.Size.Height; y++)
                    {
                        if (_cancellationToken.IsCancellationRequested)
                        {
                            state.Break();
                            return;
                        }

                        _pixelSampler.SamplePixel(renderer, x, y, frameBuffer);
                    }

                    RaiseOnCompletedPercentageDelta(frameBuffer.Size.Height / (double)(frameBuffer.Size.Width * frameBuffer.Size.Height) * 100.0);
                });
            }
            frameBuffer.EndWriting();
            RaiseRenderingComplete();*/
        }
    }
}
