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
    class CylinderLight : AreaLight
    {
        protected readonly double _radius;
        protected readonly double _height;

        public CylinderLight(Colour colour, double power, Transform transform, uint samples, double radius, double height, Distribution distribution)
            : base(colour, power, transform, samples, distribution)
        {
            _radius = radius;
            _height = height;
        }
        
        protected override Point GetSampledLightPoint(Vector2 sample)
        {
            var offset = Sampler.UniformSampleHemisphere(sample) * _radius;
            offset = Transform.ToWorldSpace(offset);

            return Pos + offset;
        }
    }
}
