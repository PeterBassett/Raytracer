using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.MathTypes;

namespace Raytracer.Rendering.Distributions
{
    class StratifiedDistribution : Distribution
    {
        public override Vector2[] TwoD(uint samples, double x1, double y1, double x2, double y2)
        {
            var sqrtSamples = Math.Sqrt(samples);
            var dimSamples = (int)Math.Floor(Math.Sqrt(samples));
            var dx = x2 - x1;
            var dy = y2 - y1;

            var dxSamples = dx / sqrtSamples;
            var dySamples = dy / sqrtSamples;

            var sample = new Vector2[dimSamples * dimSamples];

            bool jitter = true;
            for (int y = 0; y < dimSamples; y++)
            {
                for (int x = 0; x < dimSamples; x++)
                {
                    var jx = jitter ? GetNextRandom(0, dxSamples) : 0.5;
                    var jy = jitter ? GetNextRandom(0, dySamples) : 0.5;

                    sample[y * dimSamples +x] = new Vector2(dxSamples * x + jx,
                                                            dySamples * y + jy);
                }
            }

            return sample;
        }
    }
}
