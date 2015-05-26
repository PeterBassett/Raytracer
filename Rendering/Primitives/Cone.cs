using System;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.Primitives
{
    class Cone : ObjectSpacePrimitive
    {
        private readonly double _radius;
        private readonly double _height;
        private readonly double _phiMax;
        private readonly Solidity _solid;
        private readonly IntersectionInfo _missed = new IntersectionInfo(HitResult.Miss);

        public Cone(double radius, double height, double tm, Solidity solidity, Transform transform)
            : base(transform)
        {
            _radius = radius;
            _height = height;
            _phiMax = MathLib.Deg2Rad(MathLib.Clamp(tm, 0.0, 360.0));
            _solid = solidity;
        }

        protected override IntersectionInfo ObjectSpaceIntersect(Ray ray)
        {
            var i1 = IntersectWithCone(ray);

            if (_solid == Solidity.Solid)
                return i1;

            var i2 = IntersectWithEndCapDisk(ray);

            if (i1.Result == HitResult.Hit && i2.Result == HitResult.Hit)
            {
                if (i1.T < i2.T)
                    return i1;
                
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
            var k = _radius / _height;

            k = k * k;

            var A = ray.Dir.X * ray.Dir.X + ray.Dir.Y * ray.Dir.Y - k * ray.Dir.Z * ray.Dir.Z;
            var B = 2 * (ray.Dir.X * ray.Pos.X + ray.Dir.Y * ray.Pos.Y - k * ray.Dir.Z * (ray.Pos.Z - _height));
            var C = ray.Pos.X * ray.Pos.X + ray.Pos.Y * ray.Pos.Y - k * (ray.Pos.Z - _height) * (ray.Pos.Z - _height);

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

            if (hitPoint.Z < 0 || hitPoint.Z > _height || phi > _phiMax)
            {
                if (hitDistance == t1)
                    return _missed;

                hitDistance = t1;

                hitPoint = ray.Pos + ray.Dir * hitDistance;

                phi = Math.Atan2(hitPoint.Y, hitPoint.X);

                phi = ClampToRadians(phi);

                if (hitPoint.Z < 0 || hitPoint.Z > _height || phi > _phiMax)
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

            if (radial > _radius * _radius)
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

        protected override bool ObjectSpaceContains(Point point)
        {
            if (_solid != Solidity.Solid)
                return false;

            var slope = _height / _radius;

            var r2 = point.X * point.X + point.Y * point.Y;

            var h2 = point.Z * point.Z;

            return point.Z >= 0 && point.Z <= _height && h2 * slope <= 1 - r2;
        }

        protected override AABB ObjectSpaceGetAABB()
        {
            return new AABB
            {
                Min = new Point(-_radius, -_radius, 0),
                Max = new Point(_radius, _radius, _height)
            };
        }
    }
}
