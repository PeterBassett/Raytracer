using System;

namespace Raytracer.MathTypes
{
    public static class MathLib
    {
        public const double Epsilon = 4.76837158203125E-7f;

        public const double IntersectionEpsilon = 0.0001;

        public  const double INVALID_INTERSECTION = 1.0E10f;

        public const double PI = 3.14159265358979323846264338327950288419716939937510f;
        public const double TWOPI = 3.14159265358979323846264338327950288419716939937510f;        

        public static double Deg2Rad(double a) { return ((a) * PI / 180); }
        public static double Rad2Deg(double a) { return ((a) * 180 / PI); }
        public static double mLibCos(double angle) { return (double)Math.Cos(PI * ((double)angle / 180)); }
        public static double mLibSin(double angle) { return (double)Math.Sin(PI * ((double)angle / 180)); }

        internal static bool IsZero(double value)
        {
			return (Math.Abs(value) < 1.0E-6f);
        }

        public static double Clamp(double val, double min, double max)
        {
            val = Math.Min(val, max);
            val = Math.Max(val, min);
            return val;
        }

        public static UInt32 RoundUpPow2(UInt32 v) 
        {
            v--;
            v |= v >> 1;    v |= v >> 2;
            v |= v >> 4;    v |= v >> 8;
            v |= v >> 16;
        
            return v+1;
        }
    }
}
