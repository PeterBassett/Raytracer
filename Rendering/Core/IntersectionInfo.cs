using System;
using Raytracer.MathTypes;
using Raytracer.Rendering.Materials;

namespace Raytracer.Rendering.Core
{
    struct IntersectionInfo
    {

        private HitResult _result;
        private double _t;
        private Point _hitPoint;
        private Point _objectLocalHitPoint ;
        private Normal _normalAtHitPoint;
        private Traceable _primitive;
        private Material _material;

        public IntersectionInfo(HitResult miss)
            : this(miss, null, MathLib.INVALID_INTERSECTION, Point.Zero, Point.Zero, Normal.Invalid)
        {
            _result = miss;
            _primitive = null;
            _t = MathLib.INVALID_INTERSECTION;
            _hitPoint = Point.Zero;
            _objectLocalHitPoint = Point.Zero;
            _normalAtHitPoint = Normal.Invalid;

            if (HitResult.Miss != miss)
                throw new ArgumentNullException("If the result was not a miss you should provide all parameters");
        }

        public IntersectionInfo(HitResult result, Traceable primitive, double distance, Point hitPoint, Point localHitpoint, Normal normalAthitPoint) : this()
        {
            _result = result;
            _primitive = primitive;
            _t = distance;
            _hitPoint = hitPoint;
            _objectLocalHitPoint = localHitpoint;
            _normalAtHitPoint = normalAthitPoint;
            _material = _primitive != null ? _primitive.Material : null;
        }

        public IntersectionInfo(HitResult result, Traceable primitive, double distance, Point hitPoint, Point localHitpoint, Normal normalAthitPoint, Material material)
            : this()
        {
            _result = result;
            _primitive = primitive;
            _t = distance;
            _hitPoint = hitPoint;
            _objectLocalHitPoint = localHitpoint;
            _normalAtHitPoint = normalAthitPoint;
            _material = material;
        }

        public HitResult Result { get { return _result; } set { _result = value; } }
        public double T { get{ return _t; } set{_t = value;} }
        public Point HitPoint { get { return _hitPoint; } set { _hitPoint = value; } }
        public Point ObjectLocalHitPoint { get { return _objectLocalHitPoint; } set { _objectLocalHitPoint = value; } }
        public Normal NormalAtHitPoint { get{ return _normalAtHitPoint; } set{_normalAtHitPoint = value;} }
        public Traceable Primitive { get{ return _primitive; } set{_primitive = value;} }
        public Material Material { get { return _material; } set { _material = value; } }
    }
}
