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
using System.ComponentModel;
using System.Diagnostics;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Runtime.InteropServices;

namespace Raytracer.MathTypes
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector2
	{
		#region Private fields
		private double _x;
		private double _y;
		#endregion

		#region Constructors
		public Vector2(double x, double y)
		{
			_x = x;
			_y = y;
		}

		public Vector2(Vector2 vector)
		{
			_x = vector.X;
			_y = vector.Y;
		}
		#endregion

		#region Constants
		/// <summary>
		/// 2-Dimentional double-precision floating point zero vector.
		/// </summary>
		public static readonly Vector2 Zero	= new Vector2(0.0f, 0.0f);
		/// <summary>
		/// 2-Dimentional double-precision floating point X-Axis vector.
		/// </summary>
		public static readonly Vector2 XAxis = new Vector2(1.0f, 0.0f);
		/// <summary>
		/// 2-Dimentional double-precision floating point Y-Axis vector.
		/// </summary>
		public static readonly Vector2 YAxis = new Vector2(0.0f, 1.0f);
		#endregion

		#region Public properties
		/// <summery>
		/// Gets or sets the x-coordinate of this vector.
		/// </summery>
		/// <value>The x-coordinate of this vector.</value>
		public double X
		{
			get { return _x; }
			set { _x = value;}
		}
		/// <summery>
		/// Gets or sets the y-coordinate of this vector.
		/// </summery>
		/// <value>The y-coordinate of this vector.</value>
		public double Y
		{
			get { return _y; }
			set { _y = value;}
		}
		#endregion

		#region Public Static Vector3 Arithmetics
		/// <summary>
		/// Adds two vectors.
		/// </summary>
		/// <param name="v">A <see cref="Vector2"/> instance.</param>
		/// <param name="w">A <see cref="Vector2"/> instance.</param>
		/// <returns>A new <see cref="Vector2"/> instance containing the sum.</returns>
		public static Vector2 Add(Vector2 v, Vector2 w)
		{
			return new Vector2(v.X + w.X, v.Y + w.Y);
		}
		/// <summary>
		/// Adds a vector and a scalar.
		/// </summary>
		/// <param name="v">A <see cref="Vector2"/> instance.</param>
		/// <param name="s">A scalar.</param>
		/// <returns>A new <see cref="Vector2"/> instance containing the sum.</returns>
		public static Vector2 Add(Vector2 v, double s)
		{
			return new Vector2(v.X + s, v.Y + s);
		}
		/// <summary>
		/// Adds two vectors and put the result in the third vector.
		/// </summary>
		/// <param name="u">A <see cref="Vector2"/> instance.</param>
		/// <param name="v">A <see cref="Vector2"/> instance</param>
		/// <param name="w">A <see cref="Vector2"/> instance to hold the result.</param>
		public static void Add(Vector2 u, Vector2 v, Vector2 w)
		{
			w.X = u.X + v.X;
			w.Y = u.Y + v.Y;
		}
		/// <summary>
		/// Adds a vector and a scalar and put the result into another vector.
		/// </summary>
		/// <param name="u">A <see cref="Vector2"/> instance.</param>
		/// <param name="s">A scalar.</param>
		/// <param name="v">A <see cref="Vector2"/> instance to hold the result.</param>
		public static void Add(Vector2 u, double s, Vector2 v)
		{
			v.X = u.X + s;
			v.Y = u.Y + s;
		}
		/// <summary>
		/// Subtracts a vector from a vector.
		/// </summary>
		/// <param name="v">A <see cref="Vector2"/> instance.</param>
		/// <param name="w">A <see cref="Vector2"/> instance.</param>
		/// <returns>A new <see cref="Vector2"/> instance containing the difference.</returns>
		/// <remarks>
		///	result[i] = v[i] - w[i].
		/// </remarks>
		public static Vector2 Subtract(Vector2 v, Vector2 w)
		{
			return new Vector2(v.X - w.X, v.Y - w.Y);
		}
		/// <summary>
		/// Subtracts a scalar from a vector.
		/// </summary>
		/// <param name="v">A <see cref="Vector2"/> instance.</param>
		/// <param name="s">A scalar.</param>
		/// <returns>A new <see cref="Vector2"/> instance containing the difference.</returns>
		/// <remarks>
		/// result[i] = v[i] - s
		/// </remarks>
		public static Vector2 Subtract(Vector2 v, double s)
		{
			return new Vector2(v.X - s, v.Y - s);
		}
		/// <summary>
		/// Subtracts a vector from a scalar.
		/// </summary>
		/// <param name="v">A <see cref="Vector2"/> instance.</param>
		/// <param name="s">A scalar.</param>
		/// <returns>A new <see cref="Vector2"/> instance containing the difference.</returns>
		/// <remarks>
		/// result[i] = s - v[i]
		/// </remarks>
		public static Vector2 Subtract(double s, Vector2 v)
		{
			return new Vector2(s - v.X, s - v.Y);
		}
		/// <summary>
		/// Subtracts a vector from a second vector and puts the result into a third vector.
		/// </summary>
		/// <param name="u">A <see cref="Vector2"/> instance.</param>
		/// <param name="v">A <see cref="Vector2"/> instance</param>
		/// <param name="w">A <see cref="Vector2"/> instance to hold the result.</param>
		/// <remarks>
		///	w[i] = v[i] - w[i].
		/// </remarks>
		public static void Subtract(Vector2 u, Vector2 v, Vector2 w)
		{
			w.X = u.X - v.X;
			w.Y = u.Y - v.Y;
		}
		/// <summary>
		/// Subtracts a vector from a scalar and put the result into another vector.
		/// </summary>
		/// <param name="u">A <see cref="Vector2"/> instance.</param>
		/// <param name="s">A scalar.</param>
		/// <param name="v">A <see cref="Vector2"/> instance to hold the result.</param>
		/// <remarks>
		/// v[i] = u[i] - s
		/// </remarks>
		public static void Subtract(Vector2 u, double s, Vector2 v)
		{
			v.X = u.X - s;
			v.Y = u.Y - s;
		}
		/// <summary>
		/// Subtracts a scalar from a vector and put the result into another vector.
		/// </summary>
		/// <param name="u">A <see cref="Vector2"/> instance.</param>
		/// <param name="s">A scalar.</param>
		/// <param name="v">A <see cref="Vector2"/> instance to hold the result.</param>
		/// <remarks>
		/// v[i] = s - u[i]
		/// </remarks>
		public static void Subtract(double s, Vector2 u, Vector2 v)
		{
			v.X = s - u.X;
			v.Y = s - u.Y;
		}
		/// <summary>
		/// Divides a vector by another vector.
		/// </summary>
		/// <param name="u">A <see cref="Vector2"/> instance.</param>
		/// <param name="v">A <see cref="Vector2"/> instance.</param>
		/// <returns>A new <see cref="Vector2"/> containing the quotient.</returns>
		/// <remarks>
		///	result[i] = u[i] / v[i].
		/// </remarks>
		public static Vector2 Divide(Vector2 u, Vector2 v)
		{
			return new Vector2(u.X / v.X, u.Y / v.Y);
		}
		/// <summary>
		/// Divides a vector by a scalar.
		/// </summary>
		/// <param name="v">A <see cref="Vector2"/> instance.</param>
		/// <param name="s">A scalar</param>
		/// <returns>A new <see cref="Vector2"/> containing the quotient.</returns>
		/// <remarks>
		/// result[i] = v[i] / s;
		/// </remarks>
		public static Vector2 Divide(Vector2 v, double s)
		{
			return new Vector2(v.X / s, v.Y / s);
		}
		/// <summary>
		/// Divides a scalar by a vector.
		/// </summary>
		/// <param name="v">A <see cref="Vector2"/> instance.</param>
		/// <param name="s">A scalar</param>
		/// <returns>A new <see cref="Vector2"/> containing the quotient.</returns>
		/// <remarks>
		/// result[i] = s / v[i]
		/// </remarks>
		public static Vector2 Divide(double s, Vector2 v)
		{
			return new Vector2(s / v.X, s/ v.Y);
		}
		/// <summary>
		/// Divides a vector by another vector.
		/// </summary>
		/// <param name="u">A <see cref="Vector2"/> instance.</param>
		/// <param name="v">A <see cref="Vector2"/> instance.</param>
		/// <param name="w">A <see cref="Vector2"/> instance to hold the result.</param>
		/// <remarks>
		/// w[i] = u[i] / v[i]
		/// </remarks>
		public static void Divide(Vector2 u, Vector2 v, Vector2 w)
		{
			w.X = u.X / v.X;
			w.Y = u.Y / v.Y;
		}
		/// <summary>
		/// Divides a vector by a scalar.
		/// </summary>
		/// <param name="u">A <see cref="Vector2"/> instance.</param>
		/// <param name="s">A scalar</param>
		/// <param name="v">A <see cref="Vector2"/> instance to hold the result.</param>
		/// <remarks>
		/// v[i] = u[i] / s
		/// </remarks>
		public static void Divide(Vector2 u, double s, Vector2 v)
		{
			v.X = u.X / s;
			v.Y = u.Y / s;
		}
		/// <summary>
		/// Divides a scalar by a vector.
		/// </summary>
		/// <param name="u">A <see cref="Vector2"/> instance.</param>
		/// <param name="s">A scalar</param>
		/// <param name="v">A <see cref="Vector2"/> instance to hold the result.</param>
		/// <remarks>
		/// v[i] = s / u[i]
		/// </remarks>
		public static void Divide(double s, Vector2 u, Vector2 v)
		{
			v.X = s / u.X;
			v.Y = s / u.Y;
		}
		/// <summary>
		/// Multiplies a vector by a scalar.
		/// </summary>
		/// <param name="u">A <see cref="Vector2"/> instance.</param>
		/// <param name="s">A scalar.</param>
		/// <returns>A new <see cref="Vector2"/> containing the result.</returns>
		public static Vector2 Multiply(Vector2 u, double s)
		{
			return new Vector2(u.X * s, u.Y * s);
		}
		/// <summary>
		/// Multiplies a vector by a scalar and put the result in another vector.
		/// </summary>
		/// <param name="u">A <see cref="Vector2"/> instance.</param>
		/// <param name="s">A scalar.</param>
		/// <param name="v">A <see cref="Vector2"/> instance to hold the result.</param>
		public static void Multiply(Vector2 u, double s, Vector2 v)
		{
			v.X = u.X * s;
			v.Y = u.Y * s;
		}
		/// <summary>
		/// Calculates the dot product of two vectors.
		/// </summary>
		/// <param name="u">A <see cref="Vector2"/> instance.</param>
		/// <param name="v">A <see cref="Vector2"/> instance.</param>
		/// <returns>The dot product value.</returns>
		public static double DotProduct(Vector2 u, Vector2 v)
		{
			return (u.X * v.X) + (u.Y * v.Y);
		}
		/// <summary>
		/// Negates a vector.
		/// </summary>
		/// <param name="v">A <see cref="Vector2"/> instance.</param>
		/// <returns>A new <see cref="Vector2"/> instance containing the negated values.</returns>
		public static Vector2 Negate(Vector2 v)
		{
			return new Vector2(-v.X, -v.Y);
		}
		/// <summary>
		/// Tests whether two vectors are approximately equal using default tolerance value.
		/// </summary>
		/// <param name="v">A <see cref="Vector2"/> instance.</param>
		/// <param name="u">A <see cref="Vector2"/> instance.</param>
		/// <returns>True if the two vectors are approximately equal; otherwise, False.</returns>
		public static bool ApproxEqual(Vector2 v, Vector2 u)
		{
            return ApproxEqual(v, u, MathLib.Epsilon);
		}
		/// <summary>
		/// Tests whether two vectors are approximately equal given a tolerance value.
		/// </summary>
		/// <param name="v">A <see cref="Vector2"/> instance.</param>
		/// <param name="u">A <see cref="Vector2"/> instance.</param>
		/// <param name="tolerance">The tolerance value used to test approximate equality.</param>
		/// <returns>True if the two vectors are approximately equal; otherwise, False.</returns>
		public static bool ApproxEqual(Vector2 v, Vector2 u, double tolerance)
		{
			return
				(
				(System.Math.Abs(v.X - u.X) <= tolerance) &&
				(System.Math.Abs(v.Y - u.Y) <= tolerance)
				);
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

		}
		/// <summary>
		/// Returns the length of the vector.
		/// </summary>
		/// <returns>The length of the vector. (Sqrt(X*X + Y*Y))</returns>
		public double GetLength()
		{
			return System.Math.Sqrt(_x*_x + _y*_y);
		}
		/// <summary>
		/// Returns the squared length of the vector.
		/// </summary>
		/// <returns>The squared length of the vector. (X*X + Y*Y)</returns>
		public double GetLengthSquared()
		{
			return (_x*_x + _y*_y);
		}
		#endregion

		#region Overrides
		/// <summary>
		/// Returns the hashcode for this instance.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			return _x.GetHashCode() ^ _y.GetHashCode();
		}
		/// <summary>
		/// Returns a value indicating whether this instance is equal to
		/// the specified object.
		/// </summary>
		/// <param name="obj">An object to compare to this instance.</param>
		/// <returns>True if <paramref name="obj"/> is a <see cref="Vector2"/> and has the same values as this instance; otherwise, False.</returns>
		public override bool Equals(object obj)
		{
			if (obj is Vector2)
			{
				Vector2 v = (Vector2)obj;
				return (_x == v.X) && (_y == v.Y);
			}
			return false;
		}

		/// <summary>
		/// Returns a string representation of this object.
		/// </summary>
		/// <returns>A string representation of this object.</returns>
		public override string ToString()
		{
			return string.Format("({0}, {1})", _x, _y);
		}
		#endregion
		
		#region Comparison Operators
		/// <summary>
		/// Tests whether two specified vectors are equal.
		/// </summary>
		/// <param name="u">The left-hand vector.</param>
		/// <param name="v">The right-hand vector.</param>
		/// <returns>True if the two vectors are equal; otherwise, False.</returns>
		public static bool operator==(Vector2 u, Vector2 v)
		{
			if (Object.Equals(u, null))
			{
				return Object.Equals(v, null);
			}

			if (Object.Equals(v, null))
			{
				return Object.Equals(u, null);
			}

			return (u.X == v.X) && (u.Y == v.Y);
		}
		/// <summary>
		/// Tests whether two specified vectors are not equal.
		/// </summary>
		/// <param name="u">The left-hand vector.</param>
		/// <param name="v">The right-hand vector.</param>
		/// <returns>True if the two vectors are not equal; otherwise, False.</returns>
		public static bool operator!=(Vector2 u, Vector2 v)
		{
			if (Object.Equals(u, null))
			{
				return !Object.Equals(v, null);
			}

			if (Object.Equals(v, null))
			{
				return !Object.Equals(u, null);
			}

			return !((u.X == v.X) && (u.Y == v.Y));
		}

		/// <summary>
		/// Tests if a vector's components are greater than another vector's components.
		/// </summary>
		/// <param name="u">The left-hand vector.</param>
		/// <param name="v">The right-hand vector.</param>
		/// <returns>True if the left-hand vector's components are greater than the right-hand vector's component; otherwise, False.</returns>
		public static bool operator>(Vector2 u, Vector2 v)
		{
			return (
				(u._x > v._x) && 
				(u._y > v._y));
		}
		/// <summary>
		/// Tests if a vector's components are smaller than another vector's components.
		/// </summary>
		/// <param name="u">The left-hand vector.</param>
		/// <param name="v">The right-hand vector.</param>
		/// <returns>True if the left-hand vector's components are smaller than the right-hand vector's component; otherwise, False.</returns>
		public static bool operator<(Vector2 u, Vector2 v)
		{
			return (
				(u._x < v._x) && 
				(u._y < v._y));
		}
		/// <summary>
		/// Tests if a vector's components are greater or equal than another vector's components.
		/// </summary>
		/// <param name="u">The left-hand vector.</param>
		/// <param name="v">The right-hand vector.</param>
		/// <returns>True if the left-hand vector's components are greater or equal than the right-hand vector's component; otherwise, False.</returns>
		public static bool operator>=(Vector2 u, Vector2 v)
		{
			return (
				(u._x >= v._x) && 
				(u._y >= v._y));
		}
		/// <summary>
		/// Tests if a vector's components are smaller or equal than another vector's components.
		/// </summary>
		/// <param name="u">The left-hand vector.</param>
		/// <param name="v">The right-hand vector.</param>
		/// <returns>True if the left-hand vector's components are smaller or equal than the right-hand vector's component; otherwise, False.</returns>
		public static bool operator<=(Vector2 u, Vector2 v)
		{
			return (
				(u._x <= v._x) && 
				(u._y <= v._y));
		}
		#endregion

		#region Unary Operators
		/// <summary>
		/// Negates the values of the vector.
		/// </summary>
		/// <param name="v">A <see cref="Vector2"/> instance.</param>
		/// <returns>A new <see cref="Vector2"/> instance containing the negated values.</returns>
		public static Vector2 operator-(Vector2 v)
		{
			return Vector2.Negate(v);
		}
		#endregion

		#region Binary Operators
		/// <summary>
		/// Adds two vectors.
		/// </summary>
		/// <param name="u">A <see cref="Vector2"/> instance.</param>
		/// <param name="v">A <see cref="Vector2"/> instance.</param>
		/// <returns>A new <see cref="Vector2"/> instance containing the sum.</returns>
		public static Vector2 operator+(Vector2 u, Vector2 v)
		{
			return Vector2.Add(u,v);
		}
		/// <summary>
		/// Adds a vector and a scalar.
		/// </summary>
		/// <param name="v">A <see cref="Vector2"/> instance.</param>
		/// <param name="s">A scalar.</param>
		/// <returns>A new <see cref="Vector2"/> instance containing the sum.</returns>
		public static Vector2 operator+(Vector2 v, double s)
		{
			return Vector2.Add(v,s);
		}
		/// <summary>
		/// Adds a vector and a scalar.
		/// </summary>
		/// <param name="v">A <see cref="Vector2"/> instance.</param>
		/// <param name="s">A scalar.</param>
		/// <returns>A new <see cref="Vector2"/> instance containing the sum.</returns>
		public static Vector2 operator+(double s, Vector2 v)
		{
			return Vector2.Add(v,s);
		}
		/// <summary>
		/// Subtracts a vector from a vector.
		/// </summary>
		/// <param name="u">A <see cref="Vector2"/> instance.</param>
		/// <param name="v">A <see cref="Vector2"/> instance.</param>
		/// <returns>A new <see cref="Vector2"/> instance containing the difference.</returns>
		/// <remarks>
		///	result[i] = v[i] - w[i].
		/// </remarks>
		public static Vector2 operator-(Vector2 u, Vector2 v)
		{
			return Vector2.Subtract(u,v);
		}
		/// <summary>
		/// Subtracts a scalar from a vector.
		/// </summary>
		/// <param name="v">A <see cref="Vector2"/> instance.</param>
		/// <param name="s">A scalar.</param>
		/// <returns>A new <see cref="Vector2"/> instance containing the difference.</returns>
		/// <remarks>
		/// result[i] = v[i] - s
		/// </remarks>
		public static Vector2 operator-(Vector2 v, double s)
		{
			return Vector2.Subtract(v, s);
		}
		/// <summary>
		/// Subtracts a vector from a scalar.
		/// </summary>
		/// <param name="v">A <see cref="Vector2"/> instance.</param>
		/// <param name="s">A scalar.</param>
		/// <returns>A new <see cref="Vector2"/> instance containing the difference.</returns>
		/// <remarks>
		/// result[i] = s - v[i]
		/// </remarks>
		public static Vector2 operator-(double s, Vector2 v)
		{
			return Vector2.Subtract(s, v);
		}

		/// <summary>
		/// Multiplies a vector by a scalar.
		/// </summary>
		/// <param name="v">A <see cref="Vector2"/> instance.</param>
		/// <param name="s">A scalar.</param>
		/// <returns>A new <see cref="Vector2"/> containing the result.</returns>
		public static Vector2 operator*(Vector2 v, double s)
		{
			return Vector2.Multiply(v,s);
		}
		/// <summary>
		/// Multiplies a vector by a scalar.
		/// </summary>
		/// <param name="v">A <see cref="Vector2"/> instance.</param>
		/// <param name="s">A scalar.</param>
		/// <returns>A new <see cref="Vector2"/> containing the result.</returns>
		public static Vector2 operator*(double s, Vector2 v)
		{
			return Vector2.Multiply(v,s);
		}
		/// <summary>
		/// Divides a vector by a scalar.
		/// </summary>
		/// <param name="v">A <see cref="Vector2"/> instance.</param>
		/// <param name="s">A scalar</param>
		/// <returns>A new <see cref="Vector2"/> containing the quotient.</returns>
		/// <remarks>
		/// result[i] = v[i] / s;
		/// </remarks>
		public static Vector2 operator/(Vector2 v, double s)
		{
			return Vector2.Divide(v,s);
		}
		/// <summary>
		/// Divides a scalar by a vector.
		/// </summary>
		/// <param name="v">A <see cref="Vector2"/> instance.</param>
		/// <param name="s">A scalar</param>
		/// <returns>A new <see cref="Vector2"/> containing the quotient.</returns>
		/// <remarks>
		/// result[i] = s / v[i]
		/// </remarks>
		public static Vector2 operator/(double s, Vector2 v)
		{
			return Vector2.Divide(s,v);
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
		/// <param name="v">A <see cref="Vector2"/> instance.</param>
		/// <returns>An array of double-precision floating point values.</returns>
		public static explicit operator double[](Vector2 v)
		{
			double[] array = new double[2];
			array[0] = v.X;
			array[1] = v.Y;
			return array;
		}
		#endregion

	}

}
