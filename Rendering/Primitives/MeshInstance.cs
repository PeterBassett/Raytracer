using System;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.Primitives
{
    using Vector = Vector3;

    class MeshInstance : Traceable
    {
        private AABB bounds = AABB.Empty;
        public Mesh Mesh { get; set; }
        public override IntersectionInfo Intersect(Ray ray)
        {
            var transformedRay = CreateTransformedRay(ray);

            var result = Mesh.Intersect(transformedRay);

            if (result.Result != HitResult.MISS)
            {
                result.HitPoint = ray.Pos + (ray.Dir * result.T); // store the UNtranformed hitpotin
            }

            return result;
        }

        private Ray CreateTransformedRay(Ray ray)
        {
            Vector dir = ray.Pos + -this.Pos;
            ray.Dir.RotateX(-this.Ori.X, ref dir);
            dir.RotateY(-this.Ori.Y, ref dir);
            dir.RotateZ(-this.Ori.Z, ref dir);
            //dir.Normalize();
            
            return new Ray(ray.Pos + -this.Pos, dir);            
        }

        public override bool Intersect(AABB aabb)
        {
            return GetAABB().Intersect(aabb);
        }

        public override AABB GetAABB()
        {
            if (bounds.IsEmpty)
            {
                var meshaabb = Mesh.GetAABB();

                bounds = new AABB()
                {
                    Min = meshaabb.Min + this.Pos,
                    Max = meshaabb.Max + this.Pos
                };
            }

            return bounds;
        }

        public override bool Contains(Vector point)
        {
            return false;
        }
    }
}
