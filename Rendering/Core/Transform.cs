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

        public Transform Invert(Transform t)
        {
            return new Transform(t._inverse, t._transform);
        }

        public Vector GetObjectSpaceRotation()
        {
            return GetRotation(_transform);
        }

        public Vector GetRotation(Matrix m)
        {
            Vector scale;
            Quaternion rotation;
            Vector translation;
            m.Decompose(out scale, out rotation, out translation);

            return rotation.ToYawPitchRoll();
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

        public static Transform CreateLookAtTransform(Point eye, Point lookingAtPosition, Vector cameraUpVector)
		{
            var zaxis = (lookingAtPosition - eye).Normalize();
			var xaxis = Vector.CrossProduct(cameraUpVector, zaxis).Normalize();
			var yaxis = Vector.CrossProduct(zaxis, xaxis);

			var toWorld = new Matrix(
					xaxis.X, yaxis.X, zaxis.X, 0f,
					xaxis.Y, yaxis.Y, zaxis.Y, 0f,
					xaxis.Z, yaxis.Z, zaxis.Z, 0f,
					eye.X  , eye.Y  , eye.Z  , 1f);

			var inverse = toWorld;
			inverse.Invert();
			
			return new Transform(toWorld, inverse);
		}
		
        public Ray ToObjectSpace(Ray ray)
        {
            return new Ray(ray.Pos.Transform(_transform),
                           ray.Dir.Transform(_transform).Normalize());
        }

        public Ray ToWorldSpace(Ray ray)
        {
            return new Ray(ray.Pos.Transform(_inverse),
                           ray.Dir.Transform(_inverse).Normalize());
        }

        public Point ToObjectSpace(Point point)
        {
            return _transform.Transform(point);
        }

        public Point ToWorldSpace(Point point)
        {
            return _inverse.Transform(point);
        }

        public Vector ToObjectSpace(Vector vector)
        {
            return vector.Transform(_transform);
            //return _transform.Transform(vector);
        }

        public Vector ToWorldSpace(Vector vector)
        {
            return vector.Transform(_inverse);
            
            ///return _inverse.Transform(vector);
        }

        public Normal ToWorldSpace(Normal normal)
        {
            return normal.Transform(_inverse);
        }

        public IntersectionInfo ToWorldSpace(IntersectionInfo info)
        {
            return new IntersectionInfo(info.Result, 
                                        info.Primitive, 
                                        info.T, 
                                        info.HitPoint.Transform(_inverse), 
                                        info.ObjectLocalHitPoint,
                                        info.NormalAtHitPoint.Transform(_inverse).Normalize());
        }

        public IntersectionInfo ToObjectSpace(IntersectionInfo info)
        {
            return new IntersectionInfo(info.Result,
                                        info.Primitive,
                                        info.T,
                                        info.HitPoint.Transform(_transform),
                                        info.ObjectLocalHitPoint,
                                        info.NormalAtHitPoint.Transform(_transform).Normalize());
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
