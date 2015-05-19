using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using Raytracer.MathTypes.Converters;

namespace Raytracer.MathTypes
{
	[StructLayout(LayoutKind.Sequential)]
    [TypeConverter(typeof(PointConverter))]
    public struct Point
    {
        public double X;
        public double Y;
        public double Z;

        public Point(Point point) : this(point.X, point.Y, point.Z) 
        {
        }
		
		public Point(double x, double y, double z)
		{
			X = x;
			Y = y;
			Z = z;
		}

        private static Point _Zero = new Point();
        public static Point Zero { get { return _Zero; } }
		
        public static double Distance(Point p1, Point p2)
		{
            return (p1 - p2).Length;
		}

		public static double DistanceSquared(Point p1, Point p2)
		{
			return (p1 - p2).LengthSquared;
		}

        public static Point operator +(Point value1, Point value2)
		{
			Point vector;
			vector.X = value1.X + value2.X;
			vector.Y = value1.Y + value2.Y;
			vector.Z = value1.Z + value2.Z;
			return vector;
		}

		public static Point operator+(Point point, Vector vector)
		{
			return new Point(point.X + vector.X, 
                             point.Y + vector.Y, 
                             point.Z + vector.Z);
		}

        public static Point operator +(Point point, Normal normal)
        {
            return new Point(point.X + normal.X, 
                             point.Y + normal.Y, 
                             point.Z + normal.Z);
        }

        public static Point operator +(Point point, double offset)
        {
            return new Point(point.X + offset, 
                             point.Y + offset, 
                             point.Z + offset);
        }

		public static Vector operator -(Point a, Point b)
		{
			return new Vector(a.X - b.X, 
                              a.Y - b.Y, 
                              a.Z - b.Z);
		}

		public static Point operator -(Point p, Vector v)
		{
			return new Point(p.X - v.X, 
                             p.Y - v.Y, 
                             p.Z - v.Z);
		}

        public static Point operator -(Point p, double s)
        {
            return new Point(p.X - s, 
                             p.Y - s, 
                             p.Z - s);
        }

        public static Point operator *(double scaleFactor, Point value)
        {
            return value * scaleFactor;
        }

		public static Point operator *(Point p, double s)
		{
			return new Point(p.X * s, 
                             p.Y * s,
                             p.Z * s);
		}

		public static Point operator *(Point p, Vector v)
		{
            return new Point(p.X * v.X,
                             p.Y * v.Y,
                             p.Z * v.Z);
		}

		public static Point operator /(Point p, double divider)
		{
            var reciprocal = 1.0 / divider;

            return new Point(p.X * reciprocal,
                             p.Y * reciprocal,
                             p.Z * reciprocal);
		}

        public static Point operator -(Point v)
        {
            return new Point(-v.X, -v.Y, -v.Z);
        }
		
        public static bool ApproxEqual(Point v, Point u)
		{
			return ApproxEqual(v,u, MathLib.Epsilon);
		}

		public static bool ApproxEqual(Point v, Point u, double tolerance)
		{
			return
				(
				(System.Math.Abs(v.X - u.X) <= tolerance) &&
				(System.Math.Abs(v.Y - u.Y) <= tolerance) &&
				(System.Math.Abs(v.Z - u.Z) <= tolerance)
				);
		}

        public void RotateX(double amnt, ref Point dest)
        {
            double s = MathLib.mLibSin(amnt);
            double c = MathLib.mLibCos(amnt);
            double y = this.Y;
            double z = this.Z;

            dest.X = this.X;
            dest.Y = (y * c) - (z * s);
            dest.Z = (y * s) + (z * c);
        }

        public void RotateY(double amnt, ref Point dest)
        {
            double s = MathLib.mLibSin(amnt);
            double c = MathLib.mLibCos(amnt);
            double x = this.X;
            double z = this.Z;

            dest.X = (x * c) + (z * s);
            dest.Y = this.Y;
            dest.Z = (z * c) - (x * s);
        }

        public void RotateZ(double amnt, ref Point dest)
        {
            double s = MathLib.mLibSin(amnt);
            double c = MathLib.mLibCos(amnt);
            double x = this.X;
            double y = this.Y;

            dest.X = (x * c) - (y * s);
            dest.Y = (y * c) + (x * s);
            dest.Z = this.Z;
        }

		public Point Normalize(Point n)
		{
			double length = n.Length;

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
			if (obj is Point)
			{
				Point v = (Point)obj;
				return (X == v.X) && (Y == v.Y) && (Z == v.Z);
			}
			return false;
		}

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})", X, Y, Z);
        }
		
		public static bool operator==(Point u, Point v)
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
        
        public static bool operator!=(Point u, Point v)
		{
			return !(u == v);
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

        public static explicit operator Point(Vector vector)
		{
			return new Point(vector.X, vector.Y, vector.Z);
		}

		public static explicit operator Vector(Point normal)
		{
			return new Vector(normal.X, normal.Y, normal.Z);
		}

        public Point Transform(Matrix matrix)
        {
            var x = (((this.X * matrix.M11) + (this.Y * matrix.M21)) + (this.Z * matrix.M31)) + matrix.M41;
            var y = (((this.X * matrix.M12) + (this.Y * matrix.M22)) + (this.Z * matrix.M32)) + matrix.M42;
            var z = (((this.X * matrix.M13) + (this.Y * matrix.M23)) + (this.Z * matrix.M33)) + matrix.M43;

            var w = (((this.X * matrix.M14) + (this.Y * matrix.M24)) + (this.Z * matrix.M34)) + matrix.M44;

            if(w == 1)
                return new Point(x, y, z);
            else
                return new Point(x, y, z) / w;
        }

        public static Point Parse(string value)
        {
            var items = value.Split(',');

            return new Point(double.Parse(items[0]), 
                             double.Parse(items[1]), 
                             double.Parse(items[2]));
        }
    }
}
