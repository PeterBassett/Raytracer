using System;
using System.Runtime.InteropServices;

namespace Raytracer.MathTypes
{
	[StructLayout(LayoutKind.Sequential)]
    public struct Vector
	{
		public double X;
        public double Y;
        public double Z;

		public Vector(double x, double y, double z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public Vector(Vector vector)
		{
			X = vector.X;
			Y = vector.Y;
			Z = vector.Z;
		}
        
        public static readonly Vector Zero	= new Vector(0.0f, 0.0f, 0.0f);
		
        public static double DotProduct(Vector a, Vector b)
        {
            return DotProduct(a.X, a.Y, a.Z, b.X, b.Y, b.Z);
        }

        public static double DotProduct(Vector a, Normal b)
        {
            return DotProduct(a.X, a.Y, a.Z, b.X, b.Y, b.Z);
        }

        public static double DotProduct(Normal a, Vector b)
        {
            return DotProduct(a.X, a.Y, a.Z, b.X, b.Y, b.Z);
        }

        public static double DotProduct(Point a, Vector b)
        {
            return DotProduct(a.X, a.Y, a.Z, b.X, b.Y, b.Z);
        }

        public static double DotProduct(Vector a, Point b)
        {
            return DotProduct(a.X, a.Y, a.Z, b.X, b.Y, b.Z);
        }

        public static double DotProduct(double aX, double aY, double aZ, double bX, double bY, double bZ)
        {
            return (aX * bX) + (aY * bY) + (aZ * bZ);
        }
		
        public static Vector CrossProduct(double aX, double aY, double aZ, double bX, double bY, double bZ)
        {
        	return new Vector( 
				aY * bZ - aZ * bY, 
				aZ * bX - aX * bZ, 
				aX * bY - aY * bX );
		}

        public static Vector CrossProduct(Vector a, Vector b)
		{
			return CrossProduct(a.X, a.Y, a.Z, b.X, b.Y, b.Z);
		}

        public static Vector CrossProduct(Vector a, Normal b)
        {
            return CrossProduct(a.X, a.Y, a.Z, b.X, b.Y, b.Z);
        }


        public static bool ApproxEqual(Vector v, Vector u)
		{
			return ApproxEqual(v,u, MathLib.Epsilon);
		}
        
        public static bool ApproxEqual(Vector v, Vector u, double tolerance)
		{
			return
				(
				(System.Math.Abs(v.X - u.X) <= tolerance) &&
				(System.Math.Abs(v.Y - u.Y) <= tolerance) &&
				(System.Math.Abs(v.Z - u.Z) <= tolerance)
				);
		}

        public void RotateX(double amnt, ref Vector dest)
        {
            double s = MathLib.mLibSin(amnt);
            double c = MathLib.mLibCos(amnt);
            double y = this.Y;
            double z = this.Z;

            dest.X = this.X;
            dest.Y = (y * c) - (z * s);
            dest.Z = (y * s) + (z * c);
        }

        public void RotateY(double amnt, ref Vector dest)
        {
            double s = MathLib.mLibSin(amnt);
            double c = MathLib.mLibCos(amnt);
            double x = this.X;
            double z = this.Z;

            dest.X = (x * c) + (z * s);
            dest.Y = this.Y;
            dest.Z = (z * c) - (x * s);
        }

        public void RotateZ(double amnt, ref Vector dest)
        {
            double s = MathLib.mLibSin(amnt);
            double c = MathLib.mLibCos(amnt);
            double x = this.X;
            double y = this.Y;

            dest.X = (x * c) - (y * s);
            dest.Y = (y * c) + (x * s);
            dest.Z = this.Z;
        }

		public Vector Normalize()
		{
            double lengthSquared = LengthSquared;

		    if (lengthSquared == 0)
		        return this;
                //throw new DivideByZeroException("Trying to normalize a vector with length of zero.");

            return this / Math.Sqrt(lengthSquared);
		}

        public double Length
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
			if (obj is Vector)
			{
				Vector v = (Vector)obj;
				return (X == v.X) && (Y == v.Y) && (Z == v.Z);
			}
			return false;
		}

        public static bool operator==(Vector u, Vector v)
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

        public static bool operator!=(Vector u, Vector v)
		{
            return !(u == v);
		}
        
        public static Vector operator-(Vector v)
		{			
            return new Vector(-v.X, -v.Y, -v.Z);
		}

        public static Vector Add(double aX, double aY, double aZ, double bX, double bY, double bZ)
        {
        	return new Vector(aX + bX, 
                              aY + bY, 
                              aZ + bZ);
		}

		public static Vector operator+(Vector a, Vector b)
		{
            return Add(a.X, a.Y, a.Z, b.X, b.Y, b.Z);
		}

        public static Vector operator+(Vector a, double s)
		{
			return Add(a.X, a.Y, a.Z, s, s, s);
		}

        public static Vector operator +(Vector a, Normal b)
        {
            return Add(a.X, a.Y, a.Z, b.X, b.Y, b.Z);
        }
        
        public static Vector Add(Vector a, Normal b)
        {
            return Add(a.X, a.Y, a.Z, b.X, b.Y, b.Z);
        }

    	public static Vector operator+(double s, Vector a)
		{
			return Add(a.X, a.Y, a.Z, s, s, s);
		}

        public static Vector Subtract(double aX, double aY, double aZ, double bX, double bY, double bZ)
        {
        	return new Vector(aX - bX, 
                              aY - bY, 
                              aZ - bZ);
		}

        public static Vector operator-(Vector a, Vector b)
		{
			return Subtract(a.X, a.Y, a.Z, b.X, b.Y, b.Z);
		}

        public static Vector operator-(Vector a, double s)
		{
			return Subtract(a.X, a.Y, a.Z, s, s, s);
		}

        public static Vector operator-(double s, Vector a)
		{
            return Subtract(s, s, s, a.X, a.Y, a.Z);
		}

        public static Vector Multiply(double aX, double aY, double aZ, double bX, double bY, double bZ)
        {
        	return new Vector(aX * bX, 
                              aY * bY, 
                              aZ * bZ);
		}

        public static Vector operator*(Vector a, double s)
		{
            return Multiply(a.X, a.Y, a.Z, s, s, s);
        }
		
        public static Vector operator*(double s, Vector a)
		{
            return Multiply(s, s, s, a.X, a.Y, a.Z);
		}
        
        public static Vector Divide(double aX, double aY, double aZ, double bX, double bY, double bZ)
        {
        	return new Vector(aX / bX, 
                              aY / bY, 
                              aZ / bZ);
		}

        public static Vector operator/(Vector a, double s)
		{
            return Divide(a.X, a.Y, a.Z, s, s, s);
		}

        public static Vector operator/(double s, Vector a)
		{
            return Divide(s, s, s, a.X, a.Y, a.Z);
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

        public static explicit operator double[](Vector v)
		{
			double[] array = new double[3];
			array[0] = v.X;
			array[1] = v.Y;
			array[2] = v.Z;
			return array;
		}
		

        public Vector Negated()
        {
            return new Vector(-this.X, -this.Y, -this.Z);
        }

        public Vector Lerp(Vector a, double t) {
            return this + ((a - this) * t);
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})", X, Y, Z);
        }

        public Vector Transform(Matrix matrix)
        {
            var x = (((this.X * matrix.M11) + (this.Y * matrix.M21)) + (this.Z * matrix.M31));
            var y = (((this.X * matrix.M12) + (this.Y * matrix.M22)) + (this.Z * matrix.M32));
            var z = (((this.X * matrix.M13) + (this.Y * matrix.M23)) + (this.Z * matrix.M33));

            return new Vector(x, y, z);
        }
    }

}
