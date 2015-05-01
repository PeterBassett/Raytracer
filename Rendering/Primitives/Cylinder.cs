using System;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.Primitives
{
    class Cylinder : ObjectSpacePrimitive
    {
        public readonly float Radius;
        public readonly float Height;
        private readonly float _halfHeight;
        private readonly float  _radiusSquared;
        private IntersectionInfo _missed = new IntersectionInfo(HitResult.MISS);

        public Cylinder(float radius, float height, Transform transform)
            : base(transform)
        {
            Radius = radius;
            Height = height;
            _halfHeight = Height / 2.0f;
            _radiusSquared = radius*radius;
        }

        public override IntersectionInfo ObjectSpace_Intersect(Ray ray)
        {
            var allPossibleIntersections = new IntersectionInfo[3];
           
            if (CanIntersectionWithEndCapDisks(ray))
            {
                allPossibleIntersections[0] = IntersectWithEndCapDisk(ray, +(Height / 2.0));
                allPossibleIntersections[1] = IntersectWithEndCapDisk(ray, -(Height / 2.0));
            }

            allPossibleIntersections[2] = IntersectionWithCylinder(ray);

            return FindClosestIntersection(allPossibleIntersections);
        }

        private static bool CanIntersectionWithEndCapDisks(Ray ray)
        {
            return Math.Abs(ray.Dir.Z) > MathLib.Epsilon;
        }

        IntersectionInfo IntersectWithEndCapDisk(Ray ray, double zCoordOfDisk)
        {
            var u = (zCoordOfDisk - ray.Pos.Z) / ray.Dir.Z;

            if (u < MathLib.Epsilon)
                return _missed;            

            var displacement = u * ray.Dir;
            var hitpoint = ray.Pos + displacement;

            if (hitpoint.X * hitpoint.X +
                hitpoint.Y * hitpoint.Y > _radiusSquared)
                return _missed;

            var normalAtHitPoint = new Normal(0.0, 0.0, (zCoordOfDisk > 0.0) ? +1.0 : -1.0);

            return new IntersectionInfo(HitResult.HIT, this, displacement.Length, hitpoint, hitpoint, normalAtHitPoint);
        }

        private IntersectionInfo FindClosestIntersection(IntersectionInfo[] intersections)
        {
            var closestIntersectionIndex = -1;
            var smallestDistance = double.MaxValue;

            for (var i = 0; i < intersections.Length; i++)
            {
                if (intersections[i].Result != HitResult.HIT) 
                    continue;

                if (intersections[i].T >= smallestDistance)
                    continue;

                smallestDistance = intersections[i].T;
                closestIntersectionIndex = i;
            }

            if (closestIntersectionIndex != -1)
                return intersections[closestIntersectionIndex];

            return _missed;
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
                vX * vX + vY * vY - _radiusSquared,
                roots
            );

            Array.Sort(roots);

            for (int j = 0; j < numRoots; j++)
            {
                if (roots[j] < MathLib.Epsilon)
                    continue;

                Vector displacement = roots[j] * ray.Dir;
                var hitPoint = ray.Pos + displacement;

                if (Math.Abs(hitPoint.Z) > _halfHeight)
                    continue;

                var normalAtHitPoint = new Normal(hitPoint.X, hitPoint.Y, 0.0).Normalize();
                
                return new IntersectionInfo(HitResult.HIT, this, displacement.Length, hitPoint, hitPoint, normalAtHitPoint);
            }

            return _missed;
        }

        public override bool ObjectSpace_Contains(Point point)
        {
            var length = new Vector2(point.X, point.Y).LengthSquared;

            if (length > _radiusSquared)
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
