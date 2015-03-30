using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.MathTypes;

namespace Raytracer.Rendering.Primitives
{
    using Vector = Vector3;
    using Real = System.Double;

    class Plane : Traceable
    {
        public Plane()
        {
            Normal = new Vector(0.0f, 0.0f, 0.0f);
        }

        public Real D { get; set; }
        public Vector Normal { get; set; }

        public override IntersectionInfo Intersect(Ray ray)
        {
            Real nd = Vector.DotProduct(Normal, ray.Dir);

            if (nd >= 0.0f)
                return new IntersectionInfo(HitResult.MISS);

            Real distance = -(Vector.DotProduct(Normal, ray.Pos) + D) / nd;

            if (distance <= 0.0f)
                return new IntersectionInfo(HitResult.MISS);
            
            var hitPoint = ray.Pos + (ray.Dir * distance);

            return new IntersectionInfo(HitResult.HIT, this, distance, hitPoint, hitPoint, GetNormal(hitPoint));
        }
        
        public override Vector GetNormal(Vector vPoint)
        {
            return Normal;
        }

        public override bool Intersect(AABB aabb)
        {
            throw new NotImplementedException();
        }

        public override AABB GetAABB()
        {
            return AABB.Empty;
        }
    }
}
