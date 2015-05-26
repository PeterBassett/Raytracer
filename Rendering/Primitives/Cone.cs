using System;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.Primitives
{
    class Cone : ObjectSpacePrimitive
    {
        public readonly double Radius;
        public readonly double Height;
        public readonly double phiMax;
        public readonly Solidity Solid;
        private IntersectionInfo _missed = new IntersectionInfo(HitResult.Miss);

        public Cone(double radius, double height, double tm, Solidity solidity, Transform transform)
            : base(transform)
        {
            Radius = radius;
            Height = height;
            phiMax = MathLib.Deg2Rad(MathLib.Clamp(tm, 0.0, 360.0));
            Solid = solidity;
        }

        public override IntersectionInfo ObjectSpace_Intersect(Ray ray)
        {
            var i1 = IntersectWithCone(ray);

            if (Solid == Solidity.Solid)
                return i1;

            var i2 = IntersectWithEndCapDisk(ray);

            if (i1.Result == HitResult.Hit && i2.Result == HitResult.Hit)
            {
                if (i1.T < i2.T)
                    return i1;
                else
                    return i2;
            }

            if (i1.Result == HitResult.Hit)
                return i1;

            if (i2.Result == HitResult.Hit)
                return i2;

            return _missed;
        }

        private IntersectionInfo IntersectWithCone(Ray ray)
        {
            var k = Radius / Height;

            k = k * k;

            var A = ray.Dir.X * ray.Dir.X + ray.Dir.Y * ray.Dir.Y - k * ray.Dir.Z * ray.Dir.Z;
            var B = 2 * (ray.Dir.X * ray.Pos.X + ray.Dir.Y * ray.Pos.Y - k * ray.Dir.Z * (ray.Pos.Z - Height));
            var C = ray.Pos.X * ray.Pos.X + ray.Pos.Y * ray.Pos.Y - k * (ray.Pos.Z - Height) * (ray.Pos.Z - Height);

            var roots = new double[2];
            int rootCount = Algebra.SolveQuadraticEquation(A, B, C, roots);

            var t0 = roots[0];
            var t1 = roots[1];

            if (rootCount == 0)
                return _missed;

            var hitDistance = t0;

            var hitPoint = ray.Pos + ray.Dir * hitDistance;

            var phi = Math.Atan2(hitPoint.Y, hitPoint.X);

            phi = ClampToRadians(phi);

            if (hitPoint.Z < 0 || hitPoint.Z > Height || phi > phiMax)
            {
                if (hitDistance == t1)
                    return _missed;

                hitDistance = t1;

                hitPoint = ray.Pos + ray.Dir * hitDistance;

                phi = Math.Atan2(hitPoint.Y, hitPoint.X);

                phi = ClampToRadians(phi);

                if (hitPoint.Z < 0 || hitPoint.Z > Height || phi > phiMax)
                    return _missed;
            }

            return new IntersectionInfo(HitResult.Hit, this, hitDistance, hitPoint, hitPoint, GetNormal(hitPoint, ray));
        }

        IntersectionInfo IntersectWithEndCapDisk(Ray ray)
        {
            var missed = new IntersectionInfo(HitResult.Miss);

            if (Math.Abs(ray.Dir.Z) < MathLib.Epsilon)
                return missed;

            var u = -ray.Pos.Z / ray.Dir.Z;

            if (u < MathLib.Epsilon)
                return missed;

            var x = u * ray.Dir.X + ray.Pos.X;
            var y = u * ray.Dir.Y + ray.Pos.Y;
            var radial = x * x + y * y;

            if (radial > Radius * Radius)
                return missed;

            return new IntersectionInfo
            {
                HitPoint = new Point(x, y, 0.0),
                ObjectLocalHitPoint = new Point(x, y, 0.0),
                Result = HitResult.Hit,
                T = (u * ray.Dir).Length,
                NormalAtHitPoint = new Normal(0.0, 0.0, (ray.Pos.Z >= 0.0) ? 1.0 : -1.0),
                Primitive = this
            };
        }

        private static double ClampToRadians(double phi)
        {
            if (phi < 0.0)
                phi += 2.0 * MathLib.PI;

            return phi;
        }

        private Normal GetNormal(Point hit, Ray ray)
        {
            var n = new Normal(hit.X, hit.Y, 0).Normalize();

            n = new Normal(n.X, n.Y, -1).Normalize();

            return n;// -n.Faceforward(-ray.Dir);
        }

        public override bool ObjectSpace_Contains(Point point)
        {
            if (Solid != Solidity.Solid)
                return false;

            var slope = Height / Radius;

            var r2 = point.X * point.X + point.Y * point.Y;

            var h2 = point.Z * point.Z;

            return point.Z >= 0 && point.Z <= Height && h2 * slope <= 1 - r2;
        }

        public override AABB ObjectSpace_GetAABB()
        {
            return new AABB
            {
                Min = new Point(-Radius, -Radius, 0),
                Max = new Point(Radius, Radius, Height)
            };
        }
    }
}
