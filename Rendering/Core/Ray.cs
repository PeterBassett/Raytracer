using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.MathTypes;
using System.Security.Permissions;
using System.Runtime.Serialization;

namespace Raytracer.Rendering.Core
{
	public struct Ray
	{
        public enum CLASSIFICATION
        { MMM, MMP, MPM, MPP, PMM, PMP, PPM, PPP };

        public readonly Vector Dir;
        public readonly Point Pos;

        // Pluecker Coefficients and classificiation
        public readonly double R0;
        public readonly double R1;
        public readonly double R3;
        public readonly CLASSIFICATION classification;

		public Ray(Point pos, Vector dir) : this()
        {
            Pos = pos;
            Dir = dir;
            
            R0 = Pos.X * Dir.Y - Dir.X * Pos.Y;
            R1 = Pos.X * Dir.Z - Dir.X * Pos.Z;
            R3 = Pos.Y * Dir.Z - Dir.Y * Pos.Z;

            if (Dir.X < 0)
            {
                if (Dir.Y < 0)
                {
                    if (Dir.Z < 0)
                        classification = CLASSIFICATION.MMM;
                    else
                        classification = CLASSIFICATION.MMP;
                }
                else
                {
                    if (Dir.Z < 0)
                        classification = CLASSIFICATION.MPM;
                    else
                        classification = CLASSIFICATION.MPP;
                }
            }
            else
            {
                if (Dir.Y < 0)
                {
                    if (Dir.Z < 0)
                        classification = CLASSIFICATION.PMM;
                    else
                        classification = CLASSIFICATION.PMP;
                }
                else
                {
                    if (Dir.Z < 0)
                        classification = CLASSIFICATION.PPM;
                    else
                        classification = CLASSIFICATION.PPP;
                }
            }
        }

        public Ray Transform(Matrix matrix)
        {
            var pos = this.Pos.Transform(matrix);
            var dir = this.Dir.Transform(matrix).Normalize();
            
            return new Ray(pos, dir);
        }
	}
}
