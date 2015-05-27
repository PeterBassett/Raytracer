using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.Lights.AreaLights
{
    class DiscLight : SphereLight
    {
        private readonly Normal _normal;

        public DiscLight(Colour colour, double power, Transform transform, int samples, double radius)
            : base(colour, power, transform, samples, radius)
        {
            _normal = Transform.ToWorldSpace(new Normal(0, -1, 0));
        }

        protected override Point GetSampledLightPoint()
        {
            var pointOnDisc = ConcentricSampleDisk();
            var point = new Point(pointOnDisc.X, pointOnDisc.Y, 0);

            var offset = point * _radius;
            offset = Transform.ToWorldSpace(offset);

            return Pos + offset;
        }

        Vector2 ConcentricSampleDisk()
        {
            return ConcentricSampleDisk(GetNextRandom(), GetNextRandom());
        }

        Vector2 ConcentricSampleDisk(double u1, double u2)
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
