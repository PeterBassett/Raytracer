using System;
using Raytracer.MathTypes;
using Raytracer.Rendering.Primitives;

namespace Raytracer.Rendering.Core
{
    struct IntersectionInfo
    {

        private HitResult _result;
        private double _t;
        private Vector3 _hitPoint;
        private Vector3 _objectLocalHitPoint ;
        private Vector3 _normalAtHitPoint;
        private Traceable _primitive;

        public IntersectionInfo(HitResult miss)
            : this(miss, null, MathLib.INVALID_INTERSECTION, Vector3.Zero, Vector3.Zero, Vector3.Zero)
        {
            _result = miss;
            _primitive = null;
            _t = MathLib.INVALID_INTERSECTION;
            _hitPoint = Vector3.Zero;
            _objectLocalHitPoint = Vector3.Zero;
            _normalAtHitPoint = Vector3.Zero;

            if (HitResult.MISS != miss)
                throw new ArgumentNullException("If the result was not a miss you should provide all parameters");
        }

        public IntersectionInfo(HitResult result, Traceable primitive, double distance, Vector3 hitPoint, Vector3 localHitpoint, Vector3 normalAthitPoint) : this()
        {
            _result = result;
            _primitive = primitive;
            _t = distance;
            _hitPoint = hitPoint;
            _objectLocalHitPoint = localHitpoint;
            _normalAtHitPoint = normalAthitPoint;
        }

        public HitResult Result { get { return _result; } set { _result = value; } }
        public double T { get{ return _t; } set{_t = value;} }
        public Vector3 HitPoint { get{ return _hitPoint; } set{_hitPoint = value;} }
        public Vector3 ObjectLocalHitPoint { get{ return _objectLocalHitPoint; } set{_objectLocalHitPoint = value;} }
        public Vector3 NormalAtHitPoint { get{ return _normalAtHitPoint; } set{_normalAtHitPoint = value;} }
        public Traceable Primitive { get{ return _primitive; } set{_primitive = value;} }
    }
}
