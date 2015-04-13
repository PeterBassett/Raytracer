using System.Threading.Tasks;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.PixelSamplers;
using Raytracer.Rendering.Renderers;

namespace Raytracer.Rendering.RenderingStrategies
{
    class BasicRenderingStrategy : IRenderingStrategy
    {
        private IPixelSampler _pixelSampler;
        private bool _multiThreaded;
        
        public BasicRenderingStrategy(IPixelSampler pixelSampler, bool multiThreaded)
        {
            _pixelSampler = pixelSampler;
            _multiThreaded = multiThreaded;
        }

        public void RenderScene(IRenderer renderer, IBmp frameBuffer)
        {
            var options = new ParallelOptions();
            if (!_multiThreaded)
                options.MaxDegreeOfParallelism = 1;
            
            frameBuffer.BeginWriting();
            Parallel.For(0, frameBuffer.Size.Width, options, (x) =>
            {
                for (int y = 0; y < frameBuffer.Size.Height; y++)
                    frameBuffer.SetPixel(x, y, _pixelSampler.SamplePixel(renderer, x, y));
            });
            frameBuffer.EndWriting();
        }
    }
}
