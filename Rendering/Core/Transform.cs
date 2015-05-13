using Raytracer.MathTypes;

namespace Raytracer.Rendering.Core
{
    class Transform
    {
        private readonly Matrix _transform;
        private readonly Matrix _inverse;

        public Transform(Matrix transform, Matrix inverse)
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

        public static Transform CreateLookAtTransform(Point cameraPosition, Point lookingAtPosition, Vector cameraUpVector)
		{
            var zaxis = (cameraPosition - lookingAtPosition).Normalize();
			var xaxis = Vector.CrossProduct(cameraUpVector, zaxis).Normalize();
			var yaxis = Vector.CrossProduct(zaxis, xaxis);

			var toWorld = new Matrix(
					xaxis.X,                        yaxis.X,                        zaxis.X,                        0f,
					xaxis.Y,                        yaxis.Y,                        zaxis.Y,                        0f,
					xaxis.Z,                        yaxis.Z,                        zaxis.Z,                        0f,
					eye.X  ,                        eye.Y  ,                        eye.Z  ,                        1f);
		/*			
			var toWorld = new Matrix(
					xaxis.X,                        xaxis.Y,                        xaxis.Z,                        0f,
					yaxis.X,                        yaxis.Y,                        yaxis.Z,                        0f,
					zaxis.X,                        zaxis.Y,                        zaxis.Z,                        0f,
					eye.X  ,                        eye.Y  ,                        eye.Z  ,                        1f);
		*/			
			var inverse = toWorld;
			inverse.Invert();
			
			return new Transform(toWorld, inverse);
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
