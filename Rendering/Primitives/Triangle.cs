using System;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.Primitives
{   
    class Triangle : Traceable
    {
        public Vector3[] Vertex = new Vector3[3];
        private AABB bounds = AABB.Empty;
        public Vector2[] Texture = new Vector2[3];
        public Vector3[] Normal = new Vector3[3];        

        private bool InternalSide(Vector3 p1,
                                Vector3 p2,
                                Vector3 a,
                                Vector3 b)
        {
            var cp1 = Vector3.CrossProduct(b - a, p1 - a);

            var cp2 = Vector3.CrossProduct(b - a, p2 - a);

            if (Vector3.DotProduct(cp1, cp2) >= 0)
                return true;
            else
                return false;
        }

        private bool PointInTriangle(Vector3 p)
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
            var v1 = Vertex[2] - ray.Pos;
            var v2 = ray.Dir;

            var normal = GetNormalFromVertexes();

            var dot1 = Vector3.DotProduct(normal, v1);
            var dot2 = Vector3.DotProduct(normal, v2);

            if (Math.Abs(dot2) < 1.0E-6)
                return new IntersectionInfo(HitResult.MISS); // division by 0 means parallel

            double distance = dot1 / dot2;

            var hitPoint = ray.Pos + (ray.Dir * distance);
            if (!PointInTriangle(hitPoint))
                return new IntersectionInfo(HitResult.MISS);
            
            return new IntersectionInfo(HitResult.HIT, this, distance, hitPoint, hitPoint, GetNormal(hitPoint));
        }

        public override bool Intersect(AABB aabb)
        {
            return IntersectionCode2.triBoxOverlap(aabb.Center, aabb.HalfSize, Vertex);
        }

        public Vector3 GetNormal(Vector3 vPoint)
        {
            if(this.Normal != null)
                return InterpolateNormal(vPoint);

            return GetNormalFromVertexes();
        }

        public Vector3 InterpolateNormal(Vector3 pointOfIntersection)
        {
            var a = Barycentric.CalculateBarycentricInterpolationVector(pointOfIntersection, this.Vertex);

            // find the uv corresponding to point f (uv1/uv2/uv3 are associated to p1/p2/p3):
            var n = this.Normal[0] * a.X +
                    this.Normal[1] * a.Y +
                    this.Normal[2] * a.Z;

            n.Normalize();

            return n;
        }

        private Vector3 GetNormalFromVertexes()
        {
            var u = Vertex[2] - Vertex[0];
            var w = Vertex[1] - Vertex[0];

            var c = Vector3.CrossProduct(w, u);

            if (c.GetLengthSquared() == 0)
                return c;

            c.Normalize();
            return c;
        }

        public override AABB GetAABB()
        {
            if (!this.bounds.IsEmpty)
                return this.bounds;

            var min = new Vector3(
                    Math.Min(Math.Min(Vertex[0].X, Vertex[1].X), Vertex[2].X),
                    Math.Min(Math.Min(Vertex[0].Y, Vertex[1].Y), Vertex[2].Y ) ,
                    Math.Min(Math.Min(Vertex[0].Z, Vertex[1].Z), Vertex[2].Z )
            );

            var max = new Vector3(
                    Math.Max (Math.Max(Vertex[0].X, Vertex[1].X), Vertex[2].X ) ,
                    Math.Max (Math.Max(Vertex[0].Y, Vertex[1].Y), Vertex[2].Y ) ,
                    Math.Max (Math.Max(Vertex[0].Z, Vertex[1].Z), Vertex[2].Z )
            );

            this.bounds = new AABB(min, max);

            return this.bounds;
        }

        public override bool Contains(Vector3 point)
        {
            return false;
        }
    }
}
