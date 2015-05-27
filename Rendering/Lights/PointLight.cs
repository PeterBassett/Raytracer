using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.Lights
{
    class PointLight : Light
    {
        public PointLight(Colour colour, double power, Transform transform)
            : base(colour, power, transform)
        {
        }

        protected override Colour Sample(Point hitPoint, Normal normalAtHitPoint, ref Vector pointToLight, ref VisibilityTester visibilityTester)
        {
            var w = Pos - hitPoint;
            
            pointToLight = w.Normalize();

            visibilityTester.SetSegment(hitPoint, normalAtHitPoint, Pos);

            return Intensity / w.LengthSquared; 
        }
    }
}