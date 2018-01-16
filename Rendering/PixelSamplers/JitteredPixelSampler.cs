using System;
using Raytracer.MathTypes;
using Raytracer.Rendering.Renderers;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Distributions;

namespace Raytracer.Rendering.PixelSamplers
{
    class JitteredPixelSampler : IPixelSampler
    {
        protected uint _samples;
        protected double _factor;
        protected Distribution _random;

        public JitteredPixelSampler(Distribution random, uint samples)
        {
            _samples = samples;
            _factor = 1.0 / samples;
            _random = random;
        }
                
        public virtual void SamplePixel(IRenderer renderer, int x, int y, Raytracer.Rendering.Core.IBuffer buffer)
        {
            Colour colour = new Colour();

            for (int u = 0; u < _samples; ++u)
            {
                for (int v = 0; v < _samples; ++v)
                {
                    var offsets = _random.TwoD(_samples, 0, 0, 1, 1);

                    var dx = x + offsets[0].X;
                    var dy = y + offsets[0].Y;

                    buffer.AddSample(x, y, renderer.ComputeSample(new Vector2(dx, dy)));
                }
            }	        
        }

        /*
        private double SampleOffset(int i)
        {
            if (_samples < 1)
                return 0.0;
            
            double subSamplingOffset1 = _factor * i;
            double subSamplingOffset2 = _factor * (i + 1);

            var offsets = _random.TwoD(_factor, 0, 1, 0, 1);

            return subSamplingOffset1 + (_random.NextDouble() * (subSamplingOffset2 - subSamplingOffset1));
        }
        */

        public virtual void Initialise()
        {
        }
    }
}
