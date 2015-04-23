using System;
using System.Diagnostics.Contracts;
using Raytracer.MathTypes;

namespace Raytracer.Rendering.Core
{       
    struct AABB
    {
        public Point3 Min;
        public Point3 Max;        
        public static readonly AABB Empty = new AABB(true);
        private readonly bool _isEmpty;

        private AABB(bool isEmpty)
        {
            Min = new Point3();
            Max = new Point3();
            _isEmpty = isEmpty;
        }

        public AABB(Point3 min, Point3 max)
        {
            _isEmpty = false;
            double temp;
            if (min.X > max.X)
            { 
                temp = min.X;
                min.X = max.X;
                max.X = temp;
            }

            if (min.Y > max.Y)
            {
                temp = min.Y;
                min.Y = max.Y;
                max.Y = temp;
            }

            if (min.Z > max.Z)
            {
                temp = min.Z;
                min.Z = max.Z;
                max.Z = temp;
            }

            Min = min;
            Max = max;
        }

        public double Width
        {
            get
            {
                return Max.X - Min.X;
            }
        }

        public double Height
        {
            get
            {
                return Max.Y - Min.Y;
            }
        }

        public double Depth
        {
            get
            {
                return Max.Z - Min.Z;
            }
        }

        public Point3 Center
        {
            get
            {
                return new Point3
                {
                    X = Min.X + ((Max.X - Min.X) / 2),
                    Y = Min.Y + ((Max.Y - Min.Y) / 2),
                    Z = Min.Z + ((Max.Z - Min.Z) / 2)
                };                
            }
        }

        public Vector3 HalfSize
        {
            get
            {
                return new Vector3
                {
                    X = Width / 2.0f,
                    Y = Height / 2.0f,
                    Z = Depth / 2.0f
                };
            }
        }

        public AABB InflateToEncapsulate(AABB other)
        {
            var min = new Point3(Min);
            var max = new Point3(Max);

            if (other.Min.X < min.X)
                min.X = other.Min.X;

            if (other.Min.Y < min.Y)
                min.Y = other.Min.Y;

            if (other.Min.Z < min.Z)
                min.Z = other.Min.Z;

            if (other.Max.X > max.X)
                max.X = other.Max.X;

            if (other.Max.Y > max.Y)
                max.Y = other.Max.Y;

            if (other.Max.Z > max.Z)
                max.Z = other.Max.Z;

            return new AABB(min, max);
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <param name="obj">Another object to compare to. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            return obj is AABB && this == (AABB) obj;
        }

        public override int GetHashCode()
        {
            return Min.GetHashCode() ^ Max.GetHashCode() ^ _isEmpty.GetHashCode();
        }

        /// <summary>
        /// Tests whether two specified vectors are equal.
        /// </summary>
        /// <param name="u">The left-hand vector.</param>
        /// <param name="v">The right-hand vector.</param>
        /// <returns>True if the two vectors are equal; otherwise, False.</returns>
        public static bool operator ==(AABB u, AABB v)
        {
            return  (Math.Abs(u.Min.X - v.Min.X) < MathLib.Epsilon) && 
                    (Math.Abs(u.Min.Y - v.Min.Y) < MathLib.Epsilon) &&
                    (Math.Abs(u.Min.Z - v.Min.Z) < MathLib.Epsilon) &&
                    (Math.Abs(u.Max.X - v.Max.X) < MathLib.Epsilon) &&
                    (Math.Abs(u.Max.Y - v.Max.Y) < MathLib.Epsilon) &&
                    (Math.Abs(u.Max.Z - v.Max.Z) < MathLib.Epsilon);
        }

        /// <summary>
        /// Tests whether two specified vectors are not equal.
        /// </summary>
        /// <param name="u">The left-hand vector.</param>
        /// <param name="v">The right-hand vector.</param>
        /// <returns>True if the two vectors are not equal; otherwise, False.</returns>
        public static bool operator !=(AABB u, AABB v)
        {
            return !(u == v);
        }

        public bool Intersect(Ray ray)
        {
            return Primitives.IntersectionCode3.pluecker_cls_cff(ray, this);//, ref t);	        
        }

        public bool Intersect(AABB b)
        {
            var T = b.Center - Center;//vector from A to B

            return (Math.Abs(T.X) <= (Width + b.Width) &&
                   Math.Abs(T.Y) <=  (Height + b.Height) &&
                   Math.Abs(T.Z) <=  (Depth + b.Depth)); 
        }

        public bool Contains(Point3 point)
        {
            var T = point - Center;

            return (Math.Abs(T.X) <= Width &&
                   Math.Abs(T.Y) <=  Height &&
                   Math.Abs(T.Z) <=  Depth);
        }

        public bool IsEmpty { get { return _isEmpty; } }
    }
}
