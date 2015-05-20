using System;

namespace Raytracer.MathTypes
{
	public struct Quaternion
	{
		public double X;
		public double Y;
		public double Z;
		public double W;

		public Quaternion(double x, double y, double z, double w)
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		public Quaternion(Vector axis, double angle)
		{
            axis = axis.Normalize();

			var halfAngle = angle * 0.5f;
			var sin = Math.Sin(halfAngle);
			var cos = Math.Cos(halfAngle);

			X = axis.X * sin;
			Y = axis.Y * sin;
			Z = axis.Z * sin;

			W = cos;
		}

        public double LengthSquared
        {
            get
            {
                return (X * X) + (Y * Y) + (Z * Z) + (W * W);
            }
        }
        
        public double Length
        {
            get
            {
                return Math.Sqrt(LengthSquared);
            }
        }

		public void Normalize()
		{
			double inverseLength = 1.0 / Length;

			X *= inverseLength;
			Y *= inverseLength;
			Z *= inverseLength;
			W *= inverseLength;
		}

		public bool IsIdentity
		{
			get { return X == 0.0f && Y == 0.0f && Z == 0.0f && W == 1.0f; }
		}

		public double Angle
		{
			get
			{
				if (IsIdentity)
					return 0.0f;

				double imaginaryLengthSquared = Math.Sqrt((X * X) + (Y * Y) + (Z * Z));

				double real = W;

				if (imaginaryLengthSquared > double.MaxValue)
				{
					double maxAxis = Math.Max(Math.Abs(X), Math.Max(Math.Abs(Y), Math.Abs(Z)));

					double newX = X / maxAxis;
					double newY = Y / maxAxis;
					double newZ = Z / maxAxis;

					imaginaryLengthSquared = Math.Sqrt((newX * newX) + (newY * newY) + (newZ * newZ));

					real = W / maxAxis;
				}

				return Math.Atan2(imaginaryLengthSquared, real);
			}
		}

		public Vector Axis
		{
			get
			{
				if (IsIdentity)
					return new Vector(0.0f, 1.0f, 0.0f);
				
                return new Vector(X, Y, Z).Normalize();
			}
		}

		public static Quaternion operator *(Quaternion a, Quaternion b)
		{
            double aX = a.X;
			double aY = a.Y;
			double aZ = a.Z;
			double aW = a.W;

			double bX = b.X;
			double bY = b.Y;
			double bZ = b.Z;
			double bW = b.W;

			var x = ((aX * bW) + (bX * aW)) + (aY * bZ) - (aZ * bY);
			var y = ((aY * bW) + (bY * aW)) + (aZ * bX) - (aX * bZ);
			var z = ((aZ * bW) + (bZ * aW)) + (aX * bY) - (aY * bX);
			var w = (aW * bW) - ((aX * bX) + (aY * bY)) + (aZ * bZ);

			return new Quaternion(x, y, z, w);
		}

		public static Quaternion operator *(Quaternion q, double s)
		{
			return new Quaternion(q.X * s,
			                      q.Y * s,
			                      q.Z * s,
			                      q.W * s);
		}

		public static bool operator ==(Quaternion left, Quaternion right)
		{
			return left.X == right.X && 
                    left.Y == right.Y && 
                    left.Z == right.Z && 
                    left.W == right.W;
		}

		public static bool operator !=(Quaternion left, Quaternion right)
		{
			return !(left == right);
		}

		public static readonly Quaternion Identity = new Quaternion(0f, 0f, 0f, 1f);

		public static Quaternion CreateFromYawPitchRoll(double yaw, double pitch, double roll)
		{
            double halfRoll = roll * 0.5f;
			double sinHalfRoll = Math.Sin(halfRoll);
			double cosHalfRoll = Math.Cos(halfRoll);

			double halfPitch = pitch * 0.5f;
			double sinHalfPitch = Math.Sin(halfPitch);
			double cosHalfPitch = Math.Cos(halfPitch);

			double halfYaw = yaw * 0.5f;
			double sinHalfYaw = Math.Sin(halfYaw);
			double cosHalfYaw = Math.Cos(halfYaw);

			return new Quaternion(((cosHalfYaw * sinHalfPitch) * cosHalfRoll) + ((sinHalfYaw * cosHalfPitch) * sinHalfRoll),
                                  ((sinHalfYaw * cosHalfPitch) * cosHalfRoll) - ((cosHalfYaw * sinHalfPitch) * sinHalfRoll),
                                  ((cosHalfYaw * cosHalfPitch) * sinHalfRoll) - ((sinHalfYaw * sinHalfPitch) * cosHalfRoll),
                                  ((cosHalfYaw * cosHalfPitch) * cosHalfRoll) + ((sinHalfYaw * sinHalfPitch) * sinHalfRoll));
		}

		public static Quaternion CreateFromAxisAngle(Vector axis, double angle)
		{			
            double halfAngle = angle * 0.5f;
			double sin = Math.Sin(halfAngle);
			double cos = Math.Cos(halfAngle);
			
            return new Quaternion(axis.X * sin,
			                      axis.Y * sin,
			                      axis.Z * sin,
			                      cos);
		}

		public static Quaternion CreateFromRotationMatrix(Matrix matrix)
		{
			double trace = (matrix.M11 + matrix.M22) + matrix.M33;

			Quaternion quaternion = new Quaternion();
			if (trace > 0f)
			{
				double s = Math.Sqrt(trace + 1f);
				
                quaternion.W = s * 0.5f;
				s = 0.5f / s;
				quaternion.X = (matrix.M23 - matrix.M32) * s;
				quaternion.Y = (matrix.M31 - matrix.M13) * s;
				quaternion.Z = (matrix.M12 - matrix.M21) * s;

				return quaternion;
			}
			if ((matrix.M11 >= matrix.M22) && (matrix.M11 >= matrix.M33))
			{
				double s = Math.Sqrt(((1f + matrix.M11) - matrix.M22) - matrix.M33);
				double num4 = 0.5f / s;
				quaternion.X = 0.5f * s;
				quaternion.Y = (matrix.M12 + matrix.M21) * num4;
				quaternion.Z = (matrix.M13 + matrix.M31) * num4;
				quaternion.W = (matrix.M23 - matrix.M32) * num4;
				return quaternion;
			}
			if (matrix.M22 > matrix.M33)
			{
				double s = Math.Sqrt(((1f + matrix.M22) - matrix.M11) - matrix.M33);
				double num3 = 0.5f / s;
				quaternion.X = (matrix.M21 + matrix.M12) * num3;
				quaternion.Y = 0.5f * s;
				quaternion.Z = (matrix.M32 + matrix.M23) * num3;
				quaternion.W = (matrix.M31 - matrix.M13) * num3;
				return quaternion;
			}
			
            double num5 = Math.Sqrt(((1f + matrix.M33) - matrix.M11) - matrix.M22);
			double num2 = 0.5f / num5;

			quaternion.X = (matrix.M31 + matrix.M13) * num2;
			quaternion.Y = (matrix.M32 + matrix.M23) * num2;
			quaternion.Z = 0.5f * num5;
			quaternion.W = (matrix.M12 - matrix.M21) * num2;

			return quaternion;
		}

		public static bool IsNan(Quaternion q)
		{
			return  double.IsNaN(q.X) || 
                    double.IsNaN(q.Y) || 
                    double.IsNaN(q.Z) || 
                    double.IsNaN(q.W);
		}

        public override int GetHashCode()
        {
            return W.GetHashCode() ^ X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Quaternion)
            {
                Quaternion v = (Quaternion)obj;
                return (W == v.W) && (X == v.X) && (Y == v.Y) && (Z == v.Z);
            }
            return false;
        }

        public Vector ToYawPitchRoll()
        {
            return Quaternion.ToYawPitchRoll(this);
        }

        public static Vector ToYawPitchRoll(Quaternion q)
        {
            var roll =  Math.Atan2(2.0 * (q.X * q.Y + q.W * q.Z), q.W * q.W + q.X * q.X - q.Y * q.Y - q.Z * q.Z);
            var pitch = Math.Atan2(2.0 * (q.Y * q.Z + q.W * q.X), q.W * q.W - q.X * q.X - q.Y * q.Y + q.Z * q.Z);
            var yaw =   Math.Asin(-2.0 * (q.X * q.Z - q.W * q.Y));

            return new Vector(pitch, yaw, roll);
        }
	}
}