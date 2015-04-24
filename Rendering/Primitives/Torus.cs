using System;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.Primitives
{
    class Torus : Traceable
    {
        public double InnerRadius { get; set; }
        public double OuterRadius { get; set; }
       
        public override IntersectionInfo Intersect(Ray ray)
        {
            Ray r = CreateTransformedRay(ray);

            double R = InnerRadius;    // distance from center of hole to center of tube
            double S = OuterRadius;    // distance from center of tube to outside of tube

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

            //uArray = null;

            if (found)
            {
                var hitPoint = r.Pos + (r.Dir * distance);

                return new IntersectionInfo(HitResult.HIT, this, distance, hitPoint, hitPoint, GetNormal(hitPoint));
            }

            return new IntersectionInfo(HitResult.MISS); 
        }

        private Ray CreateTransformedRay(Ray ray)
        {
            var dir = (Vector)(ray.Pos + -this.Pos);
            ray.Dir.RotateX(-this.Ori.X, ref dir);
            dir.RotateY(-this.Ori.Y, ref dir);
            dir.RotateZ(-this.Ori.Z, ref dir);
            dir = dir.Normalize();

            return new Ray(ray.Pos + -this.Pos, dir);
        }

        public Normal GetNormal(Point point)
        {
            // Thanks to the fixed orientation of the torus in object space,
            // it always lies on the xy plane, and centered at <0,0,0>.
            // Therefore, if we drop a line in the z-direction from
            // any point P on the surface of the torus to the xy plane,
            // we find a point P' the same direction as a point Q at the center
            // of the torus tube.  Converting P' to a unit vector and multiplying
            // the result by the magnitude of Q (which is the constant R)
            // gives us the coordinates of Q.  Then the surface normal points
            // in the same direction as the difference P-Q.
            // See the tutorial text for more details.
            double R = InnerRadius;    // distance from center of hole to center of tube
            double S = OuterRadius;    // distance from center of tube to outside of tube

            double a = 1.0 - (R / Math.Sqrt(point.X * point.X + point.Y * point.Y));

            return new Normal(a * point.X, 
                              a * point.Y, 
                                  point.Z).Normalize();
        }
        
        public override bool Intersect(AABB aabb)
        {
            return this.GetAABB().Intersect(aabb);
        }

        public override AABB GetAABB()
        {
            return new AABB()
            {
                Min = this.Pos - OuterRadius,
                Max = this.Pos + OuterRadius
            };
        }

        public override bool Contains(Point point)
        {
            return false;
        }
    }
}
