using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.MathTypes;
using Raytracer.Rendering.Renderers;

namespace Raytracer.Rendering.Core
{
    class VisibilityTester
    {
        const double _shadowRayEpslion = 0.00001;
        private Ray ray;
        private double _maxT;

        public void SetSegment(Point from, Normal normalAtHitPoint, Vector direction)
        {
            _maxT = double.MaxValue;
            ray = new Ray(from + (normalAtHitPoint * _shadowRayEpslion), direction.Normalize());
        }

        public void SetSegment(Point from, Normal normalAtHitPoint, Point to) 
        {
            var segment = to - from;

            _maxT = segment.Length;

            segment = segment.Normalize();

            ray = new Ray(from + (normalAtHitPoint * _shadowRayEpslion), segment);
        }

        public bool Unoccluded(IRenderer renderer)
        {
            if (!ray.IsValid) 
                return true;

            var intersection = renderer.FindClosestIntersection(ray);

            return intersection.Result == HitResult.MISS || intersection.T >= _maxT || intersection.T < 0;
        }
    }
}
