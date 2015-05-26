using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.Lights
{
    class DistantLight : Light
    {
        private readonly Vector _dir;
        public DistantLight(Colour colour, double power, Transform transform)
            : base(colour, power, transform)
        {
            _dir = Transform.ToObjectSpace(new Vector(0, 1, 0));
        }

        public override Colour Sample(Point hitPoint, Normal normalAtHitPoint, ref Vector pointToLight, ref VisibilityTester visibilityTester)
        {
            pointToLight = _dir;
            
            visibilityTester.SetSegment(hitPoint, normalAtHitPoint, pointToLight);

            return Intensity;
        }
    }
}
