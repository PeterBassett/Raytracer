using System;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.Primitives
{
    class Torus : ObjectSpacePrimitive
    {
        private readonly double _innerRadius;
        private readonly double _outerRadius;

        public Torus(Transform transform, double innerRadius, double outerRadius)
            : base(transform)
        {
            _innerRadius = innerRadius;
            _outerRadius = outerRadius;
        }

        protected override IntersectionInfo ObjectSpaceIntersect(Ray ray)
        {
            var r = ray;

            var R = _innerRadius;
            var S = _outerRadius;    // distance from center of tube to outside of tube

            var direction = r.Dir;
            var vantage = r.Pos;

            var uArray = new double[4];
            // Set up the coefficients of a quartic equation for the intersection
            // of the parametric equation P = vantage + u*direction and the 
            // surface of the torus.
            // There is far too much algebra to explain here.
            // See the text of the tutorial for derivation.

            double T = 4.0 * R * R;
            double G = T * (direction.X*direction.X + direction.Y*direction.Y);
            double H = 2.0 * T * (vantage.X*direction.X + vantage.Y*direction.Y);
            double I = T * (vantage.X*vantage.X + vantage.Y*vantage.Y);
            double J = direction.LengthSquared;
            double K = 2.0 * Vector.DotProduct(vantage, direction);
            double L = vantage.LengthSquared + R*R - S*S;

            int numRealRoots = Algebra.SolveQuarticEquation(
                J*J,                    // coefficient of u^4
                2.0*J*K,                // coefficient of u^3
                2.0*J*L + K*K - G,      // coefficient of u^2
                2.0*K*L - H,            // coefficient of u^1 = u
                L * L - I,              // coefficient of u^0 = constant term
                uArray                                  // receives 0..4 real solutions
            );

            // We need to keep only the real roots.
            // There can be significant roundoff error in quartic solver, 
            // so we have to tolerate more slop than usual.
            const double SURFACE_TOLERANCE = 1.0e-4;   
            int numPositiveRoots = 0;
            for (int i=0; i < numRealRoots; ++i)
            {
                // Compact the array...
                if (uArray[i] > SURFACE_TOLERANCE)
                {
                    uArray[numPositiveRoots++] = uArray[i];
                }
            }

            var distance = double.MaxValue;
            bool found = false;
            for (int j = 0; j < numPositiveRoots; j++)
            {
                if (uArray[j] > 0.001 && uArray[j] < distance)
                {
                    found = true;
                    distance = uArray[j];
                }
            }

            if (found)
            {
                var hitPoint = r.Pos + (r.Dir * distance);

                return new IntersectionInfo(HitResult.Hit, this, distance, hitPoint, hitPoint, GetNormal(hitPoint));
            }

            return new IntersectionInfo(HitResult.Miss); 
        }

        private Normal GetNormal(Point point)
        {
            var a = 1.0 - _innerRadius / Math.Sqrt(point.X * point.X + point.Y * point.Y);

            return new Normal(a * point.X,
                              a * point.Y,
                                  point.Z).Normalize();
        }

        protected override bool ObjectSpaceContains(Point point)
        {
            var t = _innerRadius - Math.Sqrt(point.X * point.X + point.Y * point.Y);
            var f = t * t + point.Z * point.Z - _outerRadius * _outerRadius;

            return f <= MathLib.Epsilon; 
        }

        protected override AABB ObjectSpaceGetAABB()
        {
            var offset = new Vector(_outerRadius + _innerRadius,    
                                    _outerRadius + _innerRadius,   
                                    _outerRadius);

            return new AABB
            {
                Min = Pos - offset,
                Max = Pos + offset
            };
        }
    }
}
