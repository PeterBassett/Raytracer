using System;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.Lights
{
    class SpotLight : Light
    {
        private readonly double _cosTotalWidth;
        private readonly double _cosFalloffStart;
        
        public SpotLight(Colour colour, float power, float totalWidthInDegrees, float widthBeforeFallOffInDegrees, Transform transform)
            : base(colour, power, transform)
        {
            _cosTotalWidth = Math.Cos(MathLib.Deg2Rad(totalWidthInDegrees));
            _cosFalloffStart = Math.Cos(MathLib.Deg2Rad(widthBeforeFallOffInDegrees));
        }

        public override Colour Sample(Point hitPoint, Normal normalAtHitPoint, ref Vector pointToLight, ref VisibilityTester visibilityTester)
        {
            pointToLight = Pos - hitPoint;
            
            var lengthSquared = pointToLight.LengthSquared;

            pointToLight = pointToLight.Normalize();

            visibilityTester.SetSegment(hitPoint, normalAtHitPoint, Pos);

            return Intensity * Falloff(-pointToLight) / lengthSquared;
        }

        double Falloff(Vector w) 
        {
            Vector wl = Transform.ToObjectSpace(w).Normalize();
            var costheta = wl.Z;

            if (costheta < _cosTotalWidth)     
                return 0.0;

            if (costheta > _cosFalloffStart)   
                return 1.0;

            // Compute falloff inside spotlight cone
            var delta = (costheta - _cosTotalWidth) /
                        (_cosFalloffStart - _cosTotalWidth);

            return delta*delta*delta*delta;
        }
    }
}