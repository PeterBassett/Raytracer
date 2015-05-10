using Raytracer.MathTypes;

namespace Raytracer.Rendering.Core
{
    class Transform
    {
        private readonly Matrix _transform;
        private readonly Matrix _inverse;

        private Transform(Matrix transform, Matrix inverse)
        {
            _transform = transform;
            _inverse = inverse;
        }

        public static Transform CreateTransform(Vector translate, Vector rotate)
        {
            var transform = Matrix.CreateTranslation(translate.X, translate.Y, translate.Z) *
                            Matrix.CreateRotationX(MathLib.Deg2Rad(rotate.X)) * 
                            Matrix.CreateRotationY(MathLib.Deg2Rad(rotate.Y)) *
                            Matrix.CreateRotationZ(MathLib.Deg2Rad(rotate.Z));

            var inverse = transform;
            inverse.Invert();

            return new Transform(transform, inverse);
        }

        public Ray ToObjectSpace(Ray ray)
        {
            return new Ray(_transform.Transform(ray.Pos),
                           _transform.Transform(ray.Dir));
        }

        public Point ToObjectSpace(Point point)
        {
            return _transform.Transform(point);
        }

        public Point ToWorldSpace(Point point)
        {
            return _inverse.Transform(point);
        }

        public Vector ToObjectSpace(Vector point)
        {
            return _transform.Transform(point);
        }

        public Vector ToWorldSpace(Vector point)
        {
            return _inverse.Transform(point);
        }

        public IntersectionInfo ToWorldSpace(IntersectionInfo info)
        {
            return new IntersectionInfo(info.Result, 
                                        info.Primitive, 
                                        info.T, 
                                        info.HitPoint.Transform(_inverse), 
                                        info.ObjectLocalHitPoint, 
                                        info.NormalAtHitPoint.Transform(_inverse));
        }

        public AABB ToObjectSpace(AABB aabb)
        {
            return Apply(_transform, aabb);
        }

        public AABB ToWorldSpace(AABB aabb)
        {
            return Apply(_inverse, aabb);
        }

        private AABB Apply(Matrix m, AABB aabb)
        {
            return aabb.Transform(m);
        }

        internal static Transform CreateIdentityTransform()
        {
            var identity = Matrix.Identity;
            return new Transform(identity, identity);
        }
    }
}
