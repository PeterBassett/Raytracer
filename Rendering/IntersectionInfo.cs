using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.MathTypes;

namespace Raytracer.Rendering
{
    using Vector = Vector3;
    using Real = System.Double;
    using Raytracer.Rendering.Primitives;

    struct IntersectionInfo
    {

        private HitResult _Result;
        private Real _T;
        private Vector _HitPoint;
        private Vector _ObjectLocalHitPoint ;
        private Vector _NormalAtHitPoint;
        private Traceable _Primitive;

        public IntersectionInfo(HitResult miss)
            : this(miss, null, MathLib.INVALID_INTERSECTION, Vector.Zero, Vector.Zero, Vector.Zero)
        {
            _Result = miss;
            _Primitive = null;
            _T = MathLib.INVALID_INTERSECTION;
            _HitPoint = Vector.Zero;
            _ObjectLocalHitPoint = Vector.Zero;
            _NormalAtHitPoint = Vector.Zero;

            if (HitResult.MISS != miss)
                throw new ArgumentNullException("If the result was not a miss you should provide all parameters");
        }

        public IntersectionInfo(HitResult result, Traceable primitive, Real distance, Vector hitPoint, Vector localHitpoint, Vector normalAthitPoint) : this()
        {
            _Result = result;
            _Primitive = primitive;
            _T = distance;
            _HitPoint = hitPoint;
            _ObjectLocalHitPoint = localHitpoint;
            _NormalAtHitPoint = normalAthitPoint;
        }

        public HitResult Result { get { return _Result; } set { _Result = value; } }
        public Real T { get{ return _T; } set{_T = value;} }
        public Vector HitPoint { get{ return _HitPoint; } set{_HitPoint = value;} }
        public Vector ObjectLocalHitPoint { get{ return _ObjectLocalHitPoint; } set{_ObjectLocalHitPoint = value;} }
        public Vector NormalAtHitPoint { get{ return _NormalAtHitPoint; } set{_NormalAtHitPoint = value;} }
        public Traceable Primitive { get{ return _Primitive; } set{_Primitive = value;} }

        /*
        public IntersectionInfo()
        {
            T = 0.0f;
        }*/
    }
}
