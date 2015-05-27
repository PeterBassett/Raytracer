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

        public override Colour Sample(MathTypes.Point hitPoint, MathTypes.Normal normalAtHitPoint, ref MathTypes.Vector pointToLight, ref VisibilityTester visibilityTester)
        {
            var colour = new Colour();

            for (int i = 0; i < _samples; i++)
            {
                var w = GetSampledLightPoint() - hitPoint;

                pointToLight = w.Normalize();

                visibilityTester.SetSegment(hitPoint, normalAtHitPoint, w);

                if(visibilityTester.Unoccluded())
                    colour += Intensity / w.LengthSquared;     
            }

            visibilityTester.SetAlwaysUnoccluded();

            return colour / (double)_samples;
        }

        protected abstract Point GetSampledLightPoint();

        private static ThreadLocal<Random> _rnd;

        static AreaLight()
        {
            _rnd = new ThreadLocal<Random>( () => new Random() );
        }

        protected double GetNextRandom()
        {
            return _rnd.Value.NextDouble();
        }

        protected Vector UniformSampleHemisphere()
        {
            return UniformSampleHemisphere(GetNextRandom(), GetNextRandom());
        }

        protected Vector UniformSampleHemisphere(double u1, double u2)
        {
            var r = Math.Sqrt(1.0f - u1 * u1);
            var phi = 2 * Math.PI * u2;
 
            return new Vector(Math.Cos(phi) * r, Math.Sin(phi) * r, u1);
        }
    }
}
