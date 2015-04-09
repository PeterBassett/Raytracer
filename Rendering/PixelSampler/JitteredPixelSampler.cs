using System;
using Raytracer.Rendering.FileTypes;
using Raytracer.MathTypes;
using Raytracer.Rendering.Renderers;

namespace Raytracer.Rendering.PixelSamplers
{
    class JitteredPixelSampler : IPixelSampler
    {
        protected uint _samples;
        protected double _factor;
        protected Random _random;

        public JitteredPixelSampler(uint samples)
        {
            _samples = samples;
            _factor = 1.0 / samples;
            _random = new Random();
        }

        public virtual Colour SamplePixel(IRenderer renderer, int x, int y)
        {
            Colour colour = new Colour();

            for (int u = 0; u < _samples; ++u)
            {
                for (int v = 0; v < _samples; ++v)
                {
                    var dx = x + SampleOffset(u);
                    var dy = y + SampleOffset(v);

                    colour += renderer.ComputeSample(new Vector2(dx, dy));
                }
            }
	        
            return colour / (_samples * _samples);
        }

        private double SampleOffset(int i)
        {
            if (_samples <= 1)
                return 0.0;
            
            double subSamplingOffset1 = _factor * i;
            double subSamplingOffset2 = _factor * (i + 1);

            return subSamplingOffset1 + (_random.NextDouble() * (subSamplingOffset2 - subSamplingOffset1));
        }
    }
}
