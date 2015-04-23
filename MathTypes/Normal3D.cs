using System;
using System.Runtime.InteropServices;

namespace Raytracer.MathTypes
{
	[StructLayout(LayoutKind.Sequential)]
    public struct Normal3
	{
        public double X;
        public double Y;
        public double Z;
				
		public Normal3(double x, double y, double z)
		{
			X = x;
			Y = y;
			Z = z;
		}

        private static Normal3 _Invalid = new Normal3();
        public static Normal3 Invalid { get { return _Invalid; } }
		
        public static double DotProduct(Normal3 u, Normal3 v)
		{
			return (u.X * v.X) + (u.Y * v.Y) + (u.Z * v.Z);
		}

        public static double AbsDotProduct(Normal3 u, Normal3 v)
		{
			return Math.Abs(DotProduct(u, v));
		}
		
        public static Normal3 CrossProduct(Normal3 u, Normal3 v)
		{
			return new Normal3( 
				u.Y*v.Z - u.Z*v.Y, 
				u.Z*v.X - u.X*v.Z, 
				u.X*v.Y - u.Y*v.X );
		}
		
        public static Normal3 Negate(Normal3 v)
		{
			return new Normal3(-v.X, -v.Y, -v.Z);
		}
		
        public static bool ApproxEqual(Normal3 v, Normal3 u)
		{
			return ApproxEqual(v,u, MathLib.Epsilon);
		}

		public static bool ApproxEqual(Normal3 v, Normal3 u, double tolerance)
		{
			return
				(
				(System.Math.Abs(v.X - u.X) <= tolerance) &&
				(System.Math.Abs(v.Y - u.Y) <= tolerance) &&
				(System.Math.Abs(v.Z - u.Z) <= tolerance)
				);
		}

        public void RotateX(double amnt, ref Normal3 dest)
        {
            double s = MathLib.mLibSin(amnt);
            double c = MathLib.mLibCos(amnt);
            double y = this.Y;
            double z = this.Z;

            dest.X = this.X;
            dest.Y = (y * c) - (z * s);
            dest.Z = (y * s) + (z * c);
        }

        public void RotateY(double amnt, ref Normal3 dest)
        {
            double s = MathLib.mLibSin(amnt);
            double c = MathLib.mLibCos(amnt);
            double x = this.X;
            double z = this.Z;

            dest.X = (x * c) + (z * s);
            dest.Y = this.Y;
            dest.Z = (z * c) - (x * s);
        }

        public void RotateZ(double amnt, ref Normal3 dest)
        {
            double s = MathLib.mLibSin(amnt);
            double c = MathLib.mLibCos(amnt);
            double x = this.X;
            double y = this.Y;

            dest.X = (x * c) - (y * s);
            dest.Y = (y * c) + (x * s);
            dest.Z = this.Z;
        }

		public Normal3 Normalize()
		{
			double length = this.Length;

			if (length == 0)
				throw new DivideByZeroException("Trying to normalize a vector with length of zero.");

            return this / length;
		}

		public double Length
		{
            get
            {
			    return System.Math.Sqrt(LengthSquared);
            }
		}

		public double LengthSquared
		{
            get
            {
			    return (X*X + Y*Y + Z*Z);
            }
		}

        public override int GetHashCode()
		{
			return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
		}

        public override bool Equals(object obj)
		{
			if (obj is Normal3)
			{
				Normal3 v = (Normal3)obj;
				return (X == v.X) && (Y == v.Y) && (Z == v.Z);
			}
			return false;
		}

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})", X, Y, Z);
        }
		
		public static bool operator==(Normal3 u, Normal3 v)
		{
			if (Object.Equals(u, null))
			{
				return Object.Equals(v, null);
			}

			if (Object.Equals(v, null))
			{
				return Object.Equals(u, null);
			}

			return (u.X == v.X) && (u.Y == v.Y) && (u.Z == v.Z);
		}
        
        public static bool operator!=(Normal3 u, Normal3 v)
		{
			return !(u == v);
		}
		
		public static Normal3 operator-(Normal3 v)
		{
			return new Normal3(-v.X, -v.Y, -v.Z);
		}

		public static Normal3 operator+(Normal3 v, Normal3 w)
		{
            return new Normal3(v.X + w.X, v.Y + w.Y, v.Z + w.Z);
		}

		public static Normal3 operator+(Normal3 v, double s)
		{
			return new Normal3(v.X + s, v.Y + s, v.Z + s);
		}
		
        public static Normal3 operator-(Normal3 v, Normal3 w)
		{
            return new Normal3(v.X - w.X, v.Y - w.Y, v.Z - w.Z);
		}

        public static Normal3 operator-(Normal3 v, double s)
		{
			return new Normal3(v.X - s, v.Y - s, v.Z - s);
		}

        public static Normal3 operator-(double s, Normal3 v)
		{
			return new Normal3(v.X - s, v.Y - s, v.Z - s);
		}

        public static Normal3 operator -(Normal3 v, Vector3 w)
        {
            return new Normal3(v.X - w.X, v.Y - w.Y, v.Z - w.Z);
        }

        public static Normal3 operator -(Normal3 v, Point3 w)
        {
            return new Normal3(v.X - w.X, v.Y - w.Y, v.Z - w.Z);
        }

        public static Normal3 operator -(Vector3 v, Normal3 w)
        {
            return new Normal3(v.X - w.X, v.Y - w.Y, v.Z - w.Z);
        }

        public static Normal3 operator*(Normal3 v, double s)
		{
		    return new Normal3(v.X * s, v.Y * s, v.Z * s);
		}

        public static Normal3 operator*(double s, Normal3 v)
		{
		    return new Normal3(v.X * s, v.Y * s, v.Z * s);
		}

        public static Normal3 operator/(Normal3 v, double s)
		{
			return new Normal3(v.X / s, v.Y / s, v.Z / s);
		}
        
        public static Normal3 operator/(double s, Normal3 v)
		{
			return new Normal3(s / v.X, s / v.Y, s / v.Z);
		}
        
        public double this[int index]
		{
			get	
			{
				switch( index ) 
				{
					case 0:
						return X;
					case 1:
						return Y;
					case 2:
						return Z;
					default:
						throw new IndexOutOfRangeException();
				}
			}
			set 
			{
				switch( index ) 
				{
					case 0:
						X = value;
						break;
					case 1:
						Y = value;
						break;
					case 2:
						Z = value;
						break;
					default:
						throw new IndexOutOfRangeException();
				}
			}

		}

        public static explicit operator Normal3(Point3 point)
        {
            return new Normal3(point.X, point.Y, point.Z);
        }

        public static explicit operator Normal3(Vector3 vector)
		{
			return new Normal3(vector.X, vector.Y, vector.Z);
		}

		public static explicit operator Vector3(Normal3 normal)
		{
			return new Vector3(normal.X, normal.Y, normal.Z);
		}     
    }
}
