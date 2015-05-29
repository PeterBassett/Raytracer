using System;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.Primitives
{
    class Cube : ObjectSpacePrimitive
    {
        private const double HalfWidth = 0.5; // plane distance for unit cube
        private const int AXIS_X = 0;
        private const int AXIS_Y = 1;
        private const int AXIS_Z = 2;

        public Cube(Transform transform)
            : base(transform)
        {
        }

        protected override IntersectionInfo ObjectSpaceIntersect(Ray ray)
        {
            var position = ray.Pos;
            var direction = ray.Dir;

            double t1, t2;
            double tNear;
            double tFar;
            int tNear_index = 0;
            int tFar_index = 0;

            if (ParallelWithPlane(ray, AXIS_X))
                return new IntersectionInfo(HitResult.Miss);

            CalculateNearAndFarIntersectionForAxis(ray, AXIS_X, out tNear, out tFar);

            if (tNear > tFar)
                Swap(ref tNear, ref tFar);

            if (InvalidIntersection(tNear, tFar))
                return new IntersectionInfo(HitResult.Miss);

            // intersect with the Y planes
            if (ParallelWithPlane(ray, AXIS_Y))
                return new IntersectionInfo(HitResult.Miss);

            CalculateNearAndFarIntersectionForAxis(ray, AXIS_Y, out t1, out t2);

            PickSmallestHitDistances(AXIS_Y, ref t1, ref t2, ref tNear, ref tFar, ref tNear_index, ref tFar_index);

            if (InvalidIntersection(tNear, tFar))
                return new IntersectionInfo(HitResult.Miss);

            // intersect with the Z planes
            if (ParallelWithPlane(ray, AXIS_Z))
                return new IntersectionInfo(HitResult.Miss);

            CalculateNearAndFarIntersectionForAxis(ray, AXIS_Z, out t1, out t2);

            PickSmallestHitDistances(AXIS_Z, ref t1, ref t2, ref tNear, ref tFar, ref tNear_index, ref tFar_index);

            if (InvalidIntersection(tNear, tFar))
                return new IntersectionInfo(HitResult.Miss);

            var normals = new[] { new Normal(1, 0, 0), new Normal(0, 1, 0), new Normal(0, 0, 1) };

            IntersectionInfo intersection;

            if (tNear <= 0)
            {
                if (tFar == 0)
                    return new IntersectionInfo(HitResult.Miss);

                var hitPoint = ray.Pos + ray.Dir * tFar;
                intersection = new IntersectionInfo(HitResult.Hit, this, tFar, hitPoint, hitPoint, normals[tFar_index]);
            }
            else
            {
                var hitPoint = ray.Pos + ray.Dir * tNear;
                intersection = new IntersectionInfo(HitResult.Hit, this, tNear, hitPoint, hitPoint, normals[tNear_index]);
            }

            if (Vector.DotProduct(intersection.NormalAtHitPoint, direction) > 0)
                intersection.NormalAtHitPoint = -intersection.NormalAtHitPoint;

            intersection.NormalAtHitPoint = intersection.NormalAtHitPoint.Normalize();

            return intersection;
        }

        private bool InvalidIntersection(double tNear, double tFar)
        {
            if (tNear > tFar)
                return true;
            if (tFar < 0)
                return true;

            return false;
        }

        private void PickSmallestHitDistances(int axis, ref double t1, ref double t2, ref double tNear, ref double tfar, ref int tNear_index, ref int tFar_index)
        {
            if (t1 > t2)
                Swap(ref t1, ref t2);

            if (t1 > tNear)
            {
                tNear = t1;
                tNear_index = axis;
            }

            if (t2 < tfar)
            {
                tfar = t2;
                tFar_index = axis;
            }
        }

        private bool ParallelWithPlane(Ray ray, int axis)
        {
            return ray.Dir[axis] == 0 && (ray.Pos[axis] < -HalfWidth || ray.Pos[axis] > HalfWidth);
        }

        private void CalculateNearAndFarIntersectionForAxis(Ray ray, int axis, out double tNear, out double tfar)
        {
            tNear = (-HalfWidth - ray.Pos[axis]) / ray.Dir[axis];
            tfar = (HalfWidth - ray.Pos[axis]) / ray.Dir[axis];
        }

        protected override bool ObjectSpaceContains(Point point)
        {
            return ObjectSpaceGetAABB().Contains(point);
        }

        protected override AABB ObjectSpaceGetAABB()
        {
            var offset = new Vector(0.5, 0.5, 0.5);

            return new AABB
            {
                Min = Pos - offset,
                Max = Pos + offset
            };
        }

        private void Swap<T>(ref T a, ref T b)
        {
            T i = a;
            a = b;
            b = i;
        }
    }
}
