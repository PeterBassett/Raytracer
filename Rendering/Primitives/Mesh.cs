using System.Collections.Generic;
using Raytracer.MathTypes;
using Raytracer.Rendering.Accellerators;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Accellerators.Partitioners;

namespace Raytracer.Rendering.Primitives
{
    class Mesh : Traceable
    {
        public string Name { get; set; }
        readonly List<Triangle> _triangles;
        private AABB _bounds;
        private readonly IAccelerator _bvh;

        public Mesh(List<Triangle> triangles)
        {
            _triangles = triangles;

            BuildAABB();

            if(TransformToOrigin())
                BuildAABB();

           // bvh = new AABBFlattenedHierarchy(new SAHMutliAxisPrimitivePartitioner());
            _bvh = new AABBHierarchy(new SahMutliAxisPrimitivePartitioner());            
            _bvh.Build(triangles);
        }

        private bool TransformToOrigin()
        {
            var aabb = GetAABB();

            var trans = aabb.Min + ((aabb.Max - aabb.Min) / 2f);

            if (trans.X == 0 &&
                trans.Y == 0 &&
                trans.Z == 0)
                return false;

            foreach (var tri in _triangles)
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

            foreach (var tri in _triangles)
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

            _bounds = new AABB(min, max);
        }

        public override IntersectionInfo Intersect(Ray ray)
        {                        
            if (!GetAABB().Intersect(ray))
                return new IntersectionInfo(HitResult.Miss);

            return GetMinimumValidIntersection(ray);
        }

        private IntersectionInfo GetMinimumValidIntersection(Ray ray)
        {
            var minimumIntersection = new IntersectionInfo(HitResult.Miss);

            foreach (var tri in GetCandidates(ray))
            {
                var result = tri.Intersect(ray);

                if (result.T > 0f && result.T < minimumIntersection.T && result.Result != HitResult.Miss)
                    minimumIntersection = result;
            }
            return minimumIntersection;
        }

        private IEnumerable<Traceable> GetCandidates(Ray ray)
        {
            if(_bvh != null)
                return _bvh.Intersect(ray);
            
            return _triangles;
        }

        public override AABB GetAABB()
        {
            return _bounds;
        }

        public override bool Contains(Point point)
        {
            if(!GetAABB().Contains(point))
                return false;
                
            var ray = new Ray(point, new Vector(0, 1, 0));

            int intersections = 0;

            foreach (var tri in GetCandidates(ray))
            {
                var result = tri.Intersect(ray);

                if (result.T > 0f && result.Result != HitResult.Miss)
                     intersections++;
            }

            return intersections % 2 == 0;
        }
    }
}
