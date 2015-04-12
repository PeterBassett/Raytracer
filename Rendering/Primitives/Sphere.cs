using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.Primitives
{
    using Vector = Vector3;
    using Real = System.Double;

    class Sphere : Traceable
    {
        private Real m_Radius = 0.0f;
        public Real Radius
        {
            get
            {
                return m_Radius;
            }
            set
            {
                m_Radius = value;
            }
        }
        
        public override IntersectionInfo Intersect(Ray ray)
        {
            Real distance = 0.0f;
            Traceable prim = null;

            HitResult retval = HitResult.MISS;

            Real fA = 0.0f;
            Real fB = 0.0f;
            Real fC = 0.0f;
            Real fD = 0.0f;
            Real fDist1 = 0.0f;
            Real fDist2 = 0.0f;
            Real fRoot = 0.0f;
            Real fDistance = 0.0f;
	        Vector cTmp, cStart, cDir;

	        cStart = ray.Pos;
	        cDir = ray.Dir;

	        cTmp = cStart - Pos;
	        fB = 2.0f * ( Vector.DotProduct(cDir, cTmp ) );  
	        fC = Vector.DotProduct(cTmp, cTmp) - ( Radius *  Radius );
	        fA = 1.0f;

	        fD = ( fB * fB ) - 4.0f * fA * fC;

	        if( fD < 0.0f )
                return new IntersectionInfo(HitResult.MISS);

	        if( fD >= 0.0f )
	        {
		        fRoot = (Real)Math.Sqrt( fD );

		        fDist1 = ( -fB - fRoot ) * ( 0.5f * fA );
		        fDist2 = ( -fB + fRoot ) * ( 0.5f * fA );

		        if( (fDist1 > 1.0f ) || ( fDist2 > 1.0f ))
		        {
                    if (fDist1 > 1.0f)
                    {
                        fDistance = fDist1;
                        prim = this;
                        retval = HitResult.HIT;
                    }

			        if(fDist2 > 1.0f)
			        {
				        if( fDist2 < fDist1 || fDist1 < 1.0f) 
                        {
					        fDistance = fDist2;
                            prim = this;
                            retval = HitResult.INPRIM;
                        }
			        }

                    distance = fDistance;

                    if(retval != HitResult.MISS)
                    {
                        var hitPoint = ray.Pos + (ray.Dir * distance);
                        var normal = GetNormal(hitPoint);
                        // Normal needs to be flipped if this is a refractive ray.
                        //if (Vector3.DotProduct(ray.Dir, normal) > 0)
                         //   normal = -normal;

                        return new IntersectionInfo(retval, this, fDistance, hitPoint, hitPoint, normal);
                    }
                    else
			            return new IntersectionInfo(HitResult.MISS);
		        }
	        }

            return new IntersectionInfo(HitResult.MISS);       
        }

        private Vector GetNormal(Vector vPoint)
        {
            Vector vNorm;
	        vNorm = vPoint - Pos;
	        vNorm.Normalize();
	        return vNorm;
        }

        public override bool Intersect(AABB aabb)
        {
            // Get the center of the sphere relative to the center of the box
            Vector sphereCenterRelBox = this.Pos - aabb.Center;
            // Point on surface of box that is closest to the center of the sphere
            Vector boxPoint = new Vector();

            // Check sphere center against box along the X axis alone. 
            // If the sphere is off past the left edge of the box, 
            // then the left edge is closest to the sphere. 
            // Similar if it's past the right edge. If it's between 
            // the left and right edges, then the sphere's own X 
            // is closest, because that makes the X distance 0, 
            // and you can't get much closer than that :)

            if (sphereCenterRelBox.X < -aabb.Width / 2.0f)
                boxPoint.X = -aabb.Width / 2.0f;
            else if (sphereCenterRelBox.X > aabb.Width / 2.0f)
                boxPoint.X = aabb.Width / 2.0f;
            else
                boxPoint.X = sphereCenterRelBox.X;

            // ...same for Y axis
            if (sphereCenterRelBox.Y < -aabb.Height / 2.0f)
                boxPoint.Y = -aabb.Height / 2.0f;
            else if (sphereCenterRelBox.Y > aabb.Height / 2.0f)
                boxPoint.Y = aabb.Height / 2.0f;
            else
                boxPoint.Y = sphereCenterRelBox.Y;

            // ... same for Z axis
            if (sphereCenterRelBox.Z < -aabb.Depth / 2.0f)
                boxPoint.Z = -aabb.Depth / 2.0f;
            else if (sphereCenterRelBox.Z > aabb.Depth / 2.0f)
                boxPoint.Z = aabb.Depth / 2.0f;
            else
                boxPoint.Z = sphereCenterRelBox.Z;

            // Now we have the closest point on the box, so get the distance from 
            // that to the sphere center, and see if it's less than the radius

            Vector dist = sphereCenterRelBox - boxPoint;

            return (dist.X * dist.X + dist.Y * dist.Y + dist.Z * dist.Z < this.Radius * this.Radius);
        }

        public override AABB GetAABB()
        {
            return new AABB()
            {
                Min = new Vector(this.Pos.X - Radius, this.Pos.Y - Radius, this.Pos.Z - Radius),
                Max = new Vector(this.Pos.X + Radius, this.Pos.Y + Radius, this.Pos.Z + Radius)
            };
        }

        public override bool Contains(Vector point)
        {
            double radius = this.Radius + MathLib.Epsilon;
            return (point - this.Pos).GetLengthSquared() <= (radius * radius);
        }
    }
}
