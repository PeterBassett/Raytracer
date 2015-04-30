using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.Primitives
{
    class Cylinder : ObjectSpacePrimitive
    {
        public float Radius;
        public float Height;
        public float PhiMax;
        private float _halfHeight;

        public Cylinder(float radius, float height, Transform transform)
            : base(transform)
        {
            Radius = radius;
            Height = height;
            _halfHeight = Height / 2.0f;
        }

        public override IntersectionInfo ObjectSpace_Intersect(Ray ray)
        {
            var intersections = new IntersectionInfo[3];

            // Look for intersections with the disks on the top and/or bottom of the cylinder.
            if (Math.Abs(ray.Dir.Z) > MathLib.Epsilon)
            {
                intersections[0] = IntersectWithDisk(ray, +(Height / 2.0));
                intersections[1] = IntersectWithDisk(ray, -(Height / 2.0));
            }

            intersections[2] = IntersectionWithCylinder(ray);

            var closestIntersectionIndex = -1;
            var smallestDistance = double.MaxValue;
            for (int i = 0; i < 3; i++)
            {
                if(intersections[i].Result == HitResult.HIT)
                    if(intersections[i].T < smallestDistance)
                    {
                        smallestDistance = intersections[i].T;
                        closestIntersectionIndex = i;
                    }
            }

            if (closestIntersectionIndex != -1)
                return intersections[closestIntersectionIndex];
            else
                return new IntersectionInfo(HitResult.MISS);
        }

        private IntersectionInfo IntersectionWithCylinder(Ray ray)
        {
            var roots = new double[2];

            var vX = ray.Pos.X;
            var vY = ray.Pos.Y;
            var dX = ray.Dir.X;
            var dY = ray.Dir.Y;

            int numRoots = Algebra.SolveQuadraticEquation(
                dX * dX + dY * dY,
                2.0 * (vX * dX + vY * dY),
                vX * vX + vY * vY - Radius * Radius,
                roots
            );

            Array.Sort(roots);

            for (int j = 0; j < numRoots; j++)
            {
                if (roots[j] > MathLib.Epsilon)
                {
                    var intersection = new IntersectionInfo();

                    Vector displacement = roots[j] * ray.Dir;
                    intersection.HitPoint = ray.Pos + displacement;
                    intersection.ObjectLocalHitPoint = intersection.HitPoint;

                    if (Math.Abs(intersection.HitPoint.Z) <= _halfHeight)
                    {
                        intersection.Result = HitResult.HIT;
                        intersection.T = displacement.Length;

                        intersection.NormalAtHitPoint = new Normal(intersection.HitPoint.X, intersection.HitPoint.Y, 0.0).Normalize();
                        intersection.Primitive = this;

                        return intersection;
                    }
                }
            }

            return new IntersectionInfo(HitResult.MISS);
        }

        IntersectionInfo IntersectWithDisk(Ray ray, double zCoordOfDisk)
        {
            double u = (zCoordOfDisk - ray.Pos.Z) / ray.Dir.Z;

            if (u > MathLib.Epsilon)
            {
                var intersection = new IntersectionInfo();

                Vector displacement = u * ray.Dir;
                intersection.HitPoint = ray.Pos + displacement;
                intersection.ObjectLocalHitPoint = intersection.HitPoint;

                double x = intersection.HitPoint.X;
                double y = intersection.HitPoint.Y;

                if (x * x + y * y <= Radius * Radius)
                {
                    intersection.Result = HitResult.HIT;
                    intersection.T = displacement.Length;
                    intersection.NormalAtHitPoint = new Normal(0.0, 0.0, (zCoordOfDisk > 0.0) ? +1.0 : -1.0);
                    intersection.Primitive = this;

                    return intersection;
                }
            }

            return new IntersectionInfo(HitResult.MISS);
        }

        public override bool ObjectSpace_Contains(Point point)
        {
            var length = new Vector2(point.X, point.Y).GetLength();

            if(length > Radius)
                return false;

            var halfHeight = (Height / 2.0);
            return point.Z >= -halfHeight && point.Z <= halfHeight;
        }

        public override AABB ObjectSpace_GetAABB()
        {
            return new AABB
            {
                Min = new Point(-Radius, -Radius, -(Height / 2.0)),
                Max = new Point(Radius, Radius, Height / 2.0)
            };
        }
    }
}
