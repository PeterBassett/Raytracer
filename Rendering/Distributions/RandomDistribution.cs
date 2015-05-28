using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.MathTypes;

namespace Raytracer.Rendering.Distributions
{
    class RandomDistribution : Distribution
    {
        public override Vector2[] TwoD(uint samples, double x1, double y1, double x2, double y2)
        {
            var sample = new Vector2[samples];

            for (int i = 0; i < samples; i++)
                sample[i] = new Vector2(GetNextRandom(x1, x2), GetNextRandom(y1, y2));

            return sample;
        }
    }
}
