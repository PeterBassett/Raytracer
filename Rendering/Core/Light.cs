using Raytracer.FileTypes;
using Raytracer.MathTypes;

namespace Raytracer.Rendering.Core
{
    abstract class Light
    {
        protected Transform _transform;
        public Colour Intensity;
        public Point Pos;

        public Light(Colour colour, Transform transform)
        {
            Intensity = colour;
            _transform = transform;
            Pos = _transform.ToObjectSpace(new Point(0.0, 0.0, 0.0));
        }

        public abstract Colour Sample(Point hitPoint, Normal normalAtHitPoint, ref Vector pointToLight, ref VisibilityTester visibilityTester);

        public virtual bool Specular()
        {
            return true;
        }
    }
}
