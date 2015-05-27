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

        public virtual Colour SampleLight(Point hitPoint, Normal normalAtHitPoint, ref Vector pointToLight, ref VisibilityTester visibilityTester)
        {
            var colour = Sample(hitPoint, normalAtHitPoint, ref pointToLight, ref visibilityTester);

            return CosineFromNormal(colour, normalAtHitPoint, pointToLight);
        }

        protected virtual Colour CosineFromNormal(Colour colour, Normal normalAtHitPoint, Vector pointToLight)
        {
            normalAtHitPoint = normalAtHitPoint.Normalize();
            pointToLight = pointToLight.Normalize();
            return colour * Vector.DotProduct(normalAtHitPoint, pointToLight);
        }

        protected abstract Colour Sample(Point hitPoint, Normal normalAtHitPoint, ref Vector pointToLight, ref VisibilityTester visibilityTester);

        public virtual bool Specular()
        {
            return true;
        }
    }
}
