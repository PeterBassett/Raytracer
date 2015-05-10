using System;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.FileTypes;

namespace Raytracer.Rendering.Lights
{
    class SpotLight : Light
    {
        public double CosTotalWidth;
        public double CosFalloffStart;
        public Vector Dir;

        public SpotLight(Colour colour, float totalWidthInDegrees, float widthBeforeFallOffInDegrees, Transform transform)
            : base(colour, transform)
        {
            Dir = _transform.ToObjectSpace(new Vector(0.0, 1.0, 0.0));

            CosTotalWidth = Math.Cos(MathLib.Deg2Rad(totalWidthInDegrees));
            CosFalloffStart = Math.Cos(MathLib.Deg2Rad(widthBeforeFallOffInDegrees));
        }

        public override Colour Sample(Point hitPoint, Normal normalAtHitPoint, ref Vector pointToLight, ref VisibilityTester visibilityTester)
        {
            pointToLight = (Pos - hitPoint).Normalize();
            
            visibilityTester.SetSegment(hitPoint, normalAtHitPoint, Pos);

            return Intensity * Falloff(-pointToLight) / (Pos - hitPoint).LengthSquared;
        }

        double Falloff(Vector w) 
        {
            Vector wl = _transform.ToWorldSpace(w).Normalize();
            var costheta = wl.Z;

            if (costheta < CosTotalWidth)     
                return 0.0;

            if (costheta > CosFalloffStart)   
                return 1.0;

            // Compute falloff inside spotlight cone
            var delta = (costheta - CosTotalWidth) /
                        (CosFalloffStart - CosTotalWidth);

            return delta*delta*delta*delta;
        }
    }
}