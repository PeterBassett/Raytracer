using Raytracer.MathTypes;

namespace Raytracer.Rendering.Core
{
    abstract class Light
    {
        protected readonly Transform Transform;
        protected readonly Colour Intensity;
        protected readonly Point Pos;

        protected Light(Colour colour, double power, Transform transform)
        {
            Intensity = colour * power;
            Transform = transform;
            Pos = Transform.ToObjectSpace(new Point(0.0, 0.0, 0.0));
        }

        public abstract Colour Sample(Point hitPoint, Normal normalAtHitPoint, ref Vector pointToLight, ref VisibilityTester visibilityTester);

        public virtual bool Specular()
        {
            return true;
        }
    }
}
