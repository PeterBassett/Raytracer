using System;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.FileTypes;

namespace Raytracer.Rendering.Lights
{
    class DistantLight : Light
    {
        public Vector Dir;
        public DistantLight(Colour colour, double power, Transform transform)
            : base(colour, transform)
        {
            Intensity *= power;
            Dir = _transform.ToObjectSpace(new Vector(0, 1, 0));
        }

        public override Colour Sample(Point hitPoint, Normal normalAtHitPoint, ref Vector pointToLight, ref VisibilityTester visibilityTester)
        {
            pointToLight = Dir;
            
            visibilityTester.SetSegment(hitPoint, normalAtHitPoint, pointToLight);

            return Intensity;
        }
    }
}
