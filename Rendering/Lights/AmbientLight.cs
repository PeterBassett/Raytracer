using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.Lights
{
    class AmbientLight : Light
    {
        public AmbientLight(Colour colour)
            : base(colour, 1.0, Transform.CreateIdentityTransform())
        {
        }

        public override Colour Sample(Point hitPoint, Normal normalAtHitPoint, ref Vector pointToLight, ref VisibilityTester visibilityTester)
        {
            pointToLight = (Vector)(normalAtHitPoint);
            return Intensity; 
        }

        public override bool Specular()
        {
            return false;
        }
    }
}
