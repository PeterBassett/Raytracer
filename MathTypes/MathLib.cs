using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raytracer.MathTypes
{
    using Real = System.Double;

    public static class MathLib
    {
        public const Real Epsilon = 4.76837158203125E-7f;

        public  const Real INVALID_INTERSECTION = 1.0E10f;
        
        const double mLibPi = 3.14159265358979323846264338327950288419716939937510f;
        const Real mLibEpsilon = 1.0e-05f;
        const double mLibTwopi = 2 * mLibPi;
        public static double Deg2Rad(double a) { return ((a) * mLibPi / 180); }
        public static double Rad2Deg(double a) { return ((a) * 180 / mLibPi); }
        public static double mLibCos(double angle) { return (double)Math.Cos(mLibPi * ((double)angle / 180)); }
        public static double mLibSin(double angle) { return (double)Math.Sin(mLibPi * ((double)angle / 180)); }

        public static float mLibCos(float angle) { return (float)Math.Cos(mLibPi * ((double)angle / 180)); }
        public static float mLibSin(float angle) { return (float)Math.Sin(mLibPi * ((double)angle / 180)); }
    }
}
