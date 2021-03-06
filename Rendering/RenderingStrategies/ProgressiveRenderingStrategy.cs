﻿using System.Diagnostics;
using System.Threading.Tasks;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.PixelSamplers;
using Raytracer.Rendering.Renderers;
using System.Linq;
using Raytracer.Rendering.Synchronisation;

namespace Raytracer.Rendering.RenderingStrategies
{
    class ProgressiveRenderingStrategy : ParallelOptionsBase, IRenderingStrategy
    {
        private readonly IPixelSampler _pixelSampler;
        private readonly int _skip;

        public ProgressiveRenderingStrategy(IPixelSampler pixelSampler, int initialSkip, bool multiThreaded, CancellationToken cancellationToken) : base(multiThreaded, cancellationToken)
        {
            Debug.Assert(pixelSampler != null, "pixelSampler != null");
            _pixelSampler = pixelSampler;
            _skip = initialSkip;            
        }

        public void RenderScene(IRenderer renderer, IBuffer frameBuffer)
        {
            var options = GetThreadingOptions();         

            int skip = _skip;
           
            while (skip >= 1)
            {                
                var xs = Enumerable.Range(0, frameBuffer.Size.Width).Where(i => i % skip == 0).ToArray();

                RaiseRenderingStarted();
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

                        _pixelSampler.SamplePixel(renderer, x, y, frameBuffer);

                        // ReSharper disable once AccessToModifiedClosure
                        //SetColourBlock(frameBuffer, skip, x, y, colour);
                    }

                    RaiseOnCompletedPercentageDelta(1 / (double)xs.Length * 100.0);
                });
                RaiseRenderingComplete();

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

        private static void SetColourBlock(IBuffer frameBuffer, int skip, int x, int y, Colour colour)
        {
            for (int xs = x; xs < x + skip && xs < frameBuffer.Size.Width; xs++)
                for (int ys = y; ys < y + skip && ys < frameBuffer.Size.Height; ys++)
                {
                    frameBuffer.AddSample(xs, ys, colour);
                }
        }
    }
}
