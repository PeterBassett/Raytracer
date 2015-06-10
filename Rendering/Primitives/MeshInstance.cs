using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Materials;

namespace Raytracer.Rendering.Primitives
{
    class MeshInstance : ObjectSpacePrimitive
    {
        private Material _instanceMaterial;

        public MeshInstance(Mesh mesh, Transform transform, Material instanceMaterial)
            : base(transform)
        {
            _mesh = mesh;
            _instanceMaterial = instanceMaterial;
        }

        private AABB _bounds = AABB.Empty;
        private Mesh _mesh { get; set; }

        protected override IntersectionInfo ObjectSpaceIntersect(Ray ray)
        {
            var result = _mesh.Intersect(ray);

            if (result.Result != HitResult.Miss)
            {
                result.HitPoint = ray.Pos + (ray.Dir * result.T);

                if (_instanceMaterial != null)
                    result.Material = _instanceMaterial;
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

        public override Material Material
        {
            get
            {
                if(_instanceMaterial == null)
                    return base.Material;

                return _instanceMaterial;
            }
            set
            {
                //base.Material = value;
            }
        }
    }
}
