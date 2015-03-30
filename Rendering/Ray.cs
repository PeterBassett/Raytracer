using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using Raytracer.MathTypes;
using System.Security.Permissions;
using System.Runtime.Serialization;

namespace Raytracer.Rendering
{
    using Vector = Vector3;
    using Real = System.Double;
	public struct Ray
	{
        public enum CLASSIFICATION
        { MMM, MMP, MPM, MPP, PMM, PMP, PPM, PPP };

        Vector m_dir;
        Vector n_invDir;
        Vector m_pos;
        public Real R0;			// Pluecker coefficient R0
        public Real R1;			// Pluecker coefficient R1
        public Real R3;			// Pluecker coefficient R3
        public CLASSIFICATION classification;

		public Ray(Vector pos, Vector dir)
        {
            m_pos = pos;
            m_dir = dir;
            n_invDir = new Vector(1.0f / m_dir.X, 1.0f / m_dir.Y, 1.0f / m_dir.Z);

            R0 = pos.X * dir.Y - dir.X * pos.Y;
            R1 = pos.X * dir.Z - dir.X * pos.Z;
            R3 = pos.Y * dir.Z - dir.Y * pos.Z;

            if (dir.X < 0)
            {
                if (dir.Y < 0)
                {
                    if (dir.Z < 0)
                        classification = CLASSIFICATION.MMM;
                    else
                        classification = CLASSIFICATION.MMP;
                }
                else
                {
                    if (dir.Z < 0)
                        classification = CLASSIFICATION.MPM;
                    else
                        classification = CLASSIFICATION.MPP;
                }
            }
            else
            {
                if (dir.Y < 0)
                {
                    if (dir.Z < 0)
                        classification = CLASSIFICATION.PMM;
                    else
                        classification = CLASSIFICATION.PMP;
                }
                else
                {
                    if (dir.Z < 0)
                        classification = CLASSIFICATION.PPM;
                    else
                        classification = CLASSIFICATION.PPP;
                }
            }

		}

        public Real x { get { return Pos.X; } }
        public Real y { get { return Pos.Y; } }
        public Real z { get { return Pos.Z; } }

        public Real i { get { return Dir.X; } }
        public Real j { get { return Dir.Y; } }
        public Real k { get { return Dir.Z; } }

        public Real ii { get { return n_invDir.X; } }
        public Real ij { get { return n_invDir.Y; } }
        public Real ik { get { return n_invDir.Z; } }

        private void BuildInverseDir()
        {
            n_invDir = new Vector(1.0f / m_dir.X, 1.0f / m_dir.Y, 1.0f / m_dir.Z);
        }

        /// <summary>
        /// Gets the ray's inverse.
        /// </summary>
        public Vector InverseDir
        {
            get
            {
                return n_invDir;
            }
        }

		/// <summary>
		/// Gets or sets the ray's origin.
		/// </summary>
        public Vector Pos 
        {
            get
            {
                return m_pos;
            }
            set
            {
                m_pos = value;
            }
        }
		/// <summary>
		/// Gets or sets the ray's direction vector.
		/// </summary>
        public Vector Dir
        {
            get
            {
                return m_dir;
            }
            set
            {
                m_dir = value;

                BuildInverseDir();
            }
        }
	}
}
