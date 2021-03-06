﻿using Raytracer.Rendering.Core;
using Raytracer.MathTypes;

namespace Raytracer.Rendering.Primitives
{
    abstract class ObjectSpacePrimitive : Traceable
    {
        private readonly Transform _transform;

        protected ObjectSpacePrimitive(Transform transform)
        {
            _transform = transform;
        }
     
        public override IntersectionInfo Intersect(Ray ray)
        {
            var rayPos = ray.Pos;
            ray = _transform.ToWorldSpace(ray);

            var info = ObjectSpaceIntersect(ray);

            if (info.Result == HitResult.Miss)
                return info;

            info = _transform.ToObjectSpace(info);

            info.T = (rayPos - info.HitPoint).Length;

            return info;
        }

        public override bool Contains(Point point)
        {
            return ObjectSpaceContains(_transform.ToWorldSpace(point));
        }

        public override AABB GetAABB()
        {
            return _transform.ToObjectSpace(ObjectSpaceGetAABB());
        }

        protected abstract IntersectionInfo ObjectSpaceIntersect(Ray ray);
        protected abstract bool ObjectSpaceContains(Point point);
        // ReSharper disable once InconsistentNaming
        protected abstract AABB ObjectSpaceGetAABB();
    }
}
