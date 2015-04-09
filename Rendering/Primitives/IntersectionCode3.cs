using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.Primitives
{
    using Real = System.Double;    
    static class IntersectionCode3
    {

        public static bool smits_div(Ray r, AABB b, ref Real t)
        {
            Real tnear = -1e8f;
            Real tfar = 1e8f;

            {
                Real t1 = (b.Min.X - r.x) / r.i;
                Real t2 = (b.Max.X - r.x) / r.i;

                if (t1 > t2)
                {
                    Real temp = t1;
                    t1 = t2;
                    t2 = temp;
                }
                if (t1 > tnear)
                    tnear = t1;
                if (t2 < tfar)
                    tfar = t2;

                if (tnear > tfar)
                    return false;
                if (tfar < 0.0)
                    return false;
            }
            {
                Real t1 = (b.Min.Y - r.y) / r.j;
                Real t2 = (b.Max.Y - r.y) / r.j;

                if (t1 > t2)
                {
                    Real temp = t1;
                    t1 = t2;
                    t2 = temp;
                }
                if (t1 > tnear)
                    tnear = t1;
                if (t2 < tfar)
                    tfar = t2;

                if (tnear > tfar)
                    return false;
                if (tfar < 0.0)
                    return false;
            }
            {
                Real t1 = (b.Min.Z - r.z) / r.k;
                Real t2 = (b.Max.Z - r.z) / r.k;

                if (t1 > t2)
                {
                    Real temp = t1;
                    t1 = t2;
                    t2 = temp;
                }
                if (t1 > tnear)
                    tnear = t1;
                if (t2 < tfar)
                    tfar = t2;

                if (tnear > tfar)
                    return false;
                if (tfar < 0.0)
                    return false;
            }

            t = tnear;
            return true;
        }

        public static bool pluecker_cls_cff(Ray r, AABB b)
        {
            switch (r.classification)
            {
                case Ray.CLASSIFICATION.MMM:
                    // side(R,HD) < 0 or side(R,FB) > 0 or side(R,EF) > 0 or side(R,DC) < 0 or side(R,CB) < 0 or side(R,HE) > 0 to miss

                    if ((r.Pos.X < b.Min.X) || (r.Pos.Y < b.Min.Y) || (r.Pos.Z < b.Min.Z) ||
                        (r.R0 + r.i * b.Min.Y - r.j * b.Max.X < 0) ||
                        (r.R0 + r.i * b.Max.Y - r.j * b.Min.X > 0) ||
                        (r.R1 + r.i * b.Max.Z - r.k * b.Min.X > 0) ||
                        (r.R1 + r.i * b.Min.Z - r.k * b.Max.X < 0) ||
                        (r.R3 - r.k * b.Max.Y + r.j * b.Min.Z < 0) ||
                        (r.R3 - r.k * b.Min.Y + r.j * b.Max.Z > 0))
                        return false;

                    return true;

                case Ray.CLASSIFICATION.MMP:
                    // side(R,HD) < 0 or side(R,FB) > 0 or side(R,HG) > 0 or side(R,AB) < 0 or side(R,DA) < 0 or side(R,GF) > 0 to miss

                    if ((r.Pos.X < b.Min.X) || (r.Pos.Y < b.Min.Y) || (r.Pos.Z > b.Max.Z) ||
                        (r.R0 + r.i * b.Min.Y - r.j * b.Max.X < 0) ||
                        (r.R0 + r.i * b.Max.Y - r.j * b.Min.X > 0) ||
                        (r.R1 + r.i * b.Max.Z - r.k * b.Max.X > 0) ||
                        (r.R1 + r.i * b.Min.Z - r.k * b.Min.X < 0) ||
                        (r.R3 - r.k * b.Min.Y + r.j * b.Min.Z < 0) ||
                        (r.R3 - r.k * b.Max.Y + r.j * b.Max.Z > 0))
                        return false;

                    return true;

                case Ray.CLASSIFICATION.MPM:
                    // side(R,EA) < 0 or side(R,GC) > 0 or side(R,EF) > 0 or side(R,DC) < 0 or side(R,GF) < 0 or side(R,DA) > 0 to miss

                    if ((r.Pos.X < b.Min.X) || (r.Pos.Y > b.Max.Y) || (r.Pos.Z < b.Min.Z) ||
                        (r.R0 + r.i * b.Min.Y - r.j * b.Min.X < 0) ||
                        (r.R0 + r.i * b.Max.Y - r.j * b.Max.X > 0) ||
                        (r.R1 + r.i * b.Max.Z - r.k * b.Min.X > 0) ||
                        (r.R1 + r.i * b.Min.Z - r.k * b.Max.X < 0) ||
                        (r.R3 - r.k * b.Max.Y + r.j * b.Max.Z < 0) ||
                        (r.R3 - r.k * b.Min.Y + r.j * b.Min.Z > 0))
                        return false;

                    return true;

                case Ray.CLASSIFICATION.MPP:
                    // side(R,EA) < 0 or side(R,GC) > 0 or side(R,HG) > 0 or side(R,AB) < 0 or side(R,HE) < 0 or side(R,CB) > 0 to miss

                    if ((r.Pos.X < b.Min.X) || (r.Pos.Y > b.Max.Y) || (r.Pos.Z > b.Max.Z) ||
                        (r.R0 + r.i * b.Min.Y - r.j * b.Min.X < 0) ||
                        (r.R0 + r.i * b.Max.Y - r.j * b.Max.X > 0) ||
                        (r.R1 + r.i * b.Max.Z - r.k * b.Max.X > 0) ||
                        (r.R1 + r.i * b.Min.Z - r.k * b.Min.X < 0) ||
                        (r.R3 - r.k * b.Min.Y + r.j * b.Max.Z < 0) ||
                        (r.R3 - r.k * b.Max.Y + r.j * b.Min.Z > 0))
                        return false;

                    return true;

                case Ray.CLASSIFICATION.PMM:
                    // side(R,GC) < 0 or side(R,EA) > 0 or side(R,AB) > 0 or side(R,HG) < 0 or side(R,CB) < 0 or side(R,HE) > 0 to miss

                    if ((r.Pos.X > b.Max.X) || (r.Pos.Y < b.Min.Y) || (r.Pos.Z < b.Min.Z) ||
                        (r.R0 + r.i * b.Max.Y - r.j * b.Max.X < 0) ||
                        (r.R0 + r.i * b.Min.Y - r.j * b.Min.X > 0) ||
                        (r.R1 + r.i * b.Min.Z - r.k * b.Min.X > 0) ||
                        (r.R1 + r.i * b.Max.Z - r.k * b.Max.X < 0) ||
                        (r.R3 - r.k * b.Max.Y + r.j * b.Min.Z < 0) ||
                        (r.R3 - r.k * b.Min.Y + r.j * b.Max.Z > 0))
                        return false;

                    return true;

                case Ray.CLASSIFICATION.PMP:
                    // side(R,GC) < 0 or side(R,EA) > 0 or side(R,DC) > 0 or side(R,EF) < 0 or side(R,DA) < 0 or side(R,GF) > 0 to miss

                    if ((r.Pos.X > b.Max.X) || (r.Pos.Y < b.Min.Y) || (r.Pos.Z > b.Max.Z) ||
                        (r.R0 + r.i * b.Max.Y - r.j * b.Max.X < 0) ||
                        (r.R0 + r.i * b.Min.Y - r.j * b.Min.X > 0) ||
                        (r.R1 + r.i * b.Min.Z - r.k * b.Max.X > 0) ||
                        (r.R1 + r.i * b.Max.Z - r.k * b.Min.X < 0) ||
                        (r.R3 - r.k * b.Min.Y + r.j * b.Min.Z < 0) ||
                        (r.R3 - r.k * b.Max.Y + r.j * b.Max.Z > 0))
                        return false;

                    return true;

                case Ray.CLASSIFICATION.PPM:
                    // side(R,FB) < 0 or side(R,HD) > 0 or side(R,AB) > 0 or side(R,HG) < 0 or side(R,GF) < 0 or side(R,DA) > 0 to miss

                    if ((r.Pos.X > b.Max.X) || (r.Pos.Y > b.Max.Y) || (r.Pos.Z < b.Min.Z) ||
                        (r.R0 + r.i * b.Max.Y - r.j * b.Min.X < 0) ||
                        (r.R0 + r.i * b.Min.Y - r.j * b.Max.X > 0) ||
                        (r.R1 + r.i * b.Min.Z - r.k * b.Min.X > 0) ||
                        (r.R1 + r.i * b.Max.Z - r.k * b.Max.X < 0) ||
                        (r.R3 - r.k * b.Max.Y + r.j * b.Max.Z < 0) ||
                        (r.R3 - r.k * b.Min.Y + r.j * b.Min.Z > 0))
                        return false;

                    return true;

                case Ray.CLASSIFICATION.PPP:
                    // side(R,FB) < 0 or side(R,HD) > 0 or side(R,DC) > 0 or side(R,EF) < 0 or side(R,HE) < 0 or side(R,CB) > 0 to miss

                    if ((r.Pos.X > b.Max.X) || (r.Pos.Y > b.Max.Y) || (r.Pos.Z > b.Max.Z) ||
                        (r.R0 + r.i * b.Max.Y - r.j * b.Min.X < 0) ||
                        (r.R0 + r.i * b.Min.Y - r.j * b.Max.X > 0) ||
                        (r.R1 + r.i * b.Min.Z - r.k * b.Max.X > 0) ||
                        (r.R1 + r.i * b.Max.Z - r.k * b.Min.X < 0) ||
                        (r.R3 - r.k * b.Min.Y + r.j * b.Max.Z < 0) ||
                        (r.R3 - r.k * b.Max.Y + r.j * b.Min.Z > 0))
                        return false;

                    return true;
            }

            return false;
        }
    }
}
