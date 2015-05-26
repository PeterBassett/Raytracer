using System;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.Primitives
{
    class Plane : Traceable
    {
        public Plane()
        {
            Normal = new Normal(0.0f, 0.0f, 0.0f);
        }

        public double D { get; set; }
        public Normal Normal { get; set; }

        public override IntersectionInfo Intersect(Ray ray)
        {
            var nd = Vector.DotProduct(Normal, ray.Dir);

            if (nd >= 0.0f)
                return new IntersectionInfo(HitResult.Miss);

            var distance = -(Vector.DotProduct(Normal, (Vector)ray.Pos) + D) / nd;
            
            if (distance <= 0.0f)
                return new IntersectionInfo(HitResult.Miss);
            
            var hitPoint = ray.Pos + (ray.Dir * distance);

            return new IntersectionInfo(HitResult.Hit, this, distance, hitPoint, hitPoint, Normal);
        }
        
        public override AABB GetAABB()
        {
            return AABB.Empty;
        }

        public override bool Contains(Point point)
        {
            var dist = Vector.DotProduct(Normal, (point - this.Pos));

            return (Math.Abs(dist) < MathLib.IntersectionEpsilon);
        }
    }
}
