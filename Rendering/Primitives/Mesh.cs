using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.MathTypes;
using Raytracer.Rendering.Accellerators;

namespace Raytracer.Rendering.Primitives
{
    using Vector = Vector3;
    using Real = System.Double;

    class Mesh : Traceable
    {
        public string Name { get; set; }

        List<Triangle> Triangles = null;
        AABB bounds;
        //Octree bvh = null;
        BVH bvh = null;

        public Mesh(List<Triangle> triangles)
        {
            Triangles = triangles;

            BuildAABB();

            if(TransformToOrigin())
                BuildAABB();

            bvh = new BVH(triangles);
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
                    tri.Vertex[i] = tri.Vertex[i] - trans;
                }
            }

            return true;
        }
        
        private void BuildAABB()
        {
            Vector min = new Vector(int.MaxValue, int.MaxValue, int.MaxValue);
            Vector max = new Vector(int.MinValue, int.MinValue, int.MinValue);

            foreach (var tri in Triangles)
            {
                for (int i = 0; i < 3; i++)
                {
                    // minimum
                    if (tri.Vertex[i].X < min.X)
                        min.X = tri.Vertex[i].X;

                    if (tri.Vertex[i].Y < min.Y)
                        min.Y = tri.Vertex[i].Y;

                    if (tri.Vertex[i].Z < min.Z)
                        min.Z = tri.Vertex[i].Z;

                    // maximum
                    if (tri.Vertex[i].X > max.X)
                        max.X = tri.Vertex[i].X;

                    if (tri.Vertex[i].Y > max.Y)
                        max.Y = tri.Vertex[i].Y;

                    if (tri.Vertex[i].Z > max.Z)
                        max.Z = tri.Vertex[i].Z;
                }
            }

            bounds = new AABB(min, max);
        }

        public override IntersectionInfo Intersect(Ray ray)
        {                        
            if (!this.GetAABB().Intersect(ray))
                return new IntersectionInfo(HitResult.MISS);

            IntersectionInfo minimumIntersection = new IntersectionInfo(HitResult.MISS);

            foreach (var tri in GetCandidates(ray))
            {                
                var result = tri.Intersect(ray);

                if (result.T < minimumIntersection.T && result.Result != HitResult.MISS)
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

        public override Vector GetNormal(Vector vPoint)
        {
            throw new NotImplementedException();
        }

        public override AABB GetAABB()
        {
            return bounds;
        }

        public override bool Intersect(AABB aabb)
        {
            return bounds.Intersect(aabb);
        }
    }
}