using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.Primitives
{
    class Plane : Traceable
    {
        public Plane()
        {
            Normal = new Vector3(0.0f, 0.0f, 0.0f);
        }

        public double D { get; set; }
        public Vector3 Normal { get; set; }

        public override IntersectionInfo Intersect(Ray ray)
        {
            var nd = Vector3.DotProduct(Normal, ray.Dir);

            if (nd >= 0.0f)
                return new IntersectionInfo(HitResult.MISS);

            var distance = -(Vector3.DotProduct(Normal, ray.Pos) + D) / nd;
            
            if (distance <= 0.0f)
                return new IntersectionInfo(HitResult.MISS);
            
            var hitPoint = ray.Pos + (ray.Dir * distance);

            return new IntersectionInfo(HitResult.HIT, this, distance, hitPoint, hitPoint, Normal);
        }
        
        public override bool Intersect(AABB aabb)
        {
            throw new NotImplementedException();
        }

        public override AABB GetAABB()
        {
            return AABB.Empty;
        }

        public override bool Contains(Vector3 point)
        {
            var dist = Vector3.DotProduct(Normal, (point - this.Pos));

            return (Math.Abs(dist) < MathLib.IntersectionEpsilon);
        }
    }
}
