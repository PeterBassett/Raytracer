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
        public Vector dir;
        public SpotLight(Colour colour, float totalWidthInDegrees, float widthBeforeFallOffInDegrees, Transform transform)
            : base(colour, transform)
        {
            dir = _transform.ToObjectSpace(new MathTypes.Vector(0, 0, 1));
 
            CosTotalWidth = Math.Cos(MathLib.Deg2Rad(totalWidthInDegrees));
            CosFalloffStart = Math.Cos(MathLib.Deg2Rad(widthBeforeFallOffInDegrees));
        }

        public override Colour Sample(Point hitPoint, Normal normalAtHitPoint, ref Vector pointToLight, ref VisibilityTester visibilityTester)
        {
            var hitPoint2 = _transform.ToObjectSpace(hitPoint);

            pointToLight = (Pos - hitPoint).Normalize();

            var cos = Vector.DotProduct(pointToLight, dir);
            //visibilityTester.SetSegment(hitPoint, normalAtHitPoint, Pos);

            return Intensity * Falloff(-pointToLight, cos) / (Pos - hitPoint).LengthSquared;
        }

        double Falloff(Vector w, double costheta2) 
        {
            Vector wl = _transform.ToObjectSpace(w).Normalize();
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