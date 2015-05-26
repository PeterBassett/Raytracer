using System;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.Primitives
{   
    class Triangle : Traceable
    {
        public readonly Point[] Vertices = new Point[3];
        private AABB _bounds = AABB.Empty;
        public Vector2[] TextureUVs = new Vector2[3];
        public Normal[] Normals = new Normal[3];
        private Normal _cachedFaceNormal = Normal.Invalid;

        private bool InternalSide(Point p1,
                                Point p2,
                                Point a,
                                Point b)
        {
            var cp1 = Vector.CrossProduct(b - a, p1 - a);

            var cp2 = Vector.CrossProduct(b - a, p2 - a);

            if (Vector.DotProduct(cp1, cp2) >= 0)
                return true;
            
            return false;
        }

        private bool PointInTriangle(Point p)
        {
            if (InternalSide(p,
                Vertices[0],
                Vertices[1],
                Vertices[2]) &&
            InternalSide(p,
                Vertices[1],
                Vertices[0],
                Vertices[2]) &&
            InternalSide(p,
                Vertices[2],
                Vertices[0],
                Vertices[1]))
                return true;
            
            return false;
        }

        public override IntersectionInfo Intersect(Ray ray)
        {                        
            var v1 = Vertices[2] - ray.Pos;
            var v2 = ray.Dir;

            var normal = GetNormalFromVertexes();

            var dot1 = Vector.DotProduct(normal, v1);
            var dot2 = Vector.DotProduct(normal, v2);

            if (Math.Abs(dot2) < 1.0E-6)
                return new IntersectionInfo(HitResult.Miss); // division by 0 means parallel

            double distance = dot1 / dot2;

            var hitPoint = ray.Pos + (ray.Dir * distance);
            if (!PointInTriangle(hitPoint))
                return new IntersectionInfo(HitResult.Miss);
            
            return new IntersectionInfo(HitResult.Hit, this, distance, hitPoint, hitPoint, GetNormal(hitPoint));
        }

        private Normal GetNormal(Point vPoint)
        {
            if(Normals != null)
                return InterpolateNormal(vPoint);

            return GetNormalFromVertexes();
        }

        private Normal InterpolateNormal(Point pointOfIntersection)
        {
            var a = Barycentric.CalculateBarycentricInterpolationVector(pointOfIntersection, Vertices);

            // find the uv corresponding to point f (uv1/uv2/uv3 are associated to p1/p2/p3):
            var n = Normals[0] * a.X +
                    Normals[1] * a.Y +
                    Normals[2] * a.Z;

            return n.Normalize();
        }
        
        private Normal GetNormalFromVertexes()
        {
            if (_cachedFaceNormal != Normal.Invalid)
                return _cachedFaceNormal;
         
            var u = Vertices[2] - Vertices[0];
            var w = Vertices[1] - Vertices[0];

            var norm = (Normal)Vector.CrossProduct(w, u);

            if (norm.LengthSquared == 0)
                return norm;

            _cachedFaceNormal = norm.Normalize();

            return _cachedFaceNormal;
        }

        public override AABB GetAABB()
        {
            if (!_bounds.IsEmpty)
                return _bounds;

            var min = new Point(
                    Math.Min(Math.Min(Vertices[0].X, Vertices[1].X), Vertices[2].X),
                    Math.Min(Math.Min(Vertices[0].Y, Vertices[1].Y), Vertices[2].Y ) ,
                    Math.Min(Math.Min(Vertices[0].Z, Vertices[1].Z), Vertices[2].Z )
            );

            var max = new Point(
                    Math.Max (Math.Max(Vertices[0].X, Vertices[1].X), Vertices[2].X ) ,
                    Math.Max (Math.Max(Vertices[0].Y, Vertices[1].Y), Vertices[2].Y ) ,
                    Math.Max (Math.Max(Vertices[0].Z, Vertices[1].Z), Vertices[2].Z )
            );

            _bounds = new AABB(min, max);

            return _bounds;
        }

        public override bool Contains(Point point)
        {
            return false;
        }
    }
}
