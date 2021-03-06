﻿using System;
using System.Numerics;

namespace Raytracer.MathTypes
{
    static class Algebra
    {
        const double Tolerance = 1.0e-8;

        // Returns n=0..numComplexValues, and fills in outArray with the n values from
        // inArray that are real-valued (i.e., whose imaginary parts are within TOLERANCE of 0.)
        // outArray must be large enough to receive numComplexValues values.
        private static int FilterRealNumbers(int numComplexValues, Complex[] inArray, double [] outArray)
        {
            int numRealValues = 0;

            for (int i=0; i < numComplexValues; ++i)
            {
                if (Math.Abs(inArray[i].Imaginary) < Tolerance)
                {
                    outArray[numRealValues++] = inArray[i].Real;
                }
            }
            return numRealValues;
        }

        public static int SolveQuadraticEquation(
                        double a,
                        double b,
                        double c,
                        double[] roots)
        {
            var croots = new Complex[2];

            int numComplexRoots = SolveQuadraticEquation(
                                            new Complex(a, 0),
                                            new Complex(b, 0),
                                            new Complex(c, 0),
                                            croots);

            return FilterRealNumbers(numComplexRoots, croots, roots);
        }

        // Returns n=0..2, the number of distinct real roots found for the equation
        //
        //     ax^2 + bx + c = 0
        //
        // Stores the roots in the first n slots of the array 'roots'.
        private static int SolveQuadraticEquation(Complex a, Complex b, Complex c, Complex [] roots)
        {
            if (roots.Length != 2)
                throw new ArgumentException("roots");

            if (IsZero(a))
            {
                if (IsZero(b))
                {
                    // The equation devolves to: c = 0, where the variable x has vanished!
                    return 0;   // cannot divide by zero, so there is no solution.
                }
                
                // Simple linear equation: bx + c = 0, so x = -c/b.
                roots[0] = (-c / b);
                return 1;   // there is a single solution.
            }
            
            Complex radicand = b*b - 4.0*a*c;
            if (IsZero(radicand))
            {
                // Both roots have the same value: -b / 2a.
                roots[0] = (-b / (2.0 * a));
                return 1;
            }
            
            // There are two distinct real roots.
            Complex r = Complex.Sqrt(radicand);
            Complex d = 2.0 * a;

            roots[0] = ((-b + r) / d);
            roots[1] = ((-b - r) / d);
            return 2;
        }

        static bool IsZero(Complex x)
        {
            return (Math.Abs(x.Real) < Tolerance) && (Math.Abs(x.Imaginary) < Tolerance);
        }

        static Complex CubeRoot(Complex a, int n)
        {
            /*
                This function returns one of the 3 Complex cube roots of the Complex number 'a'.
                The value of n=0..2 selects which root is returned.
            */
            

            double rho   = Math.Pow(Complex.Abs(a), 1.0/3.0);
            double theta = ((MathLib.TWOPI * n) + a.Phase) / 3.0;
            return new Complex (rho * Math.Cos(theta), rho * Math.Sin(theta));
        }

        public static int SolveCubicEquation(
                        double a, 
                        double b, 
                        double c, 
                        double d, 
                        double [] roots)
        {
            var croots = new Complex[4];

            int numComplexRoots = SolveCubicEquation(
                                            new Complex(a, 0),
                                            new Complex(b, 0),
                                            new Complex(c, 0),
                                            new Complex(d, 0),
                                            croots);

            return FilterRealNumbers(numComplexRoots, croots, roots);
        }

        // Returns n=0..3, the number of distinct real roots found for the equation
        //
        //     ax^3 + bx^2 + cx + d = 0
        //
        // Stores the roots in the first n slots of the array 'roots'.
        private static int SolveCubicEquation(Complex a, Complex b, Complex c, Complex d, Complex[] roots)
        {
            if (roots.Length != 3)
                throw new ArgumentException("roots");

            if (IsZero(a))
            {
                return SolveQuadraticEquation(b, c, d, roots);
            }

            b /= a;
            c /= a;
            d /= a;

            Complex S = b/3.0;
            Complex D = c/3.0 - S*S;
            Complex E = S*S*S + (d - S*c)/2.0;
            Complex Froot = Complex.Sqrt(E*E + D*D*D); // changed
            Complex F = -Froot - E;

            if (IsZero(F)) 
            {
                F = Froot - E;
            }

            for (int i=0; i < 3; ++i) 
            {
                Complex G = CubeRoot(F,i);
                roots[i] = (G - D/G - S);
            }

            return 3;
        }

        public static int SolveQuarticEquation(double a, double b, double c, double d, double e, double[] roots)
        {
            var croots = new Complex[4];

            int numComplexRoots = SolveQuarticEquation(
                                            new Complex(a, 0),
                                            new Complex(b, 0),
                                            new Complex(c, 0),
                                            new Complex(d, 0),
                                            new Complex(e, 0),
                                            croots);

            return FilterRealNumbers(numComplexRoots, croots, roots);
        }

        // Returns n=0..4, the number of distinct real roots found for the equation
        //
        //     ax^4 + bx^3 + cx^2 + dx + e = 0
        //
        // Stores the roots in the first n slots of the array 'roots'.
        private static int SolveQuarticEquation(Complex a, Complex b, Complex c, Complex d, Complex e, Complex[] roots)
        {
            if (roots.Length != 4)
                throw new ArgumentException("roots");

            if (IsZero(a))
            {
                return SolveCubicEquation(b, c, d, e, roots);
            }

            // See "Summary of Ferrari's Method" in http://en.wikipedia.org/wiki/Quartic_function
        
            // Without loss of generality, we can divide through by 'a'.
            // Anywhere 'a' appears in the equations, we can assume a = 1.
            b /= a;
            c /= a;
            d /= a;
            e /= a;

            Complex b2 = b * b;
            Complex b3 = b * b2;
            Complex b4 = b2 * b2;

            Complex alpha = (-3.0/8.0)*b2 + c;
            Complex beta  = b3/8.0 - b*c/2.0 + d;
            Complex gamma = (-3.0/256.0)*b4 + b2*c/16.0 - b*d/4.0 + e;

            Complex alpha2 = alpha * alpha;
            Complex t = -b / 4.0;

            if (IsZero(beta))
            {
                Complex rad = Complex.Sqrt(alpha2 - 4.0 * gamma);
                Complex r1 = Complex.Sqrt((-alpha + rad) / 2.0);
                Complex r2 = Complex.Sqrt((-alpha - rad) / 2.0);

                roots[0] = (t + r1);
                roots[1] = (t - r1);
                roots[2] = (t + r2);
                roots[3] = (t - r2);
            }
            else
            {
                Complex alpha3 = alpha * alpha2;
                Complex P = -(alpha2/12.0 + gamma);
                Complex Q = -alpha3/108.0 + alpha*gamma/3.0 - beta*beta/8.0;
                Complex R = -Q / 2.0 + Complex.Sqrt(Q * Q / 4.0 + P * P * P / 27.0);
                Complex U = CubeRoot(R, 0);
                Complex y = (-5.0/6.0)*alpha + U;
                if (IsZero(U))
                {
                    y -= CubeRoot(Q,0);
                }
                else
                {
                    y -= P/(3.0 * U);
                }
                Complex W = Complex.Sqrt(alpha + 2.0 * y);

                Complex r1 = Complex.Sqrt(-(3.0 * alpha + 2.0 * y + 2.0 * beta / W));
                Complex r2 = Complex.Sqrt(-(3.0 * alpha + 2.0 * y - 2.0 * beta / W));

                roots[0] = (t + (W - r1) / 2.0);
                roots[1] = (t + (W + r1) / 2.0);
                roots[2] = (t + (-W - r2) / 2.0);
                roots[3] = (t + (-W + r2) / 2.0);
            }

            return 4;
        }


    }
}
