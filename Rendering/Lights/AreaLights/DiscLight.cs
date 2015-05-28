using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Samplers;
using Raytracer.Rendering.Distributions;

namespace Raytracer.Rendering.Lights.AreaLights
{
    class DiscLight : SphereLight
    {
        private readonly Normal _normal;

        public DiscLight(Colour colour, double power, Transform transform, uint samples, double radius, Distribution distribution)
            : base(colour, power, transform, samples, radius, distribution)
        {
            _normal = Transform.ToWorldSpace(new Normal(0, -1, 0));
        }

        protected override Point GetSampledLightPoint(Vector2 sample)
        {
            var pointOnDisc = Sampler.ConcentricSampleDisk(sample);
            var point = new Point(pointOnDisc.X, pointOnDisc.Y, 0);

            var offset = point * _radius;
            offset = Transform.ToWorldSpace(offset);

            return Pos + offset;
        }
    }
}
