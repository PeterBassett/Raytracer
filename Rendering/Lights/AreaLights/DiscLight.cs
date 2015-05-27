using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Samplers;

namespace Raytracer.Rendering.Lights.AreaLights
{
    class DiscLight : SphereLight
    {
        private readonly Normal _normal;

        public DiscLight(Colour colour, double power, Transform transform, int samples, double radius)
            : base(colour, power, transform, samples, radius)
        {
            _normal = Transform.ToWorldSpace(new Normal(0, -1, 0));
        }

        protected override Point GetSampledLightPoint()
        {
            var pointOnDisc = Sampler.ConcentricSampleDisk();
            var point = new Point(pointOnDisc.X, pointOnDisc.Y, 0);

            var offset = point * _radius;
            offset = Transform.ToWorldSpace(offset);

            return Pos + offset;
        }
    }
}
