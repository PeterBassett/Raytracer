using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using Raytracer.MathTypes;
using System.Security.Permissions;
using System.Runtime.Serialization;

namespace Raytracer.Rendering.Core
{
    
    
	public struct Ray
	{
        public enum CLASSIFICATION
        { MMM, MMP, MPM, MPP, PMM, PMP, PPM, PPP };

        Vector3 m_dir;
        Vector3 n_invDir;
        Vector3 m_pos;
        public double R0;			// Pluecker coefficient R0
        public double R1;			// Pluecker coefficient R1
        public double R3;			// Pluecker coefficient R3
        public CLASSIFICATION classification;

		public Ray(Vector3 pos, Vector3 dir)
        {
            m_pos = pos;
            m_dir = dir;
            n_invDir = new Vector3(1.0f / m_dir.X, 1.0f / m_dir.Y, 1.0f / m_dir.Z);

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

        public double x { get { return Pos.X; } }
        public double y { get { return Pos.Y; } }
        public double z { get { return Pos.Z; } }

        public double i { get { return Dir.X; } }
        public double j { get { return Dir.Y; } }
        public double k { get { return Dir.Z; } }

        public double ii { get { return n_invDir.X; } }
        public double ij { get { return n_invDir.Y; } }
        public double ik { get { return n_invDir.Z; } }

        private void BuildInverseDir()
        {
            n_invDir = new Vector3(1.0f / m_dir.X, 1.0f / m_dir.Y, 1.0f / m_dir.Z);
        }

        /// <summary>
        /// Gets the ray's inverse.
        /// </summary>
        public Vector3 InverseDir
        {
            get
            {
                return n_invDir;
            }
        }

		/// <summary>
		/// Gets or sets the ray's origin.
		/// </summary>
        public Vector3 Pos 
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
        public Vector3 Dir
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
