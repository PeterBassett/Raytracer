using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Samplers;

namespace Raytracer.Rendering.Lights.AreaLights
{
    class SphereLight : AreaLight
    {
        protected readonly double _radius;

        public SphereLight(Colour colour, double power, Transform transform, int samples, double radius)
            : base(colour, power, transform, samples)
        {
            _radius = radius;
        }

        protected override Point GetSampledLightPoint()
        {
            var offset = Sampler.UniformSampleHemisphere() * _radius;
            offset = Transform.ToWorldSpace(offset);

            return Pos + offset;
        }
    }
}
