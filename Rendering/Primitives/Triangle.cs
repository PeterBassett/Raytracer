using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.MathTypes;

namespace Raytracer.Rendering.Primitives
{
    using Vector = Vector3;
    using Real = System.Double;

    class Triangle : Traceable
    {
        public Vector[] Vertex = new Vector[3];
        private AABB bounds = AABB.Empty;
        public Vector2[] Texture = new Vector2[3];
        public Vector[] Normal = new Vector[3];        

        private bool InternalSide(Vector p1,
                                Vector p2,
                                Vector a,
                                Vector b)
        {
            Vector cp1 = Vector.CrossProduct(b - a, p1 - a);

            Vector cp2 = Vector.CrossProduct(b - a, p2 - a);

            if (Vector.DotProduct(cp1, cp2) >= 0)
                return true;
            else
                return false;
        }

        private bool PointInTriangle(Vector p)
        {
            if (InternalSide(p,
                Vertex[0],
                Vertex[1],
                Vertex[2]) &&
            InternalSide(p,
                Vertex[1],
                Vertex[0],
                Vertex[2]) &&
            InternalSide(p,
                Vertex[2],
                Vertex[0],
                Vertex[1]))
                return true;
            else
                return false;
        }

        public override IntersectionInfo Intersect(Ray ray)
        {                        
            Vector v1 = Vertex[2] - ray.Pos;
            Vector v2 = ray.Dir;

            Vector normal = GetNormalFromVertexes();

            Real dot1 = Vector.DotProduct(normal, v1);
            Real dot2 = Vector.DotProduct(normal, v2);

            if (Math.Abs(dot2) < 1.0E-6)
                return new IntersectionInfo(HitResult.MISS); // division by 0 means parallel

            Real distance = dot1 / dot2;

            var hitPoint = ray.Pos + (ray.Dir * distance);
            if (!PointInTriangle(hitPoint))
                return new IntersectionInfo(HitResult.MISS);
            
            return new IntersectionInfo(HitResult.HIT, this, distance, hitPoint, hitPoint, GetNormal(hitPoint));
        }

        public override bool Intersect(AABB aabb)
        {
            return IntersectionCode2.triBoxOverlap(aabb.Center, aabb.HalfSize, Vertex);
        }

        public override Vector GetNormal(Vector vPoint)
        {
            if(this.Normal != null)
                return InterpolateNormal(vPoint);

            return GetNormalFromVertexes();
        }

        public Vector InterpolateNormal(Vector3 pointOfIntersection)
        {
            var a = Barycentric.CalculateBarycentricInterpolationVector(pointOfIntersection, this.Vertex);

            // find the uv corresponding to point f (uv1/uv2/uv3 are associated to p1/p2/p3):
            var n = this.Normal[0] * a.X +
                    this.Normal[1] * a.Y +
                    this.Normal[2] * a.Z;

            n.Normalize();

            return n;
        }

        private Vector GetNormalFromVertexes()
        {
            Vector u = Vertex[2] - Vertex[0];
            Vector w = Vertex[1] - Vertex[0];

            Vector c = Vector.CrossProduct(w, u);

            if (c.GetLengthSquared() == 0)
                return c;

            c.Normalize();
            return c;
        }

        public override AABB GetAABB()
        {
            if (!this.bounds.IsEmpty)
                return this.bounds;

            Vector min = new Vector(
                    Math.Min(Math.Min(Vertex[0].X, Vertex[1].X), Vertex[2].X),
                    Math.Min(Math.Min(Vertex[0].Y, Vertex[1].Y), Vertex[2].Y ) ,
                    Math.Min(Math.Min(Vertex[0].Z, Vertex[1].Z), Vertex[2].Z )
            );

            Vector max = new Vector(
                    Math.Max (Math.Max(Vertex[0].X, Vertex[1].X), Vertex[2].X ) ,
                    Math.Max (Math.Max(Vertex[0].Y, Vertex[1].Y), Vertex[2].Y ) ,
                    Math.Max (Math.Max(Vertex[0].Z, Vertex[1].Z), Vertex[2].Z )
            );

            this.bounds = new AABB(min, max);

            return this.bounds;
        }
    }
}
