using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Raytracer.MathTypes;

namespace Raytracer.Rendering.Samplers
{
    static class Sampler
    {
        private static ThreadLocal<Random> _rnd;

        static Sampler()
        {
            _rnd = new ThreadLocal<Random>(() => new Random());
        }

        public static double GetNextRandom()
        {
            return _rnd.Value.NextDouble();
        }

        static float U_m1_p1(){
            return (float)(GetNextRandom());// *(1.0f / 2147483648.0f) - 1.0f;
        }

        public static Vector RandomVectorInSphere()
        {
            float x0, x1, x2, x3, d2;
            do
            {
                x0 = U_m1_p1();
                x1 = U_m1_p1();
                x2 = U_m1_p1();
                x3 = U_m1_p1();
                d2 = x0 * x0 + x1 * x1 + x2 * x2 + x3 * x3;
            } while (d2 > 1.0f);
            float scale = 1.0f / d2;
            return new Vector(2 * (x1 * x3 + x0 * x2) * scale,
                              2 * (x2 * x3 + x0 * x1) * scale,
                              (x0 * x0 + x3 * x3 - x1 * x1 - x2 * x2) * scale);
        }

        public static Vector RandomUnitVectorInHemisphereOf(Normal v)
        {
            Vector result = RandomVectorInSphere().Normalize();
            if (Vector.DotProduct(result, v) < 0)
            {
                result.X = -result.X;
                result.Y = -result.Y;
                result.Z = -result.Z;
            }
            return result;
        }

        public static Vector UniformSampleHemisphere(Vector2 sample)
        {
            return UniformSampleHemisphere(sample.X, sample.Y);
        }

        public static Vector UniformSampleHemisphere(double u1, double u2)
        {
            var r = Math.Sqrt(1.0f - u1 * u1);
            var phi = 2 * Math.PI * u2;

            return new Vector(Math.Cos(phi) * r, Math.Sin(phi) * r, u1);
        }

        public static Vector2 ConcentricSampleDisk(Vector2 sample)
        {
            return ConcentricSampleDisk(sample.X, sample.Y);
        }

        public static Vector2 ConcentricSampleDisk(double u1, double u2)
        {
            double dx, dy;

            double r, theta;
            // Map uniform random numbers to $[-1,1]^2$
            var sx = 2 * u1 - 1;
            var sy = 2 * u2 - 1;

            // Map square to $(r,\theta)$

            // Handle degeneracy at the origin
            if (sx == 0.0 && sy == 0.0)
            {
                dx = 0.0;
                dy = 0.0;
                return new Vector2(dx, dy);
            }
            if (sx >= -sy)
            {
                if (sx > sy)
                {
                    // Handle first region of disk
                    r = sx;
                    if (sy > 0.0) theta = sy / r;
                    else theta = 8.0f + sy / r;
                }
                else
                {
                    // Handle second region of disk
                    r = sy;
                    theta = 2.0f - sx / r;
                }
            }
            else
            {
                if (sx <= sy)
                {
                    // Handle third region of disk
                    r = -sx;
                    theta = 4.0f - sy / r;
                }
                else
                {
                    // Handle fourth region of disk
                    r = -sy;
                    theta = 6.0f + sx / r;
                }
            }

            theta *= Math.PI / 4.0;

            dx = r * Math.Cos(theta);
            dy = r * Math.Sin(theta);

            return new Vector2(dx, dy);
        }
    }
}
