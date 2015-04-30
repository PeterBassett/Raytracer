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

        public Cylinder(float radius, float height, Transform transform)
            : base(transform)
        {
            Radius = radius;
            Height = height;
        }

        public override IntersectionInfo ObjectSpace_Intersect(Ray ray)
        {
            var intersections = new IntersectionInfo[3];

            var direction = ray.Dir;
            var vantage = ray.Pos;

            var intersection = new IntersectionInfo();

            // Look for intersections with the disks on the top and/or bottom of the cylinder.
            if (Math.Abs(direction.Z) > MathLib.Epsilon)
            {
                intersections[0] = IntersectWithDisk(vantage, direction, +(Height / 2.0));
                intersections[1] = IntersectWithDisk(vantage, direction, -(Height / 2.0));
            }

            intersections[2] = IntersectionWithCylinder(direction, vantage);

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

        private IntersectionInfo IntersectionWithCylinder(Vector direction, Point vantage)
        {
            // Look for intersections with the curved lateral surface of the cylinder.
            var u = new double[2];

            var a = Radius;
            var b = (Height) / 2.0;

            int numRoots = Algebra.SolveQuadraticEquation(
                direction.X * direction.X + direction.Y * direction.Y,
                2.0 * (vantage.X * direction.X + vantage.Y * direction.Y),
                vantage.X * vantage.X + vantage.Y * vantage.Y - a * a,
                u
            );

            Array.Sort(u);

            for (int j = 0; j < numRoots; j++)
            {
                if (u[j] > MathLib.Epsilon)
                {
                    var intersection = new IntersectionInfo();

                    Vector displacement = u[j] * direction;
                    intersection.HitPoint = vantage + displacement;
                    intersection.ObjectLocalHitPoint = intersection.HitPoint;

                    // We found an intersection with the infinitely-extended lateral surface,
                    // but the z-component must be within + or - b.

                    if (Math.Abs(intersection.HitPoint.Z) <= b)
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

        /*const double u = (zDisk - vantage.z) / direction.z;
        if (u > EPSILON)
        {
            Intersection intersection;
            Vector displacement = u * direction;
            intersection.point = vantage + displacement;
            const double x = intersection.point.x;
            const double y = intersection.point.y;
            if (x*x + y*y <= a*a)
            {
                intersection.distanceSquared = displacement.MagnitudeSquared();
                intersection.surfaceNormal = Vector(0.0, 0.0, (zDisk > 0.0) ? +1.0 : -1.0);
                intersection.solid = this;

                intersectionList.push_back(intersection);
            }
        }*/

        IntersectionInfo IntersectWithDisk(Point vantage, Vector direction, double zDisk)
        {
            double u = (zDisk - vantage.Z) / direction.Z;

            if (u > MathLib.Epsilon)
            {
                var intersection = new IntersectionInfo();

                Vector displacement = u * direction;
                intersection.HitPoint = vantage + displacement;
                intersection.ObjectLocalHitPoint = intersection.HitPoint;

                double x = intersection.HitPoint.X;
                double y = intersection.HitPoint.Y;

                if (x * x + y * y <= Radius * Radius)
                {
                    intersection.Result = HitResult.HIT;
                    intersection.T = displacement.Length;
                    intersection.NormalAtHitPoint = new Normal(0.0, 0.0, (zDisk > 0.0) ? +1.0 : -1.0);
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
