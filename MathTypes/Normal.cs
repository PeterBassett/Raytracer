using System;
using System.Runtime.InteropServices;

namespace Raytracer.MathTypes
{
	[StructLayout(LayoutKind.Sequential)]
    public struct Normal
	{
        public double X;
        public double Y;
        public double Z;
				
		public Normal(double x, double y, double z)
		{
			X = x;
			Y = y;
			Z = z;
		}

        private static readonly Normal _Invalid;
        public static Normal Invalid { get { return _Invalid; } }
		
		public Normal Normalize()
		{
			var length = this.Length;

            if (length == 0)
                return this;
                //throw new DivideByZeroException("Trying to normalize a vector with length of zero.");

            return this / length;
		}

	    private double Length
		{
            get
            {
			    return Math.Sqrt(LengthSquared);
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
			if (obj is Normal)
			{
				var v = (Normal)obj;
				return (X == v.X) && (Y == v.Y) && (Z == v.Z);
			}
			return false;
		}

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})", X, Y, Z);
        }
		
		public static bool operator==(Normal u, Normal v)
		{
		    return (u.X == v.X) && (u.Y == v.Y) && (u.Z == v.Z);
		}
        
        public static bool operator!=(Normal u, Normal v)
		{
			return !(u == v);
		}
		
		public static Normal operator-(Normal v)
		{
			return new Normal(-v.X, -v.Y, -v.Z);
		}

		public static Normal operator+(Normal v, Normal w)
		{
            return new Normal(v.X + w.X, v.Y + w.Y, v.Z + w.Z);
		}

        public static Normal operator-(Normal v, double s)
		{
			return new Normal(v.X - s, v.Y - s, v.Z - s);
		}

        public static Normal operator -(Normal v, Vector w)
        {
            return new Normal(v.X - w.X, v.Y - w.Y, v.Z - w.Z);
        }

        public static Normal operator -(Normal v, Point w)
        {
            return new Normal(v.X - w.X, v.Y - w.Y, v.Z - w.Z);
        }

        public static Normal operator*(Normal v, double s)
		{
		    return new Normal(v.X * s, v.Y * s, v.Z * s);
		}

        public static Normal operator*(double s, Normal v)
		{
		    return new Normal(v.X * s, v.Y * s, v.Z * s);
		}

        public static Normal operator/(Normal v, double s)
		{
			return new Normal(v.X / s, v.Y / s, v.Z / s);
		}

	    public static explicit operator Normal(Point point)
        {
            return new Normal(point.X, point.Y, point.Z);
        }

        public static explicit operator Normal(Vector vector)
		{
			return new Normal(vector.X, vector.Y, vector.Z);
		}

		public static explicit operator Vector(Normal normal)
		{
			return new Vector(normal.X, normal.Y, normal.Z);
		}

        public Normal Transform(Matrix matrix)
        {
            var x = ((this.X * matrix.M11) + (this.Y * matrix.M21)) + (this.Z * matrix.M31);
            var y = ((this.X * matrix.M12) + (this.Y * matrix.M22)) + (this.Z * matrix.M32);
            var z = ((this.X * matrix.M13) + (this.Y * matrix.M23)) + (this.Z * matrix.M33);

            return new Normal(x, y, z);
        }

        public double this[int index]
        {
            get
            {
                switch (index)
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
                switch (index)
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

        public Normal Faceforward(Vector v)
        {
            return (Vector.DotProduct(this, v) < 0.0) ? -this : this;
        }
    }
}
