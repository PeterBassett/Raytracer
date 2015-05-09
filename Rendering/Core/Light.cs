using Raytracer.Rendering.FileTypes;
using Raytracer.MathTypes;

namespace Raytracer.Rendering.Core
{
    abstract class Light
    {
        protected Transform _transform;
        public Colour Intensity;
        public Point Pos { get; set; }

        public Light(Colour colour, Transform transform)
        {
            Intensity = colour;
            _transform = transform;
            Pos = _transform.ToWorldSpace(new Point(0.0, 0.0, 0.0));
        }

        public abstract Colour Sample(Point hitPoint, Normal normalAtHitPoint, ref Vector pointToLight, ref VisibilityTester visibilityTester);
    }
}
