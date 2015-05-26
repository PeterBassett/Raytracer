using System;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Materials;

namespace Raytracer.Rendering.Primitives
{
    class Sphere : Traceable
    {
        private readonly double _radius;

        public Sphere(Point point, Vector rotation, double radius, Material mat)
        {
            _radius = radius;

            Pos = point;
            Ori = rotation;            
            Material = mat;
        }
        
        public override IntersectionInfo Intersect(Ray ray)
        {
            var retval = HitResult.Miss;

            double fDistance = 0.0f;

            Vector relativePosition = ray.Pos - Pos;
            double fB = 2.0f * (Vector.DotProduct(ray.Dir, relativePosition));  
	        double fC = Vector.DotProduct(relativePosition, relativePosition) - ( _radius *  _radius );
	        double fA = 1.0f;

	        double fD = ( fB * fB ) - 4.0f * fA * fC;

	        if( fD < 0.0f )
                return new IntersectionInfo(HitResult.Miss);

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
                        retval = HitResult.Hit;
                    }

			        if(fDist2 > 1.0f)
			        {
				        if( fDist2 < fDist1 || fDist1 < 1.0f) 
                        {
					        fDistance = fDist2;
                            retval = HitResult.InPrim;
                        }
			        }

		            if(retval != HitResult.Miss)
                    {
                        var hitPoint = ray.Pos + (ray.Dir * fDistance);
                        var normal = GetNormal(hitPoint);
                        // Normal needs to be flipped if this is a refractive ray.
                        if (Vector.DotProduct(ray.Dir, normal) > 0)
                            normal = -normal;

                        return new IntersectionInfo(retval, this, fDistance, hitPoint, hitPoint, normal);
                    }
                    else
			            return new IntersectionInfo(HitResult.Miss);
		        }
	        }

            return new IntersectionInfo(HitResult.Miss);       
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
                Min = this.Pos - _radius,
                Max = this.Pos + _radius
            };
        }

        public override bool Contains(Point point)
        {
            var radius = this._radius + MathLib.Epsilon;
            return (point - this.Pos).LengthSquared <= (radius * radius);
        }
    }
}
