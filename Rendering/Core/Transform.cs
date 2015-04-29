using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.MathTypes;

namespace Raytracer.Rendering.Core
{
    class Transform
    {
        private Matrix _transform;
        private Matrix _inverse;

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

        public Ray ToObjectSpace(Ray ray)
        {
            return new Ray(this._transform.Transform(ray.Pos),
                           this._transform.Transform(ray.Dir));
        }

        public Point ToObjectSpace(Point point)
        {
            return this._transform.Transform(point);
        }

        public IntersectionInfo ToWorldSpace(IntersectionInfo info)
        {
            return new IntersectionInfo(info.Result, 
                                        info.Primitive, 
                                        info.T, 
                                        info.HitPoint.Transform(this._inverse), 
                                        info.ObjectLocalHitPoint, 
                                        info.NormalAtHitPoint.Transform(this._inverse));
        }

        public AABB ToWorldSpace(AABB aabb)
        {
            return Apply(this._transform, aabb);
        }

        public AABB ToObjectSpace(AABB aabb)
        {
            return Apply(this._inverse, aabb);
        }

        private AABB Apply(Matrix m, AABB aabb)
        {
            var min = aabb.Min.Transform(m);
            var max = aabb.Max.Transform(m);

            return new AABB(min, max);
        }
    }
}
