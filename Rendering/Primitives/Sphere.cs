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

            Vector relativePosition = ray.Pos - Pos;
            double fB = 2.0f * (Vector.DotProduct(ray.Dir, relativePosition));  
	        double fC = Vector.DotProduct(relativePosition, relativePosition) - ( Radius *  Radius );
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
                        if (Vector.DotProduct(ray.Dir, normal) > 0)
                            normal = -normal;

                        return new IntersectionInfo(retval, this, fDistance, hitPoint, hitPoint, normal);
                    }
                    else
			            return new IntersectionInfo(HitResult.MISS);
		        }
	        }

            return new IntersectionInfo(HitResult.MISS);       
        }

        private Normal GetNormal(Point vPoint)
        {
            Normal vNorm = (Normal)vPoint - Pos;
	        return vNorm.Normalize();
        }

        public override AABB GetAABB()
        {
            return new AABB()
            {
                Min = this.Pos - Radius,
                Max = this.Pos + Radius
            };
        }

        public override bool Contains(Point point)
        {
            double radius = this.Radius + MathLib.Epsilon;
            return (point - this.Pos).LengthSquared <= (radius * radius);
        }
    }
}
