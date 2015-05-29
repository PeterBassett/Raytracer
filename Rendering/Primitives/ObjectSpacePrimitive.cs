using Raytracer.Rendering.Core;
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
            ray = _transform.ToObjectSpace(ray);

            var info = ObjectSpaceIntersect(ray);

            if (info.Result == HitResult.Miss)
                return info;

            info = _transform.ToWorldSpace(info);
            
            return info;
        }

        public override bool Contains(Point point)
        {
            return ObjectSpaceContains(_transform.ToObjectSpace(point));
        }

        public override AABB GetAABB()
        {
            return _transform.ToWorldSpace(ObjectSpaceGetAABB());
        }

        protected abstract IntersectionInfo ObjectSpaceIntersect(Ray ray);
        protected abstract bool ObjectSpaceContains(Point point);
        // ReSharper disable once InconsistentNaming
        protected abstract AABB ObjectSpaceGetAABB();
    }
}
