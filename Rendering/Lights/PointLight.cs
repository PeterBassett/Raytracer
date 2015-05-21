using System;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.FileTypes;

namespace Raytracer.Rendering.Lights
{
    class PointLight : Light
    {
        public PointLight(Colour colour, Transform transform)
            : base(colour, transform)
        {
        }

        public override Colour Sample(Point hitPoint, Normal normalAtHitPoint, ref Vector pointToLight, ref VisibilityTester visibilityTester)
        {
            var w = Pos - hitPoint;
            
            pointToLight = w.Normalize();

            visibilityTester.SetSegment(hitPoint, normalAtHitPoint, Pos);

            return Intensity / w.LengthSquared; 
        }
    }
}