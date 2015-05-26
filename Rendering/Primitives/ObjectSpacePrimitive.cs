using Raytracer.Rendering.Core;
using Raytracer.MathTypes;

namespace Raytracer.Rendering.Primitives
{
    abstract class ObjectSpacePrimitive : Traceable
    {
        protected Transform _transform;

        public ObjectSpacePrimitive(Transform transform)
        {
            _transform = transform;
        }
     
        public override IntersectionInfo Intersect(Ray ray)
        {
            ray = _transform.ToObjectSpace(ray);

            var info = ObjectSpace_Intersect(ray);

            if (info.Result == HitResult.Miss)
                return info;

            info = _transform.ToWorldSpace(info);

            return info;
        }

        public override bool Contains(Point point)
        {
            return ObjectSpace_Contains(_transform.ToObjectSpace(point));
        }

        public override AABB GetAABB()
        {
            return _transform.ToWorldSpace(ObjectSpace_GetAABB());
        }

        public abstract IntersectionInfo ObjectSpace_Intersect(Ray ray);
        public abstract bool ObjectSpace_Contains(Point point);
        public abstract AABB ObjectSpace_GetAABB();
    }
}
