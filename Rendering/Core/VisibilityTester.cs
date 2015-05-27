using Raytracer.MathTypes;
using Raytracer.Rendering.Renderers;

namespace Raytracer.Rendering.Core
{
    class VisibilityTester
    {
        const double ShadowRayEpslion = 0.00001;
        private Ray _ray;
        private double _maxT;
        private readonly IRenderer _renderer;
        public VisibilityTester(IRenderer renderer)
        {
            _renderer = renderer;
        }

        public void SetAlwaysUnoccluded()
        {
            _ray = new Ray();
        }

        public void SetSegment(Point from, Normal normalAtHitPoint, Vector direction)
        {
            _maxT = double.MaxValue;
            _ray = new Ray(from + (normalAtHitPoint * ShadowRayEpslion), direction.Normalize());
        }

        public void SetSegment(Point from, Normal normalAtHitPoint, Point to) 
        {
            var segment = to - from;

            _maxT = segment.Length;

            segment = segment.Normalize();

            _ray = new Ray(from + (normalAtHitPoint * ShadowRayEpslion), segment);
        }

        public bool Unoccluded()
        {
            if (!_ray.IsValid) 
                return true;

            var intersection = _renderer.FindClosestIntersection(_ray);

            return intersection.Result == HitResult.Miss || intersection.T >= _maxT || intersection.T < 0;
        }
    }
}
