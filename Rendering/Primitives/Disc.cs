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

        public override IntersectionInfo ObjectSpace_Intersect(Ray ray)
        {
            var missed = new IntersectionInfo(HitResult.MISS);

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
                Result = HitResult.HIT,
                T = (u*ray.Dir).Length,
                NormalAtHitPoint = new Normal(0.0, 0.0, (ray.Pos.Z >= 0.0) ? 1.0 : -1.0),
                Primitive = this
            };
        }

        public override bool ObjectSpace_Contains(Point point)
        {
            var length = new Vector2(point.X, point.Y).LengthSquared;

            if (length > _outerRadiusSquared)
                return false;

            return point.Z >= -MathLib.Epsilon && point.Z <= MathLib.Epsilon;
        }

        public override AABB ObjectSpace_GetAABB()
        {
            return new AABB
            {
                Min = new Point(-OuterRadius, -OuterRadius, -MathLib.Epsilon),
                Max = new Point(OuterRadius, OuterRadius, MathLib.Epsilon)
            };
        }
    }
}
