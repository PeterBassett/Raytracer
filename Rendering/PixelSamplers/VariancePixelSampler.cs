using System;
using Raytracer.MathTypes;
using Raytracer.Rendering.Renderers;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Distributions;

namespace Raytracer.Rendering.PixelSamplers
{
    class VariancePixelSampler : IPixelSampler
    {
        protected uint _minimumSamples;
        protected uint _fireflySamples;
        protected uint _adaptiveSamples;
        protected double _factor;
        protected Distribution _sampler;

        public VariancePixelSampler(uint minimumSamples, uint fireflySamples, uint adaptiveSamples)
        {
            _minimumSamples = minimumSamples;
            _fireflySamples = fireflySamples;
            _adaptiveSamples = adaptiveSamples;
 
            _sampler = new StratifiedDistribution();            
        }

        public virtual void SamplePixel(IRenderer renderer, int x, int y, Raytracer.Rendering.Core.IBuffer buffer)
        {            
            var samplesTaken = 0;

            var samples = GetSampleBlock(null, ref samplesTaken);

            for (int i = 0; i < _minimumSamples; i++)
            {
                AddSample(renderer, x, y, buffer, samples[samplesTaken++]);
                samples = GetSampleBlock(samples, ref samplesTaken);
            }

            if(this._adaptiveSamples > 0)
            {
                var stddev = (double)buffer.StandardDeviation(x, y).MaxComponent();
			    stddev = MathLib.Clamp(stddev, 0.0, 1.0);
                //stddev = (float)Math.Pow(stddev, 1);
			    int additionalSamples = (int)stddev;

                for (int i = 0; i < additionalSamples; i++)
                {
                    AddSample(renderer, x, y, buffer, samples[samplesTaken++]);
                    samples = GetSampleBlock(samples, ref samplesTaken);
                }        
            }

            if(this._fireflySamples > 0) 
            {
				if(buffer.StandardDeviation(x, y).MaxComponent() > 0.01)
                {
					for(int i = 0; i < this._fireflySamples; i++)
                    {
                        AddSample(renderer, x, y, buffer, samples[samplesTaken++]);
                        samples = GetSampleBlock(samples, ref samplesTaken);
                    }
				}
			}        
        }

        private Vector2[] GetSampleBlock(Vector2[] existingSamples, ref int samplesTaken)
        {
            if (samplesTaken % 16 != 0)
            {                
                return existingSamples;
            }

            samplesTaken = 0;
            return _sampler.TwoD(16, 0, 0, 1, 1);
        }

        private void AddSample(IRenderer renderer, int x, int y, Raytracer.Rendering.Core.IBuffer buffer, Vector2 offset)
        {
            var dx = x + offset.X;
            var dy = y + offset.Y;

            buffer.AddSample(x, y, renderer.ComputeSample(new Vector2(dx, dy)));
        }

        public virtual void Initialise()
        {
        }
    }
}
