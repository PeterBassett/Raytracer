using System;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.Primitives
{
    class Cube : ObjectSpacePrimitive
    {
        public Cube(Transform transform) : base(transform)
        {
        }

        protected override IntersectionInfo ObjectSpaceIntersect(Ray ray)
        {
	        var S = ray.Pos;
	        var c = ray.Dir;

	        const double D = 0.5; // plane distance for unit cube

	        double t1, t2;
	        double tnear, tfar;
	        int tnear_index=0, tfar_index=0;

	        // intersect with the X planes
	        if (c.X == 0 && (S.X < -D || S.X > D))
		        return new IntersectionInfo(HitResult.Miss);
	        else
	        {
		        tnear = (-D - S.X) / c.X;
		        tfar = (D - S.X) / c.X;

		        if (tnear > tfar)
			        Swap(ref tnear, ref tfar);

		        if (tnear > tfar)
			        return new IntersectionInfo(HitResult.Miss);
		        if (tfar < 0)
			        return new IntersectionInfo(HitResult.Miss);
	        }

	        // intersect with the Y planes
	        if (c.Y == 0 && (S.Y < -D || S.Y > D))
		        return new IntersectionInfo(HitResult.Miss);
	        else
	        {
		        t1 = (-D - S.Y) / c.Y;
		        t2 = (D - S.Y) / c.Y;

		        if (t1 > t2)
                    Swap(ref t1, ref t2);

		        if (t1 > tnear)
		        {
			        tnear = t1;
			        tnear_index = 1;
		        }

		        if (t2 < tfar)
		        {
			        tfar = t2;
			        tfar_index = 1;
		        }

		        if (tnear > tfar)
			        return new IntersectionInfo(HitResult.Miss);
		        if (tfar < 0)
			        return new IntersectionInfo(HitResult.Miss);
	        }

	        // intersect with the Z planes
	        if (c.Z == 0 && (S.Z < -D || S.Z > D))
		        return new IntersectionInfo(HitResult.Miss);
	        else
	        {
		        t1 = (-D - S.Z) / c.Z;
		        t2 = (D - S.Z) / c.Z;

		        if (t1 > t2)
                    Swap(ref t1, ref t2);

		        if (t1 > tnear)
		        {
			        tnear = t1;
			        tnear_index = 2;
		        }

		        if (t2 < tfar)
		        {
			        tfar = t2;
			        tfar_index = 2;
		        }

		        if (tnear > tfar)
			        return new IntersectionInfo(HitResult.Miss);
		        if (tfar < 0)
			        return new IntersectionInfo(HitResult.Miss);
	        }

	        var normals = new [] {new Normal(1,0,0), new Normal(0,1,0), new Normal(0,0,1)};

            IntersectionInfo intersection;

	        if (tnear <= 0)
	        {
                intersection = new IntersectionInfo(HitResult.Hit, this, tfar, ray.Pos + ray.Dir * tfar, ray.Pos + ray.Dir * tfar, normals[tfar_index]);
            }
	        else
	        {
                intersection = new IntersectionInfo(HitResult.Hit, this, tnear, ray.Pos + ray.Dir * tnear, ray.Pos + ray.Dir * tnear, normals[tnear_index]);
            }

	        if (Vector.DotProduct(intersection.NormalAtHitPoint, c) > 0)
		        intersection.NormalAtHitPoint = -intersection.NormalAtHitPoint;

	        intersection.NormalAtHitPoint = intersection.NormalAtHitPoint.Normalize();

	        return intersection; 
        }

        protected override bool ObjectSpaceContains(Point point)
        {
            return ObjectSpaceGetAABB().Contains(point);
        }

        protected override AABB ObjectSpaceGetAABB()
        {
            var offset = new Vector(0.5, 0.5, 0.5);

            return new AABB
            {
                Min = Pos - offset,
                Max = Pos + offset
            };
        }

        private void Swap<T>(ref T a, ref T b)
        {
            T i = a;
            a = b;
            b = i;
        }
    }
}
