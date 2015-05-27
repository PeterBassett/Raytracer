using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.Lights.AreaLights
{
    abstract class AreaLight : Light
    {
        private readonly int _samples;
        protected AreaLight(Colour colour, double power, Transform transform, int samples)
            : base(colour, power, transform)
        {
            _samples = samples;
        }

        protected override Colour Sample(MathTypes.Point hitPoint, MathTypes.Normal normalAtHitPoint, ref MathTypes.Vector pointToLight, ref VisibilityTester visibilityTester)
        {
            pointToLight = (Pos - hitPoint).Normalize();

            var colour = new Colour();
            
            for (int i = 0; i < _samples; i++)
            {
                var w = GetSampledLightPoint() - hitPoint;
                var lengthSquared = w.LengthSquared;

                visibilityTester.SetSegment(hitPoint, normalAtHitPoint, w);

                w = w.Normalize();

                var lightCos = Vector.DotProduct(w, normalAtHitPoint);

                if (visibilityTester.Unoccluded())
                    colour += (Intensity / lengthSquared) * lightCos;     
            }

            visibilityTester.SetAlwaysUnoccluded();

            return colour / (double)_samples;
        }

        protected override Colour CosineFromNormal(Colour colour, Normal normalAtHitPoint, Vector pointToLight)
        {
            return colour;
        }

        protected abstract Point GetSampledLightPoint();
    }
}
