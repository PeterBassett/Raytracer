using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.Primitives
{
    
    

    class Sphere : Traceable
    {
        private double m_Radius = 0.0f;
        public double Radius
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
            var retval = HitResult.MISS;

            double fDistance = 0.0f;

            Vector3 relativePosition = ray.Pos - Pos;
            double fB = 2.0f * (Vector3.DotProduct(ray.Dir, relativePosition));  
	        double fC = Vector3.DotProduct(relativePosition, relativePosition) - ( Radius *  Radius );
	        double fA = 1.0f;

	        double fD = ( fB * fB ) - 4.0f * fA * fC;

	        if( fD < 0.0f )
                return new IntersectionInfo(HitResult.MISS);

	        if( fD >= 0.0f )
	        {
		        double fRoot = (double)Math.Sqrt( fD );

		        double fDist1 = ( -fB - fRoot ) * ( 0.5f * fA );
		        double fDist2 = ( -fB + fRoot ) * ( 0.5f * fA );

		        if( (fDist1 > 1.0f ) || ( fDist2 > 1.0f ))
		        {
                    if (fDist1 > 1.0f)
                    {
                        fDistance = fDist1;
                        retval = HitResult.HIT;
                    }

			        if(fDist2 > 1.0f)
			        {
				        if( fDist2 < fDist1 || fDist1 < 1.0f) 
                        {
					        fDistance = fDist2;
                            retval = HitResult.INPRIM;
                        }
			        }

		            if(retval != HitResult.MISS)
                    {
                        var hitPoint = ray.Pos + (ray.Dir * fDistance);
                        var normal = GetNormal(hitPoint);
                        // Normal needs to be flipped if this is a refractive ray.
                        if (Vector3.DotProduct(ray.Dir, normal) > 0)
                            normal = -normal;

                        return new IntersectionInfo(retval, this, fDistance, hitPoint, hitPoint, normal);
                    }
                    else
			            return new IntersectionInfo(HitResult.MISS);
		        }
	        }

            return new IntersectionInfo(HitResult.MISS);       
        }

        private Normal3 GetNormal(Point3 vPoint)
        {
            Normal3 vNorm = (Normal3)vPoint - Pos;
	        vNorm.Normalize();
	        return vNorm;
        }

        public override bool Intersect(AABB aabb)
        {
            // Get the center of the sphere relative to the center of the box
            Vector3 sphereCenterRelBox = this.Pos - aabb.Center;
            // Point on surface of box that is closest to the center of the sphere
            Vector3 boxPoint = new Vector3();

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

            Vector3 dist = sphereCenterRelBox - boxPoint;

            return (dist.X * dist.X + dist.Y * dist.Y + dist.Z * dist.Z < this.Radius * this.Radius);
        }

        public override AABB GetAABB()
        {
            return new AABB()
            {
                Min = new Vector3(this.Pos.X - Radius, this.Pos.Y - Radius, this.Pos.Z - Radius),
                Max = new Vector3(this.Pos.X + Radius, this.Pos.Y + Radius, this.Pos.Z + Radius)
            };
        }

        public override bool Contains(Point3 point)
        {
            double radius = this.Radius + MathLib.Epsilon;
            return (point - this.Pos).GetLengthSquared() <= (radius * radius);
        }
    }
}
