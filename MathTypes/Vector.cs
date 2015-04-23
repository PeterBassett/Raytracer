#region Sharp3D.Math, Copyright(C) 2003-2004 Eran Kampf, Licensed under LGPL.
//	Sharp3D.Math math library
//	Copyright (C) 2003-2004  
//	Eran Kampf
//	tentacle@zahav.net.il
//	http://tentacle.flipcode.com
//
//	This library is free software; you can redistribute it and/or
//	modify it under the terms of the GNU Lesser General Public
//	License as published by the Free Software Foundation; either
//	version 2.1 of the License, or (at your option) any later version.
//
//	This library is distributed in the hope that it will be useful,
//	but WITHOUT ANY WARRANTY; without even the implied warranty of
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//	Lesser General Public License for more details.
//
//	You should have received a copy of the GNU Lesser General Public
//	License along with this library; if not, write to the Free Software
//	Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
#endregion
using System;
using System.Runtime.InteropServices;

namespace Raytracer.MathTypes
{
	[StructLayout(LayoutKind.Sequential)]
    public struct Vector
	{
		#region Private fields
		public double _x;
        public double _y;
        public double _z;
		#endregion

		#region Constructors
        
		public Vector(double x, double y, double z)
		{
			_x = x;
			_y = y;
			_z = z;
		}

		public Vector(Vector vector)
		{
			_x = vector._x;
			_y = vector._y;
			_z = vector._z;
		}
		#endregion

		#region Constants
		/// <summary>
		/// 3-Dimentional double-precision floating point zero vector.
		/// </summary>
		public static readonly Vector Zero	= new Vector(0.0f, 0.0f, 0.0f);
		/// <summary>
		/// 3-Dimentional double-precision floating point X-Axis vector.
		/// </summary>
		public static readonly Vector XAxis	= new Vector(1.0f, 0.0f, 0.0f);
		/// <summary>
		/// 3-Dimentional double-precision floating point Y-Axis vector.
		/// </summary>
		public static readonly Vector YAxis	= new Vector(0.0f, 1.0f, 0.0f);
		/// <summary>
		/// 3-Dimentional double-precision floating point Y-Axis vector.
		/// </summary>
		public static readonly Vector ZAxis	= new Vector(0.0f, 0.0f, 1.0f);
		#endregion

		#region Public properties
		public double X
		{
			get { return _x; }
			set { _x = value;}
		}

		public double Y
		{
			get { return _y; }
			set { _y = value;}
		}

		public double Z
		{
			get { return _z; }
			set { _z = value;}
		}
		#endregion

		#region Public Static Vector3 Arithmetics
		/// <summary>
		/// Adds two vectors.
		/// </summary>
		/// <param name="v">A <see cref="Vector"/> instance.</param>
		/// <param name="w">A <see cref="Vector"/> instance.</param>
		/// <returns>A new <see cref="Vector"/> instance containing the sum.</returns>
		public static Vector Add(Vector v, Vector w)
		{
			return new Vector(v._x + w._x, v._y + w._y, v._z + w._z);
		}
		/// <summary>
		/// Adds a vector and a scalar.
		/// </summary>
		/// <param name="v">A <see cref="Vector"/> instance.</param>
		/// <param name="s">A scalar.</param>
		/// <returns>A new <see cref="Vector"/> instance containing the sum.</returns>
		public static Vector Add(Vector v, double s)
		{
			return new Vector(v._x + s, v._y + s, v._z + s);
		}
		/// <summary>
		/// Adds two vectors and put the result in the third vector.
		/// </summary>
		/// <param name="u">A <see cref="Vector"/> instance.</param>
		/// <param name="v">A <see cref="Vector"/> instance</param>
		/// <param name="w">A <see cref="Vector"/> instance to hold the result.</param>
		public static void Add(Vector u, Vector v, Vector w)
		{
			w._x = u._x + v._x;
			w._y = u._y + v._y;
			w._z = u._z + v._z;
		}
		/// <summary>
		/// Adds a vector and a scalar and put the result into another vector.
		/// </summary>
		/// <param name="u">A <see cref="Vector"/> instance.</param>
		/// <param name="s">A scalar.</param>
		/// <param name="v">A <see cref="Vector"/> instance to hold the result.</param>
		public static void Add(Vector u, double s, Vector v)
		{
			v._x = u._x + s;
			v._y = u._y + s;
			v._z = u._z + s;
		}
		/// <summary>
		/// Subtracts a vector from a vector.
		/// </summary>
		/// <param name="v">A <see cref="Vector"/> instance.</param>
		/// <param name="w">A <see cref="Vector"/> instance.</param>
		/// <returns>A new <see cref="Vector"/> instance containing the difference.</returns>
		/// <remarks>
		///	result[i] = v[i] - w[i].
		/// </remarks>
		public static Vector Subtract(Vector v, Vector w)
		{
			return new Vector(v._x - w._x, v._y - w._y, v._z - w._z);
		}
		/// <summary>
		/// Subtracts a scalar from a vector.
		/// </summary>
		/// <param name="v">A <see cref="Vector"/> instance.</param>
		/// <param name="s">A scalar.</param>
		/// <returns>A new <see cref="Vector"/> instance containing the difference.</returns>
		/// <remarks>
		/// result[i] = v[i] - s
		/// </remarks>
		public static Vector Subtract(Vector v, double s)
		{
			return new Vector(v._x - s, v._y - s, v._z - s);
		}
		/// <summary>
		/// Subtracts a vector from a scalar.
		/// </summary>
		/// <param name="v">A <see cref="Vector"/> instance.</param>
		/// <param name="s">A scalar.</param>
		/// <returns>A new <see cref="Vector"/> instance containing the difference.</returns>
		/// <remarks>
		/// result[i] = s - v[i]
		/// </remarks>
		public static Vector Subtract(double s, Vector v)
		{
			return new Vector(s - v._x, s - v._y, s - v._z);
		}
		/// <summary>
		/// Subtracts a vector from a second vector and puts the result into a third vector.
		/// </summary>
		/// <param name="u">A <see cref="Vector"/> instance.</param>
		/// <param name="v">A <see cref="Vector"/> instance</param>
		/// <param name="w">A <see cref="Vector"/> instance to hold the result.</param>
		/// <remarks>
		///	w[i] = v[i] - w[i].
		/// </remarks>
		public static void Subtract(Vector u, Vector v, Vector w)
		{
			w._x = u._x - v._x;
			w._y = u._y - v._y;
			w._z = u._z - v._z;
		}
		/// <summary>
		/// Subtracts a vector from a scalar and put the result into another vector.
		/// </summary>
		/// <param name="u">A <see cref="Vector"/> instance.</param>
		/// <param name="s">A scalar.</param>
		/// <param name="v">A <see cref="Vector"/> instance to hold the result.</param>
		/// <remarks>
		/// v[i] = u[i] - s
		/// </remarks>
		public static void Subtract(Vector u, double s, Vector v)
		{
			v._x = u._x - s;
			v._y = u._y - s;
			v._z = u._z - s;
		}
		/// <summary>
		/// Subtracts a scalar from a vector and put the result into another vector.
		/// </summary>
		/// <param name="u">A <see cref="Vector"/> instance.</param>
		/// <param name="s">A scalar.</param>
		/// <param name="v">A <see cref="Vector"/> instance to hold the result.</param>
		/// <remarks>
		/// v[i] = s - u[i]
		/// </remarks>
		public static void Subtract(double s, Vector u, Vector v)
		{
			v._x = s - u._x;
			v._y = s - u._y;
			v._z = s - u._z;
		}
		/// <summary>
		/// Divides a vector by another vector.
		/// </summary>
		/// <param name="u">A <see cref="Vector"/> instance.</param>
		/// <param name="v">A <see cref="Vector"/> instance.</param>
		/// <returns>A new <see cref="Vector"/> containing the quotient.</returns>
		/// <remarks>
		///	result[i] = u[i] / v[i].
		/// </remarks>
		public static Vector Divide(Vector u, Vector v)
		{
			return new Vector(u._x / v._x, u._y / v._y, u._z / v._z);
		}
		/// <summary>
		/// Divides a vector by a scalar.
		/// </summary>
		/// <param name="v">A <see cref="Vector"/> instance.</param>
		/// <param name="s">A scalar</param>
		/// <returns>A new <see cref="Vector"/> containing the quotient.</returns>
		/// <remarks>
		/// result[i] = v[i] / s;
		/// </remarks>
		public static Vector Divide(Vector v, double s)
		{
			return new Vector(v._x / s, v._y / s, v._z / s);
		}
		/// <summary>
		/// Divides a scalar by a vector.
		/// </summary>
		/// <param name="v">A <see cref="Vector"/> instance.</param>
		/// <param name="s">A scalar</param>
		/// <returns>A new <see cref="Vector"/> containing the quotient.</returns>
		/// <remarks>
		/// result[i] = s / v[i]
		/// </remarks>
		public static Vector Divide(double s, Vector v)
		{
			return new Vector(s / v._x, s/ v._y, s / v._z);
		}
		/// <summary>
		/// Divides a vector by another vector.
		/// </summary>
		/// <param name="u">A <see cref="Vector"/> instance.</param>
		/// <param name="v">A <see cref="Vector"/> instance.</param>
		/// <param name="w">A <see cref="Vector"/> instance to hold the result.</param>
		/// <remarks>
		/// w[i] = u[i] / v[i]
		/// </remarks>
		public static void Divide(Vector u, Vector v, Vector w)
		{
			w._x = u._x / v._x;
			w._y = u._y / v._y;
			w._z = u._z / v._z;
		}
		/// <summary>
		/// Divides a vector by a scalar.
		/// </summary>
		/// <param name="u">A <see cref="Vector"/> instance.</param>
		/// <param name="s">A scalar</param>
		/// <param name="v">A <see cref="Vector"/> instance to hold the result.</param>
		/// <remarks>
		/// v[i] = u[i] / s
		/// </remarks>
		public static void Divide(Vector u, double s, Vector v)
		{
			v._x = u._x / s;
			v._y = u._y / s;
			v._z = u._z / s;
		}
		/// <summary>
		/// Divides a scalar by a vector.
		/// </summary>
		/// <param name="u">A <see cref="Vector"/> instance.</param>
		/// <param name="s">A scalar</param>
		/// <param name="v">A <see cref="Vector"/> instance to hold the result.</param>
		/// <remarks>
		/// v[i] = s / u[i]
		/// </remarks>
		public static void Divide(double s, Vector u, Vector v)
		{
			v._x = s / u._x;
			v._y = s / u._y;
			v._z = s / u._z;
		}
		/// <summary>
		/// Multiplies a vector by a scalar.
		/// </summary>
		/// <param name="u">A <see cref="Vector"/> instance.</param>
		/// <param name="s">A scalar.</param>
		/// <returns>A new <see cref="Vector"/> containing the result.</returns>
		public static Vector Multiply(Vector u, double s)
		{
			return new Vector(u._x * s, u._y * s, u._z * s);
		}
		/// <summary>
		/// Multiplies a vector by a scalar and put the result in another vector.
		/// </summary>
		/// <param name="u">A <see cref="Vector"/> instance.</param>
		/// <param name="s">A scalar.</param>
		/// <param name="v">A <see cref="Vector"/> instance to hold the result.</param>
		public static void Multiply(Vector u, double s, Vector v)
		{
			v._x = u._x * s;
			v._y = u._y * s;
			v._z = u._z * s;
		}
		/// <summary>
		/// Calculates the dot product of two vectors.
		/// </summary>
		/// <param name="u">A <see cref="Vector"/> instance.</param>
		/// <param name="v">A <see cref="Vector"/> instance.</param>
		/// <returns>The dot product value.</returns>
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
		
        public static Vector CrossProduct(Vector u, Vector v)
		{
			return new Vector( 
				u._y*v._z - u._z*v._y, 
				u._z*v._x - u._x*v._z, 
				u._x*v._y - u._y*v._x );
		}

        public static Vector CrossProduct(Vector u, Normal v)
        {
            return new Vector(
                u._y * v.Z - u._z * v.Y,
                u._z * v.X - u._x * v.Z,
                u._x * v.Y - u._y * v.X);
        }
		/// <summary>
		/// Calculates the cross product of two vectors.
		/// </summary>
		/// <param name="u">A <see cref="Vector"/> instance.</param>
		/// <param name="v">A <see cref="Vector"/> instance.</param>
		/// <param name="w">A <see cref="Vector"/> instance to hold the cross product result.</param>
		public static void CrossProduct(Vector u, Vector v, Vector w)
		{
			w._x = u._y*v._z - u._z*v._y;
			w._y = u._z*v._x - u._x*v._z;
			w._z = u._x*v._y - u._y*v._x;
		}
		/// <summary>
		/// Negates a vector.
		/// </summary>
		/// <param name="v">A <see cref="Vector"/> instance.</param>
		/// <returns>A new <see cref="Vector"/> instance containing the negated values.</returns>
		public static Vector Negate(Vector v)
		{
			return new Vector(-v._x, -v._y, -v._z);
		}
		/// <summary>
		/// Tests whether two vectors are approximately equal using default tolerance value.
		/// </summary>
		/// <param name="v">A <see cref="Vector"/> instance.</param>
		/// <param name="u">A <see cref="Vector"/> instance.</param>
		/// <returns>True if the two vectors are approximately equal; otherwise, False.</returns>
		public static bool ApproxEqual(Vector v, Vector u)
		{
			return ApproxEqual(v,u, MathLib.Epsilon);
		}
		/// <summary>
		/// Tests whether two vectors are approximately equal given a tolerance value.
		/// </summary>
		/// <param name="v">A <see cref="Vector"/> instance.</param>
		/// <param name="u">A <see cref="Vector"/> instance.</param>
		/// <param name="tolerance">The tolerance value used to test approximate equality.</param>
		/// <returns>True if the two vectors are approximately equal; otherwise, False.</returns>
		public static bool ApproxEqual(Vector v, Vector u, double tolerance)
		{
			return
				(
				(System.Math.Abs(v._x - u._x) <= tolerance) &&
				(System.Math.Abs(v._y - u._y) <= tolerance) &&
				(System.Math.Abs(v._z - u._z) <= tolerance)
				);
		}

        public void RotateX(double amnt, ref Vector dest)
        {
            double s = MathLib.mLibSin(amnt);
            double c = MathLib.mLibCos(amnt);
            double y = this._y;
            double z = this._z;

            dest._x = this._x;
            dest._y = (y * c) - (z * s);
            dest._z = (y * s) + (z * c);
        }

        public void RotateY(double amnt, ref Vector dest)
        {
            double s = MathLib.mLibSin(amnt);
            double c = MathLib.mLibCos(amnt);
            double x = this._x;
            double z = this._z;

            dest._x = (x * c) + (z * s);
            dest._y = this._y;
            dest._z = (z * c) - (x * s);
        }

        public void RotateZ(double amnt, ref Vector dest)
        {
            double s = MathLib.mLibSin(amnt);
            double c = MathLib.mLibCos(amnt);
            double x = this._x;
            double y = this._y;

            dest._x = (x * c) - (y * s);
            dest._y = (y * c) + (x * s);
            dest._z = this._z;
        }

		#endregion

		#region Public Methods
		/// <summary>
		/// Scale the vector so that its length is 1.
		/// </summary>
		public void Normalize()
		{
			double length = GetLength();
			if (length == 0)
			{
				throw new DivideByZeroException("Trying to normalize a vector with length of zero.");
			}

			_x /= length;
			_y /= length;
			_z /= length;
		}
		/// <summary>
		/// Returns the length of the vector.
		/// </summary>
		/// <returns>The length of the vector. (Sqrt(X*X + Y*Y + Z*Z))</returns>
		public double GetLength()
		{
			return System.Math.Sqrt(_x*_x + _y*_y + _z*_z);
		}
		/// <summary>
		/// Returns the squared length of the vector.
		/// </summary>
		/// <returns>The squared length of the vector. (X*X + Y*Y + Z*Z)</returns>
		public double GetLengthSquared()
		{
			return (_x*_x + _y*_y + _z*_z);
		}
		#endregion

		#region Overrides
		/// <summary>
		/// Returns the hashcode for this instance.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			return _x.GetHashCode() ^ _y.GetHashCode() ^ _z.GetHashCode();
		}
		/// <summary>
		/// Returns a value indicating whether this instance is equal to
		/// the specified object.
		/// </summary>
		/// <param name="obj">An object to compare to this instance.</param>
		/// <returns>True if <paramref name="obj"/> is a <see cref="Vector"/> and has the same values as this instance; otherwise, False.</returns>
		public override bool Equals(object obj)
		{
			if (obj is Vector)
			{
				Vector v = (Vector)obj;
				return (_x == v._x) && (_y == v._y) && (_z == v._z);
			}
			return false;
		}

        /// <summary>
        /// Returns a string representation of this object.
        /// </summary>
        /// <returns>A string representation of this object.</returns>
        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})", _x, _y, _z);
        }
		#endregion
		
		#region Comparison Operators
		/// <summary>
		/// Tests whether two specified vectors are equal.
		/// </summary>
		/// <param name="u">The left-hand vector.</param>
		/// <param name="v">The right-hand vector.</param>
		/// <returns>True if the two vectors are equal; otherwise, False.</returns>
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

			return (u._x == v._x) && (u._y == v._y) && (u._z == v._z);
		}
		/// <summary>
		/// Tests whether two specified vectors are not equal.
		/// </summary>
		/// <param name="u">The left-hand vector.</param>
		/// <param name="v">The right-hand vector.</param>
		/// <returns>True if the two vectors are not equal; otherwise, False.</returns>
		public static bool operator!=(Vector u, Vector v)
		{
			if (Object.Equals(u, null))
			{
				return !Object.Equals(v, null);
			}

			if (Object.Equals(v, null))
			{
				return !Object.Equals(u, null);
			}

			return !((u._x == v._x) && (u._y == v._y) && (u._z == v._z));
		}
		/// <summary>
		/// Tests if a vector's components are greater than another vector's components.
		/// </summary>
		/// <param name="u">The left-hand vector.</param>
		/// <param name="v">The right-hand vector.</param>
		/// <returns>True if the left-hand vector's components are greater than the right-hand vector's component; otherwise, False.</returns>
		public static bool operator>(Vector u, Vector v)
		{
			return (
				(u._x > v._x) && 
				(u._y > v._y) && 
				(u._z > v._z));
		}
		/// <summary>
		/// Tests if a vector's components are smaller than another vector's components.
		/// </summary>
		/// <param name="u">The left-hand vector.</param>
		/// <param name="v">The right-hand vector.</param>
		/// <returns>True if the left-hand vector's components are smaller than the right-hand vector's component; otherwise, False.</returns>
		public static bool operator<(Vector u, Vector v)
		{
			return (
				(u._x < v._x) && 
				(u._y < v._y) && 
				(u._z < v._z));
		}
		/// <summary>
		/// Tests if a vector's components are greater or equal than another vector's components.
		/// </summary>
		/// <param name="u">The left-hand vector.</param>
		/// <param name="v">The right-hand vector.</param>
		/// <returns>True if the left-hand vector's components are greater or equal than the right-hand vector's component; otherwise, False.</returns>
		public static bool operator>=(Vector u, Vector v)
		{
			return (
				(u._x >= v._x) && 
				(u._y >= v._y) && 
				(u._z >= v._z));
		}
		/// <summary>
		/// Tests if a vector's components are smaller or equal than another vector's components.
		/// </summary>
		/// <param name="u">The left-hand vector.</param>
		/// <param name="v">The right-hand vector.</param>
		/// <returns>True if the left-hand vector's components are smaller or equal than the right-hand vector's component; otherwise, False.</returns>
		public static bool operator<=(Vector u, Vector v)
		{
			return (
				(u._x <= v._x) && 
				(u._y <= v._y) && 
				(u._z <= v._z));
		}
		#endregion

		#region Unary Operators
		/// <summary>
		/// Negates the values of the vector.
		/// </summary>
		/// <param name="v">A <see cref="Vector"/> instance.</param>
		/// <returns>A new <see cref="Vector"/> instance containing the negated values.</returns>
		public static Vector operator-(Vector v)
		{
			return Vector.Negate(v);
		}
		#endregion

		#region Binary Operators
		/// <summary>
		/// Adds two vectors.
		/// </summary>
		/// <param name="u">A <see cref="Vector"/> instance.</param>
		/// <param name="v">A <see cref="Vector"/> instance.</param>
		/// <returns>A new <see cref="Vector"/> instance containing the sum.</returns>
		public static Vector operator+(Vector u, Vector v)
		{
			return Vector.Add(u,v);
		}

        public static Vector operator+(Vector v, double s)
		{
			return Vector.Add(v,s);
		}

        public static Vector operator +(Vector v, Normal w)
        {
            return new Vector(v.X + w.X, v.Y + w.Y, v.Z + w.Z);
        }

		/// <summary>
		/// Adds a vector and a scalar.
		/// </summary>
		/// <param name="v">A <see cref="Vector"/> instance.</param>
		/// <param name="s">A scalar.</param>
		/// <returns>A new <see cref="Vector"/> instance containing the sum.</returns>
		public static Vector operator+(double s, Vector v)
		{
			return Vector.Add(v,s);
		}
		/// <summary>
		/// Subtracts a vector from a vector.
		/// </summary>
		/// <param name="u">A <see cref="Vector"/> instance.</param>
		/// <param name="v">A <see cref="Vector"/> instance.</param>
		/// <returns>A new <see cref="Vector"/> instance containing the difference.</returns>
		/// <remarks>
		///	result[i] = v[i] - w[i].
		/// </remarks>
		public static Vector operator-(Vector u, Vector v)
		{
			return Vector.Subtract(u,v);
		}
		/// <summary>
		/// Subtracts a scalar from a vector.
		/// </summary>
		/// <param name="v">A <see cref="Vector"/> instance.</param>
		/// <param name="s">A scalar.</param>
		/// <returns>A new <see cref="Vector"/> instance containing the difference.</returns>
		/// <remarks>
		/// result[i] = v[i] - s
		/// </remarks>
		public static Vector operator-(Vector v, double s)
		{
			return Vector.Subtract(v, s);
		}
		/// <summary>
		/// Subtracts a vector from a scalar.
		/// </summary>
		/// <param name="v">A <see cref="Vector"/> instance.</param>
		/// <param name="s">A scalar.</param>
		/// <returns>A new <see cref="Vector"/> instance containing the difference.</returns>
		/// <remarks>
		/// result[i] = s - v[i]
		/// </remarks>
		public static Vector operator-(double s, Vector v)
		{
			return Vector.Subtract(s, v);
		}

		/// <summary>
		/// Multiplies a vector by a scalar.
		/// </summary>
		/// <param name="v">A <see cref="Vector"/> instance.</param>
		/// <param name="s">A scalar.</param>
		/// <returns>A new <see cref="Vector"/> containing the result.</returns>
		public static Vector operator*(Vector v, double s)
		{
			return Vector.Multiply(v,s);
		}
		/// <summary>
		/// Multiplies a vector by a scalar.
		/// </summary>
		/// <param name="v">A <see cref="Vector"/> instance.</param>
		/// <param name="s">A scalar.</param>
		/// <returns>A new <see cref="Vector"/> containing the result.</returns>
		public static Vector operator*(double s, Vector v)
		{
			return Vector.Multiply(v,s);
		}
		/// <summary>
		/// Divides a vector by a scalar.
		/// </summary>
		/// <param name="v">A <see cref="Vector"/> instance.</param>
		/// <param name="s">A scalar</param>
		/// <returns>A new <see cref="Vector"/> containing the quotient.</returns>
		/// <remarks>
		/// result[i] = v[i] / s;
		/// </remarks>
		public static Vector operator/(Vector v, double s)
		{
			return Vector.Divide(v,s);
		}
		/// <summary>
		/// Divides a scalar by a vector.
		/// </summary>
		/// <param name="v">A <see cref="Vector"/> instance.</param>
		/// <param name="s">A scalar</param>
		/// <returns>A new <see cref="Vector"/> containing the quotient.</returns>
		/// <remarks>
		/// result[i] = s / v[i]
		/// </remarks>
		public static Vector operator/(double s, Vector v)
		{
			return Vector.Divide(s,v);
		}
		#endregion

		#region Array Indexing Operator
		/// <summary>
		/// Indexer ( [x, y] ).
		/// </summary>
		public double this[int index]
		{
			get	
			{
				switch( index ) 
				{
					case 0:
						return _x;
					case 1:
						return _y;
					case 2:
						return _z;
					default:
						throw new IndexOutOfRangeException();
				}
			}
			set 
			{
				switch( index ) 
				{
					case 0:
						_x = value;
						break;
					case 1:
						_y = value;
						break;
					case 2:
						_z = value;
						break;
					default:
						throw new IndexOutOfRangeException();
				}
			}

		}

		#endregion

		#region Conversion Operators
		/// <summary>
		/// Converts the vector to an array of double-precision floating point values.
		/// </summary>
		/// <param name="v">A <see cref="Vector"/> instance.</param>
		/// <returns>An array of double-precision floating point values.</returns>
		public static explicit operator double[](Vector v)
		{
			double[] array = new double[3];
			array[0] = v._x;
			array[1] = v._y;
			array[2] = v._z;
			return array;
		}
		#endregion

        public Vector Negated()
        {
            return new Vector(-this._x, -this._y, -this._z);
        }

        public Vector Lerp(Vector a, double t) {
            return this + ((a - this) * t);
        }
    }

}
