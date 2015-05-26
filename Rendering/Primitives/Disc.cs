using System;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.Primitives
{
    class Disc : ObjectSpacePrimitive
    {
        public readonly double OuterRadius;
        public readonly double InnerRadius;

        private readonly double _outerRadiusSquared;
        private readonly double _innerRadiusSquared;

        public Disc(double outerRadius, double innerRadius, Transform transform)
            : base(transform)
        {
            OuterRadius = outerRadius;
            InnerRadius = innerRadius;

            _outerRadiusSquared = OuterRadius * OuterRadius;
            _innerRadiusSquared = InnerRadius * InnerRadius;
        }

        protected override IntersectionInfo ObjectSpaceIntersect(Ray ray)
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

            if (radial > _outerRadiusSquared || radial < _innerRadiusSquared)
                return missed;

            return new IntersectionInfo
            {
                HitPoint = new Point(x, y, 0.0),
                ObjectLocalHitPoint = new Point(x, y, 0.0),
                Result = HitResult.Hit,
                T = (u*ray.Dir).Length,
                NormalAtHitPoint = new Normal(0.0, 0.0, (ray.Pos.Z >= 0.0) ? 1.0 : -1.0),
                Primitive = this
            };
        }

        protected override bool ObjectSpaceContains(Point point)
        {
            var length = new Vector2(point.X, point.Y).LengthSquared;

            if (length > _outerRadiusSquared)
                return false;

            return point.Z >= -MathLib.Epsilon && point.Z <= MathLib.Epsilon;
        }

        protected override AABB ObjectSpaceGetAABB()
        {
            return new AABB
            {
                Min = new Point(-OuterRadius, -OuterRadius, -MathLib.Epsilon),
                Max = new Point(OuterRadius, OuterRadius, MathLib.Epsilon)
            };
        }
    }
}
