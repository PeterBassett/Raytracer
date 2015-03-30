using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.Extensions;
using Raytracer.MathTypes;

namespace Raytracer.Rendering
{
    using Vector = Vector3;
    using Real = System.Double;
    struct AABB
    {
        public Vector Min;
        public Vector Max;        
        public static readonly AABB Empty = new AABB(true);
        private bool isEmpty;

        private AABB(bool isEmpty)
        {
            Min = new Vector();
            Max = new Vector();
            this.isEmpty = isEmpty;
        }

        public AABB(AABB other) : this(other.Min, other.Max)
        {
            
        }

        public AABB(Vector min, Vector max)
        {
            isEmpty = false;
            Real temp = 0;
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
            
           /* center = new MathTypes.Vector3F()
            {
                X = Min.X + ((Max.X - Min.X) / 2),
                Y = Min.Y + ((Max.Y - Min.Y) / 2),
                Z = Min.Z + ((Max.Z - Min.Z) / 2)
            };
            halfsize = new MathTypes.Vector3F()
            {
                X = Width / 2.0f,
                Y = Height / 2.0f,
                Z = Depth / 2.0f
            };*/
        }

        public Real Width
        {
            get
            {
                return Max.X - Min.X;
            }
        }

        public Real Height
        {
            get
            {
                return Max.Y - Min.Y;
            }
        }

        public Real Depth
        {
            get
            {
                return Max.Z - Min.Z;
            }
        }

        public Vector Center
        {
            get
            {
                return new Vector()
                {
                    X = Min.X + ((Max.X - Min.X) / 2),
                    Y = Min.Y + ((Max.Y - Min.Y) / 2),
                    Z = Min.Z + ((Max.Z - Min.Z) / 2)
                };                
            }
        }

        public Vector HalfSize
        {
            get
            {
                return new Vector()
                {
                    X = Width / 2.0f,
                    Y = Height / 2.0f,
                    Z = Depth / 2.0f
                };
            }
        }

        public int GetLongestAxis()
        {
            Vector diff = Max - Min;

            if (diff.X >= diff.Y && diff.X >= diff.Z) 
                return 0;

            if (diff.Y >= diff.X && diff.Y >= diff.Z) 
                return 1;

            return 2;
        }

        public void InflateToEncapsulate(AABB other)
        {
            /*
            Min.X = Math.Min(Min.X, other.Min.X);
            Min.Y = Math.Min(Min.Y, other.Min.Y);
            Min.Z = Math.Min(Min.Z, other.Min.Z);

            Max.X = Math.Max(Max.X, other.Min.X);
            Max.Y = Math.Max(Max.Y, other.Min.Y);
            Max.Z = Math.Max(Max.Z, other.Min.Z);
            */                      

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

        /// <summary>
        /// Tests whether two specified vectors are equal.
        /// </summary>
        /// <param name="u">The left-hand vector.</param>
        /// <param name="v">The right-hand vector.</param>
        /// <returns>True if the two vectors are equal; otherwise, False.</returns>
        public static bool operator ==(AABB u, AABB v)
        {
            if (Object.Equals(u, null))
            {
                return Object.Equals(v, null);
            }

            if (Object.Equals(v, null))
            {
                return Object.Equals(u, null);
            }

            return (u.Min.X == v.Min.X) && (u.Min.Y == v.Min.Y) && (u.Min.Z == v.Min.Z) && (u.Max.X == v.Max.X) && (u.Max.Y == v.Max.Y) && (u.Max.Z == v.Max.Z);
        }

        /// <summary>
        /// Tests whether two specified vectors are not equal.
        /// </summary>
        /// <param name="u">The left-hand vector.</param>
        /// <param name="v">The right-hand vector.</param>
        /// <returns>True if the two vectors are not equal; otherwise, False.</returns>
        public static bool operator !=(AABB u, AABB v)
        {
            if (Object.Equals(u, null))
            {
                return !Object.Equals(v, null);
            }

            if (Object.Equals(v, null))
            {
                return !Object.Equals(u, null);
            }

            return !(u.Min.X == v.Min.X) && (u.Min.Y == v.Min.Y) && (u.Min.Z == v.Min.Z) && (u.Max.X == v.Max.X) && (u.Max.Y == v.Max.Y) && (u.Max.Z == v.Max.Z);
        }

        public bool Intersect(Ray ray)
        {
            Real t = 0;
            return Raytracer.Rendering.Primitives.IntersectionCode3.pluecker_cls_cff(ray, this);//, ref t);	        
        }

        public bool Intersect(AABB b)
        {
            Vector T = b.Center - Center;//vector from A to B

            return (Math.Abs(T.X) <= (this.Width + b.Width) &&
                   Math.Abs(T.Y) <= (this.Height + b.Height) &&
                   Math.Abs(T.Z) <= (this.Depth + b.Depth)); 
        }

        public bool IsEmpty { get { return isEmpty; } }
    }
}
