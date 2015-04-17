
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raytracer.MathTypes
{
    class Polynomial
    {
        readonly double[] coef = new double[16];

        private const double COEFF_LIMIT = 1.0e-16;
        private const double EPSILON = 1.0e-10;
        private const double TWO_PI = 6.283185207179586476925286766560;
        private const double TWO_PI_3 = 2.0943951023931954923084;
        private const double TWO_PI_43 = 4.1887902047863909846168;

        public Polynomial()
        {
            coef[0] = 0.0;
        }

        public Polynomial(double a, double b, double c, double d, double e)
            : this()
        {
            coef[0] = e;
            coef[1] = d;
            coef[2] = c;
            coef[3] = b;
            coef[4] = a;
        }

        Polynomial(double a, double b, double c, double d)
            : this()
        {
            coef[0] = d;
            coef[1] = c;
            coef[2] = b;
            coef[3] = a;
        }

        public double this[int index]
		{
			get	
			{
				return coef[index];
			}
			set 
			{
				coef[index] = value;
			}

		}

        public int SolveQuartic(double[] results)
        {
            double[] roots = new double[3];

            double c12, z, p, q, q1, q2, r, d1, d2;
            double c0, c1, c2, c3, c4;
            int i;

            // See if the high order term has vanished 
            c0 = coef[4];
            if (Math.Abs(c0) < COEFF_LIMIT)
            {
                return SolveCubic(results);
            }
            // See if the constant term has vanished 
            if (Math.Abs(coef[0]) < COEFF_LIMIT)
            {
                var y = new Polynomial(coef[4], coef[3], coef[2], coef[1]);
                return y.SolveCubic(results);
            }
            // Make sure the quartic has a leading coefficient of 1.0 
            if (c0 != 1.0)
            {
                c1 = coef[3] / c0;
                c2 = coef[2] / c0;
                c3 = coef[1] / c0;
                c4 = coef[0] / c0;
            }
            else
            {
                c1 = coef[3];
                c2 = coef[2];
                c3 = coef[1];
                c4 = coef[0];
            }

            // Compute the cubic resolvant 
            c12 = c1 * c1;
            p = -0.375 * c12 + c2;
            q = 0.125 * c12 * c1 - 0.5 * c1 * c2 + c3;
            r = -0.01171875 * c12 * c12 + 0.0625 * c12 * c2 - 0.25 * c1 * c3 + c4;

            Polynomial cubic = new Polynomial();
            cubic[3] = 1.0;
            cubic[2] = -0.5 * p;
            cubic[1] = -r;
            cubic[0] = 0.5 * r * p - 0.125 * q * q;

            i = cubic.SolveCubic(roots);
            if (i > 0)
                z = roots[0];
            else
                return 0;

            d1 = 2.0 * z - p;

            if (d1 < 0.0)
            {
                if (d1 > -EPSILON)
                    d1 = 0.0;
                else
                    return 0;
            }
            if (d1 < EPSILON)
            {
                d2 = z * z - r;
                if (d2 < 0.0)
                    return 0;
                d2 = Math.Sqrt(d2);
            }
            else
            {
                d1 = Math.Sqrt(d1);
                d2 = 0.5 * q / d1;
            }

            // Set up useful values for the quadratic factors 
            q1 = d1 * d1;
            q2 = -0.25 * c1;
            i = 0;

            // Solve the first quadratic
            p = q1 - 4.0 * (z - d2);
            if (p == 0)
                results[i++] = -0.5 * d1 - q2;
            else if (p > 0)
            {
                p = Math.Sqrt(p);
                results[i++] = -0.5 * (d1 + p) + q2;
                results[i++] = -0.5 * (d1 - p) + q2;
            }
            // Solve the second quadratic 
            p = q1 - 4.0 * (z + d2);
            if (p == 0)
                results[i++] = 0.5 * d1 - q2;
            else if (p > 0)
            {
                p = Math.Sqrt(p);
                results[i++] = 0.5 * (d1 + p) + q2;
                results[i++] = 0.5 * (d1 - p) + q2;
            }
            return i;
        }

        int SolveCubic(double[] y)
        {
            double Q, R, Q3, R2, sQ, d, an, theta;
            double A2, a1, a2, a3;

            double a0 = coef[3];

            if (Math.Abs(a0) < EPSILON)
            {
                return SolveQuadratic(y);
            }
            else
            {
                if (a0 != 1.0)
                {
                    a1 = coef[2] / a0;
                    a2 = coef[1] / a0;
                    a3 = coef[0] / a0;
                }
                else
                {
                    a1 = coef[2];
                    a2 = coef[1];
                    a3 = coef[0];
                }
            }

            A2 = a1 * a1;

            Q = (A2 - 3.0 * a2) / 9.0;

            R = (a1 * (A2 - 4.5 * a2) + 13.5 * a3) / 27.0;

            Q3 = Q * Q * Q;

            R2 = R * R;

            d = Q3 - R2;

            an = a1 / 3.0;

            if (d >= 0.0)
            {
                /* Three real roots. */

                d = R / Math.Sqrt(Q3);

                theta = Math.Acos(d) / 3.0;

                sQ = -2.0 * Math.Sqrt(Q);

                y[0] = sQ * Math.Cos(theta) - an;
                y[1] = sQ * Math.Cos(theta + TWO_PI_3) - an;
                y[2] = sQ * Math.Cos(theta + TWO_PI_43) - an;

                return 3;
            }
            else
            {
                sQ = Math.Pow(Math.Sqrt(R2 - Q3) + Math.Abs(R), 1.0 / 3.0);

                if (R < 0)
                {
                    y[0] = (sQ + Q / sQ) - an;
                }
                else
                {
                    y[0] = -(sQ + Q / sQ) - an;
                }

                return 1;
            }
        }

        int SolveQuadratic(double[] y)
        {
            double d, t, a, b, c;
            a = coef[2];
            b = -coef[1];
            c = coef[0];
            if (a == 0.0)
            {
                if (b == 0.0)
                    return 0;
                y[0] = y[1] = c / b;
                return 1;
            }
            d = b * b - 4.0 * a * c;
            if (d < 0.0)
                return 0;
            else if (Math.Abs(d) < COEFF_LIMIT)
            {
                y[0] = y[1] = 0.5 * b / a;
                return 1;
            }
            d = Math.Sqrt(d);
            t = 0.5 / a;
            if (t > 0.0)
            {
                y[0] = (b - d) * t;
                y[1] = (b + d) * t;
            }
            else
            {
                y[0] = (b + d) * t;
                y[1] = (b - d) * t;
            }
            return 2;
        }
    }
}
