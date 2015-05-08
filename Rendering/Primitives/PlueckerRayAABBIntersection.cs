using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.Primitives
{ 
    static class PlueckerRayAABBIntersection
    {  
        public static bool Intersect(Ray r, AABB b)
        {
            switch (r.classification)
            {
                case Ray.Classification.MMM:
                    // side(R,HD) < 0 or side(R,FB) > 0 or side(R,EF) > 0 or side(R,DC) < 0 or side(R,CB) < 0 or side(R,HE) > 0 to miss

                    if ((r.Pos.X < b.Min.X) || (r.Pos.Y < b.Min.Y) || (r.Pos.Z < b.Min.Z) ||
                        (r.R0 + r.Dir.X * b.Min.Y - r.Dir.Y * b.Max.X < 0) ||
                        (r.R0 + r.Dir.X * b.Max.Y - r.Dir.Y * b.Min.X > 0) ||
                        (r.R1 + r.Dir.X * b.Max.Z - r.Dir.Z * b.Min.X > 0) ||
                        (r.R1 + r.Dir.X * b.Min.Z - r.Dir.Z * b.Max.X < 0) ||
                        (r.R3 - r.Dir.Z * b.Max.Y + r.Dir.Y * b.Min.Z < 0) ||
                        (r.R3 - r.Dir.Z * b.Min.Y + r.Dir.Y * b.Max.Z > 0))
                        return false;

                    return true;

                case Ray.Classification.MMP:
                    // side(R,HD) < 0 or side(R,FB) > 0 or side(R,HG) > 0 or side(R,AB) < 0 or side(R,DA) < 0 or side(R,GF) > 0 to miss

                    if ((r.Pos.X < b.Min.X) || (r.Pos.Y < b.Min.Y) || (r.Pos.Z > b.Max.Z) ||
                        (r.R0 + r.Dir.X * b.Min.Y - r.Dir.Y * b.Max.X < 0) ||
                        (r.R0 + r.Dir.X * b.Max.Y - r.Dir.Y * b.Min.X > 0) ||
                        (r.R1 + r.Dir.X * b.Max.Z - r.Dir.Z * b.Max.X > 0) ||
                        (r.R1 + r.Dir.X * b.Min.Z - r.Dir.Z * b.Min.X < 0) ||
                        (r.R3 - r.Dir.Z * b.Min.Y + r.Dir.Y * b.Min.Z < 0) ||
                        (r.R3 - r.Dir.Z * b.Max.Y + r.Dir.Y * b.Max.Z > 0))
                        return false;

                    return true;

                case Ray.Classification.MPM:
                    // side(R,EA) < 0 or side(R,GC) > 0 or side(R,EF) > 0 or side(R,DC) < 0 or side(R,GF) < 0 or side(R,DA) > 0 to miss

                    if ((r.Pos.X < b.Min.X) || (r.Pos.Y > b.Max.Y) || (r.Pos.Z < b.Min.Z) ||
                        (r.R0 + r.Dir.X * b.Min.Y - r.Dir.Y * b.Min.X < 0) ||
                        (r.R0 + r.Dir.X * b.Max.Y - r.Dir.Y * b.Max.X > 0) ||
                        (r.R1 + r.Dir.X * b.Max.Z - r.Dir.Z * b.Min.X > 0) ||
                        (r.R1 + r.Dir.X * b.Min.Z - r.Dir.Z * b.Max.X < 0) ||
                        (r.R3 - r.Dir.Z * b.Max.Y + r.Dir.Y * b.Max.Z < 0) ||
                        (r.R3 - r.Dir.Z * b.Min.Y + r.Dir.Y * b.Min.Z > 0))
                        return false;

                    return true;

                case Ray.Classification.MPP:
                    // side(R,EA) < 0 or side(R,GC) > 0 or side(R,HG) > 0 or side(R,AB) < 0 or side(R,HE) < 0 or side(R,CB) > 0 to miss

                    if ((r.Pos.X < b.Min.X) || (r.Pos.Y > b.Max.Y) || (r.Pos.Z > b.Max.Z) ||
                        (r.R0 + r.Dir.X * b.Min.Y - r.Dir.Y * b.Min.X < 0) ||
                        (r.R0 + r.Dir.X * b.Max.Y - r.Dir.Y * b.Max.X > 0) ||
                        (r.R1 + r.Dir.X * b.Max.Z - r.Dir.Z * b.Max.X > 0) ||
                        (r.R1 + r.Dir.X * b.Min.Z - r.Dir.Z * b.Min.X < 0) ||
                        (r.R3 - r.Dir.Z * b.Min.Y + r.Dir.Y * b.Max.Z < 0) ||
                        (r.R3 - r.Dir.Z * b.Max.Y + r.Dir.Y * b.Min.Z > 0))
                        return false;

                    return true;

                case Ray.Classification.PMM:
                    // side(R,GC) < 0 or side(R,EA) > 0 or side(R,AB) > 0 or side(R,HG) < 0 or side(R,CB) < 0 or side(R,HE) > 0 to miss

                    if ((r.Pos.X > b.Max.X) || (r.Pos.Y < b.Min.Y) || (r.Pos.Z < b.Min.Z) ||
                        (r.R0 + r.Dir.X * b.Max.Y - r.Dir.Y * b.Max.X < 0) ||
                        (r.R0 + r.Dir.X * b.Min.Y - r.Dir.Y * b.Min.X > 0) ||
                        (r.R1 + r.Dir.X * b.Min.Z - r.Dir.Z * b.Min.X > 0) ||
                        (r.R1 + r.Dir.X * b.Max.Z - r.Dir.Z * b.Max.X < 0) ||
                        (r.R3 - r.Dir.Z * b.Max.Y + r.Dir.Y * b.Min.Z < 0) ||
                        (r.R3 - r.Dir.Z * b.Min.Y + r.Dir.Y * b.Max.Z > 0))
                        return false;

                    return true;

                case Ray.Classification.PMP:
                    // side(R,GC) < 0 or side(R,EA) > 0 or side(R,DC) > 0 or side(R,EF) < 0 or side(R,DA) < 0 or side(R,GF) > 0 to miss

                    if ((r.Pos.X > b.Max.X) || (r.Pos.Y < b.Min.Y) || (r.Pos.Z > b.Max.Z) ||
                        (r.R0 + r.Dir.X * b.Max.Y - r.Dir.Y * b.Max.X < 0) ||
                        (r.R0 + r.Dir.X * b.Min.Y - r.Dir.Y * b.Min.X > 0) ||
                        (r.R1 + r.Dir.X * b.Min.Z - r.Dir.Z * b.Max.X > 0) ||
                        (r.R1 + r.Dir.X * b.Max.Z - r.Dir.Z * b.Min.X < 0) ||
                        (r.R3 - r.Dir.Z * b.Min.Y + r.Dir.Y * b.Min.Z < 0) ||
                        (r.R3 - r.Dir.Z * b.Max.Y + r.Dir.Y * b.Max.Z > 0))
                        return false;

                    return true;

                case Ray.Classification.PPM:
                    // side(R,FB) < 0 or side(R,HD) > 0 or side(R,AB) > 0 or side(R,HG) < 0 or side(R,GF) < 0 or side(R,DA) > 0 to miss

                    if ((r.Pos.X > b.Max.X) || (r.Pos.Y > b.Max.Y) || (r.Pos.Z < b.Min.Z) ||
                        (r.R0 + r.Dir.X * b.Max.Y - r.Dir.Y * b.Min.X < 0) ||
                        (r.R0 + r.Dir.X * b.Min.Y - r.Dir.Y * b.Max.X > 0) ||
                        (r.R1 + r.Dir.X * b.Min.Z - r.Dir.Z * b.Min.X > 0) ||
                        (r.R1 + r.Dir.X * b.Max.Z - r.Dir.Z * b.Max.X < 0) ||
                        (r.R3 - r.Dir.Z * b.Max.Y + r.Dir.Y * b.Max.Z < 0) ||
                        (r.R3 - r.Dir.Z * b.Min.Y + r.Dir.Y * b.Min.Z > 0))
                        return false;

                    return true;

                case Ray.Classification.PPP:
                    // side(R,FB) < 0 or side(R,HD) > 0 or side(R,DC) > 0 or side(R,EF) < 0 or side(R,HE) < 0 or side(R,CB) > 0 to miss

                    if ((r.Pos.X > b.Max.X) || (r.Pos.Y > b.Max.Y) || (r.Pos.Z > b.Max.Z) ||
                        (r.R0 + r.Dir.X * b.Max.Y - r.Dir.Y * b.Min.X < 0) ||
                        (r.R0 + r.Dir.X * b.Min.Y - r.Dir.Y * b.Max.X > 0) ||
                        (r.R1 + r.Dir.X * b.Min.Z - r.Dir.Z * b.Max.X > 0) ||
                        (r.R1 + r.Dir.X * b.Max.Z - r.Dir.Z * b.Min.X < 0) ||
                        (r.R3 - r.Dir.Z * b.Min.Y + r.Dir.Y * b.Max.Z < 0) ||
                        (r.R3 - r.Dir.Z * b.Max.Y + r.Dir.Y * b.Min.Z > 0))
                        return false;

                    return true;
            }

            return false;
        }
    }
}
