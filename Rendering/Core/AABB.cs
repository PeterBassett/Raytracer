using System;
using Raytracer.MathTypes;

namespace Raytracer.Rendering.Core
{       
    struct AABB
    {
        public Vector3 Min;
        public Vector3 Max;        
        public static readonly AABB Empty = new AABB(true);
        private readonly bool _isEmpty;

        private AABB(bool isEmpty)
        {
            Min = new Vector3();
            Max = new Vector3();
            _isEmpty = isEmpty;
        }

        public AABB(Vector3 min, Vector3 max)
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

        public Vector3 Center
        {
            get
            {
                return new Vector3
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

        public void InflateToEncapsulate(AABB other)
        {               
            if (other.Min.X < Min.X)
                Min.X = other.Min.X;

            if (other.Min.Y < Min.Y)
                Min.Y = other.Min.Y;

            if (other.Min.Z < Min.Z)
                Min.Z = other.Min.Z;

            if (other.Max.X > Max.X)
                Max.X = other.Max.X;

            if (other.Max.Y > Max.Y)
                Max.Y = other.Max.Y;

            if (other.Max.Z > Max.Z)
                Max.Z = other.Max.Z;
        }

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

        public bool Contains(Vector3 point)
        {
            var T = point - Center;

            return (Math.Abs(T.X) <= Width &&
                   Math.Abs(T.Y) <=  Height &&
                   Math.Abs(T.Z) <=  Depth);
        }

        public bool IsEmpty { get { return _isEmpty; } }
    }
}
