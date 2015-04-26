using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.Rendering.Core;
using Raytracer.MathTypes;

namespace Raytracer.Rendering.Primitives
{
    abstract class ObjectSpacePrimitive : Traceable
    {
        protected Matrix _worldToObjectSpace;
        protected Matrix _objectToWorldSpace;

        public ObjectSpacePrimitive(Matrix worldToObjectSpace)
        {
            _worldToObjectSpace = worldToObjectSpace;

            _objectToWorldSpace = worldToObjectSpace;
            _objectToWorldSpace.Invert();
        }

        public ObjectSpacePrimitive (Matrix worldToObjectSpace, Matrix objectToWorldSpace)
	    {
            _worldToObjectSpace = worldToObjectSpace;
            _objectToWorldSpace = objectToWorldSpace;
	    }

        public override IntersectionInfo Intersect(Ray ray)
        {
            var objectSpaceRay = ray.Transform(_worldToObjectSpace);

            var info = ObjectSpace_Intersect(objectSpaceRay);

            if (info.Result == HitResult.MISS)
                return info;

            info.HitPoint = info.HitPoint.Transform(_objectToWorldSpace);
            info.NormalAtHitPoint = info.NormalAtHitPoint.Transform(_objectToWorldSpace);

            return info;
        }

        public override bool Intersect(AABB aabb)
        {
            var objectSpaceAABB = aabb.Transform(_worldToObjectSpace);

            return ObjectSpace_Intersect(objectSpaceAABB);
        }

        public override bool Contains(Point point)
        {
            point = point.Transform(_worldToObjectSpace);

            return ObjectSpace_Contains(point);
        }

        public override AABB GetAABB()
        {
            return ObjectSpace_GetAABB().Transform(_objectToWorldSpace);
        }

        public abstract IntersectionInfo ObjectSpace_Intersect(Ray ray);
        public abstract bool ObjectSpace_Intersect(AABB aabb);
        public abstract bool ObjectSpace_Contains(Point point);
        public abstract AABB ObjectSpace_GetAABB();
    }
}
