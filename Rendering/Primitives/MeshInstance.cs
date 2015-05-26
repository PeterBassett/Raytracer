using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.Primitives
{
    class MeshInstance : ObjectSpacePrimitive
    {
        public MeshInstance(Mesh mesh, Transform transform)
            : base(transform)
        {
            _mesh = mesh;
        }

        private AABB _bounds = AABB.Empty;
        private Mesh _mesh { get; set; }

        protected override IntersectionInfo ObjectSpaceIntersect(Ray ray)
        {
            var result = _mesh.Intersect(ray);

            if (result.Result != HitResult.Miss)
            {
                result.HitPoint = ray.Pos + (ray.Dir * result.T);
            }

            return result;
        }

        protected override bool ObjectSpaceContains(Point point)
        {
            return _mesh.Contains(point);
        }

        protected override AABB ObjectSpaceGetAABB()
        {
            if (_bounds.IsEmpty)
            {
                var meshaabb = _mesh.GetAABB();

                _bounds = new AABB
                {
                    Min = meshaabb.Min + Pos,
                    Max = meshaabb.Max + Pos
                };
            }

            return _bounds;
        }
    }
}
