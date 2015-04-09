using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.Primitives
{
    using Vector = Vector3;
    using Real = System.Double;
    using System.Numerics;

    class Torus : Traceable
    {
        public Real InnerRadius { get; set; }
        public Real OuterRadius { get; set; }
       
        public override IntersectionInfo Intersect(Ray ray)
        {
            Ray r = CreateTransformedRay(ray);

            double R = InnerRadius;    // distance from center of hole to center of tube
            double S= OuterRadius;    // distance from center of tube to outside of tube

            Vector direction = r.Dir;
            Vector vantage = r.Pos;

            Real[] uArray = new Real[4];
            // Set up the coefficients of a quartic equation for the intersection
            // of the parametric equation P = vantage + u*direction and the 
            // surface of the torus.
            // There is far too much algebra to explain here.
            // See the text of the tutorial for derivation.

            double T = 4.0 * R * R;
            double G = T * (direction.X*direction.X + direction.Y*direction.Y);
            double H = 2.0 * T * (vantage.X*direction.X + vantage.Y*direction.Y);
            double I = T * (vantage.X*vantage.X + vantage.Y*vantage.Y);
            double J = direction.GetLengthSquared();
            double K = 2.0 * Vector.DotProduct(vantage, direction);
            double L = vantage.GetLengthSquared() + R*R - S*S;

            int numRealRoots = Algebra.SolveQuarticEquation(
                new Complex(J*J, 0),                    // coefficient of u^4
                new Complex(2.0*J*K, 0),                // coefficient of u^3
                new Complex(2.0*J*L + K*K - G, 0),      // coefficient of u^2
                new Complex(2.0*K*L - H, 0),            // coefficient of u^1 = u
                new Complex(L * L - I, 0),              // coefficient of u^0 = constant term
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

            for (int j = 0; j < numPositiveRoots; j++)
            {
                if (uArray[j] > 0.001 && uArray[j] < distance)
                    distance = uArray[j];
            }

            uArray = null;

            if (distance != double.MaxValue)
            {
                var hitPoint = r.Pos + (r.Dir * distance);

                return new IntersectionInfo(HitResult.HIT, this, distance, hitPoint, hitPoint, GetNormal(hitPoint));
            }

            return new IntersectionInfo(HitResult.MISS); 
        }

        private Ray CreateTransformedRay(Ray ray)
        {
            Vector dir = ray.Pos + -this.Pos;
            ray.Dir.RotateX(-this.Ori.X, ref dir);
            dir.RotateY(-this.Ori.Y, ref dir);
            dir.RotateZ(-this.Ori.Z, ref dir);
            //dir.Normalize();

            return new Ray(ray.Pos + -this.Pos, dir);
        }

        Vector SurfaceNormal(Vector point)
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

            double a = 1.0 - (R / Math.Sqrt(point.X*point.X + point.Y*point.Y));
            var n = new Vector(a*point.X, a*point.Y, point.Z);
            n.Normalize();

            return n;
        }

        public override Vector GetNormal(Vector p)
        {
            return SurfaceNormal(p);
            /*double nx, ny, nz;
            nx = 4 * p.X * (Math.Pow(p.X, 2) + Math.Pow(p.Y, 2) + Math.Pow(p.Y, 2) + Math.Pow(OuterRadius, 2) - Math.Pow(InnerRadius, 2)) - 8 * Math.Pow(OuterRadius, 2) * p.X;
            ny = 4 * p.Y * (Math.Pow(p.X, 2) + Math.Pow(p.Y, 2) + Math.Pow(p.Y, 2) + Math.Pow(OuterRadius, 2) - Math.Pow(InnerRadius, 2)) - 8 * Math.Pow(OuterRadius, 2) * p.Y;
            nz = 4 * p.Z * (Math.Pow(p.X, 2) + Math.Pow(p.Y, 2) + Math.Pow(p.Y, 2) + Math.Pow(OuterRadius, 2) - Math.Pow(InnerRadius, 2)) - 8 * Math.Pow(OuterRadius, 2) * p.Z;
            Vector res = new Vector(nx, ny, nz);
            res.Normalize();
            return res;*/

            //return new Vector(0, 1, 0);
        }

        public override bool Intersect(AABB aabb)
        {
            return this.GetAABB().Intersect(aabb);
        }

        public override AABB GetAABB()
        {
            return new AABB()
            {
                Min = new Vector(this.Pos.X - OuterRadius, this.Pos.Y - OuterRadius, this.Pos.Z - OuterRadius),
                Max = new Vector(this.Pos.X + OuterRadius, this.Pos.Y + OuterRadius, this.Pos.Z + OuterRadius)
            };
        }
    }
}
