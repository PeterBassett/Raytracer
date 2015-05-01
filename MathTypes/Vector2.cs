using System;
using System.Runtime.InteropServices;

namespace Raytracer.MathTypes
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector2
	{		
		public double X;
		public double Y;

        public static readonly Vector2 Zero = new Vector2(0.0f, 0.0f);
		
        public Vector2(double x, double y)
		{
			X = x;
			Y = y;
		}

		public void Normalize()
		{
			double length = Length;
			if (length == 0)
				throw new DivideByZeroException("Trying to normalize a vector with length of zero.");

			X /= length;
			Y /= length;

		}

		public double Length
		{
		    get { return Math.Sqrt(LengthSquared); }
		}

		public double LengthSquared
		{
		    get { return (X*X + Y*Y); }
		}

		public override int GetHashCode()
		{
			return X.GetHashCode() ^ Y.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj is Vector2)
			{
				Vector2 v = (Vector2)obj;
				return (X == v.X) && (Y == v.Y);
			}
			return false;
		}

		public override string ToString()
		{
			return string.Format("({0}, {1})", X, Y);
		}

		public static bool operator==(Vector2 u, Vector2 v)
		{
			return (u.X == v.X) && (u.Y == v.Y);
		}

		public static bool operator!=(Vector2 u, Vector2 v)
		{
			return !(u == v);
		}
        
		public static Vector2 operator+(Vector2 u, Vector2 v)
		{
            return new Vector2(u.X + v.X, u.Y + v.Y);
		}

		public static Vector2 operator*(Vector2 v, double s)
		{
            return new Vector2(v.X * s, v.Y * s);
		}	
	}

}
