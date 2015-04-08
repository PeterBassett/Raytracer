using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.MathTypes;
using Raytracer.Rendering.FileTypes;
using System.Threading.Tasks;
using Raytracer.Rendering.Cameras;
using Raytracer.Rendering.PixelSamplers;

namespace Raytracer.Rendering
{
    class Renderer
    {
        private IScene _scene;
        private IBmp _frameBuffer;
        private IPixelSampler _pixelSampler;
        private ICamera _camera;
        private bool _multiThreaded;

        public Renderer(IScene scene, ICamera camera, IBmp frameBuffer, IPixelSampler pixelSampler, bool multiThreaded)
        {
            _scene = scene;
            _camera = camera;
            _frameBuffer = frameBuffer;
            _pixelSampler = pixelSampler;
            _multiThreaded = multiThreaded;
        }

        public void RenderScene()
        {
            var options = new ParallelOptions();
            if (!_multiThreaded)
                options.MaxDegreeOfParallelism = 1;

            Parallel.For(0, _frameBuffer.Size.Width, options, (x) =>
            {
                for (int y = 0; y < _frameBuffer.Size.Height; y++)                    
                    _frameBuffer.SetPixel(x, y, _pixelSampler.SamplePixel(this, x, y));
            });
        }

        public Colour ComputeSample(Vector2 pixelcoord)
        {
            return _scene.Trace(_camera.GenerateRayForPixel(pixelcoord));
        }
    }
}
