using System;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.Primitives
{
    class MeshInstance : ObjectSpacePrimitive
    {
        public MeshInstance(Mesh mesh, Transform transform)
            : base(transform)
        {
            Mesh = mesh;
        }

        private AABB bounds = AABB.Empty;
        public Mesh Mesh { get; set; }

        public override IntersectionInfo ObjectSpace_Intersect(Ray ray)
        {
            var result = Mesh.Intersect(ray);

            if (result.Result != HitResult.MISS)
            {
                result.HitPoint = ray.Pos + (ray.Dir * result.T); // store the UNtranformed hitpotin
            }

            return result;
        }

        public override bool ObjectSpace_Intersect(AABB aabb)
        {
            return this.GetAABB().Intersect(aabb);
        }

        public override bool ObjectSpace_Contains(Point point)
        {
            throw new NotImplementedException();
        }

        public override AABB ObjectSpace_GetAABB()
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
    }
}
