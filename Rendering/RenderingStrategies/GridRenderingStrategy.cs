using System.Diagnostics;
using System.Threading.Tasks;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.PixelSamplers;
using Raytracer.Rendering.Renderers;
using Raytracer.Rendering.Synchronisation;
using System;
using Raytracer.MathTypes;
using System.Collections.Generic;

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

        int ComputeSubWindow(int width, int height, out int xTiles, out int yTiles) 
        {
            int chunks = Math.Max(32 * Environment.ProcessorCount, (width * height) / (16 * 16));
            chunks = (int)MathLib.RoundUpPow2((uint)chunks);

            int dx = width;
            int dy = height;

            int nx = chunks, ny = 1;

            while ((nx & 0x1) == 0 && 2 * dx * ny < dy * nx) {
                nx >>= 1;
                ny <<= 1;
            }
            
            Debug.Assert(nx * ny == chunks);

            xTiles = nx;
            yTiles = ny;

            return chunks;
        }

        public void RenderScene(IRenderer renderer, IBmp frameBuffer)
        {
            _pixelSampler.Initialise();

            int xTiles; 
            int yTiles;
            int tasks = ComputeSubWindow(frameBuffer.Size.Width, frameBuffer.Size.Height, out xTiles, out yTiles);

            int framePixels = frameBuffer.Size.Width * frameBuffer.Size.Height;

            var ranges = BuildRanges(tasks, xTiles, yTiles, frameBuffer.Size);
            var options = GetThreadingOptions();
            
            RaiseRenderingStarted();
            frameBuffer.BeginWriting();
            Parallel.ForEach(ranges, options, (imageRange, state) =>
            {
                for (int x = imageRange.X1; x < imageRange.X2; x++)
                {
                    for (int y = imageRange.Y1; y < imageRange.Y2; y++)
                    {
                        if (_cancellationToken.IsCancellationRequested)
                        {
                            state.Break();
                            return;
                        }

                        frameBuffer.SetPixel(x, y, _pixelSampler.SamplePixel(renderer, x, y));
                    }
                }


                RaiseOnCompletedPercentageDelta(imageRange.Pixels / (double)framePixels * 100.0);
            });            
            frameBuffer.EndWriting();
            RaiseRenderingComplete();
        }

        private IEnumerable<ImageRange> BuildRanges(int tasks, int xTiles, int yTiles, Size size)
        {
            int nx = xTiles;
            int ny = yTiles;

            for (int i = 0; i < tasks; i++)
			{
			    // Compute $x$ and $y$ pixel sample range for sub-window
                int xo = i % nx, yo = i / nx;

                float tx0 = (float)xo / (float)nx, tx1 = ((float)xo+1) / (float)nx;
                float ty0 = (float)yo / (float)ny, ty1 = ((float)yo+1) / (float)ny;
                
                yield return new ImageRange(
                    (int)Math.Floor(MathLib.Lerp(0, size.Width, tx0)),
                    (int)Math.Floor(MathLib.Lerp(0, size.Width, tx1)),
                    (int)Math.Floor(MathLib.Lerp(0, size.Height, ty0)),
                    (int)Math.Floor(MathLib.Lerp(0, size.Height, ty1)));
            }
        }
    }
}
