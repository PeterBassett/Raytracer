using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.MathTypes;
using Raytracer.Rendering.Accellerators;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Accellerators.Partitioners;

namespace Raytracer.Rendering.Primitives
{
    class Mesh : Traceable
    {
        public string Name { get; set; }

        List<Triangle> Triangles = null;
        AABB bounds;
        
        IAccelerator bvh = null;

        public Mesh(List<Triangle> triangles)
        {
            Triangles = triangles;

            BuildAABB();

            if(TransformToOrigin())
                BuildAABB();

            bvh = new AABBHierarchy(new EqualPrimitivePartioner());
            bvh.Build(triangles);
        }

        private bool TransformToOrigin()
        {
            var aabb = GetAABB();

            var trans = aabb.Min + ((aabb.Max - aabb.Min) / 2f);

            if (trans.X == 0 &&
                trans.Y == 0 &&
                trans.Z == 0)
                return false;

            foreach (var tri in Triangles)
            {
                for (int i = 0; i < 3; i++)
                {
                    tri.Vertices[i] = (Point)(tri.Vertices[i] - trans);
                }
            }

            return true;
        }
        
        private void BuildAABB()
        {
            var min = new Point(int.MaxValue, int.MaxValue, int.MaxValue);
            var max = new Point(int.MinValue, int.MinValue, int.MinValue);

            foreach (var tri in Triangles)
            {
                for (int i = 0; i < 3; i++)
                {
                    // minimum
                    if (tri.Vertices[i].X < min.X)
                        min.X = tri.Vertices[i].X;

                    if (tri.Vertices[i].Y < min.Y)
                        min.Y = tri.Vertices[i].Y;

                    if (tri.Vertices[i].Z < min.Z)
                        min.Z = tri.Vertices[i].Z;

                    // maximum
                    if (tri.Vertices[i].X > max.X)
                        max.X = tri.Vertices[i].X;

                    if (tri.Vertices[i].Y > max.Y)
                        max.Y = tri.Vertices[i].Y;

                    if (tri.Vertices[i].Z > max.Z)
                        max.Z = tri.Vertices[i].Z;
                }
            }

            bounds = new AABB(min, max);
        }

        public override IntersectionInfo Intersect(Ray ray)
        {                        
            if (!this.GetAABB().Intersect(ray))
                return new IntersectionInfo(HitResult.MISS);

            return GetMinimumValidIntersection(ray);
        }

        private IntersectionInfo GetMinimumValidIntersection(Ray ray)
        {
            IntersectionInfo minimumIntersection = new IntersectionInfo(HitResult.MISS);

            foreach (var tri in GetCandidates(ray))
            {
                var result = tri.Intersect(ray);

                if (result.T > 0f && result.T < minimumIntersection.T && result.Result != HitResult.MISS)
                    minimumIntersection = result;
            }
            return minimumIntersection;
        }

        private IEnumerable<Traceable> GetCandidates(Ray ray)
        {           
            if(bvh != null)
                return bvh.Intersect(ray);
            else
                return Triangles;
        }

        public override AABB GetAABB()
        {
            return bounds;
        }

        public override bool Contains(Point point)
        {
            var ray = new Ray(point, new Vector(0, 1, 0));

            int intersections = 0;

            while (true)
            {
                var intersection = GetMinimumValidIntersection(ray);

                if (intersection.Result == HitResult.MISS)
                    return intersections % 2 == 0;

                intersections++;

                ray = new Ray(intersection.HitPoint + ray.Dir * MathLib.Epsilon, ray.Dir);
            }
        }
    }
}