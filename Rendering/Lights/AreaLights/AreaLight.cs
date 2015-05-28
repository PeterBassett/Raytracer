using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Distributions;

namespace Raytracer.Rendering.Lights.AreaLights
{
    abstract class AreaLight : Light
    {
        protected readonly uint _samples;
        protected readonly Distribution _distribution;

        protected AreaLight(Colour colour, double power, Transform transform, uint samples, Distribution distribution)
            : base(colour, power, transform)
        {
            _samples = samples;
            _distribution = distribution;
        }

        protected override Colour Sample(MathTypes.Point hitPoint, MathTypes.Normal normalAtHitPoint, ref MathTypes.Vector pointToLight, ref VisibilityTester visibilityTester)
        {
            pointToLight = (Pos - hitPoint).Normalize();

            var colour = new Colour();
            var samples = _distribution.TwoD(_samples, 0, 0, 1, 1);

            for (int i = 0; i < samples.Length; i++)
            {
                var w = GetSampledLightPoint(samples[i]) - hitPoint;
                var lengthSquared = w.LengthSquared;

                visibilityTester.SetSegment(hitPoint, normalAtHitPoint, w);

                w = w.Normalize();

                var lightCos = Vector.DotProduct(w, normalAtHitPoint);

                if (visibilityTester.Unoccluded())
                    colour += (Intensity / lengthSquared) * lightCos;     
            }

            visibilityTester.SetAlwaysUnoccluded();

            return colour / (double)samples.Length;
        }

        protected override Colour CosineFromNormal(Colour colour, Normal normalAtHitPoint, Vector pointToLight)
        {
            return colour;
        }

        protected abstract Point GetSampledLightPoint(Vector2 sample);
    }
}
