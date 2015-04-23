using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.MathTypes;

namespace Raytracer.Rendering.Primitives
{
    
    

    static class IntersectionCode2
    {
        const int X = 0;
        const int Y = 1;
        const int Z = 2;

        static void FINDMINMAX(double x0, double x1, double x2, ref double min, ref double max)
        {

            min = max = x0;

            if (x1 < min) min = x1;

            if (x1 > max) max = x1;

            if (x2 < min) min = x2;

            if (x2 > max) max = x2;

        }

        static bool planeBoxOverlap(Vector3 normal, Vector3 vert, Vector3 maxbox)	// -NJMP-
        {

            int q;

            Vector3 vmin = new Vector3();
            Vector3 vmax = new Vector3();
            double v;

            for (q = X; q <= Z; q++)
            {

                v = vert[q];					// -NJMP-

                if (normal[q] > 0.0f)
                {

                    vmin[q] = -maxbox[q] - v;	// -NJMP-

                    vmax[q] = maxbox[q] - v;	// -NJMP-

                }

                else
                {

                    vmin[q] = maxbox[q] - v;	// -NJMP-

                    vmax[q] = -maxbox[q] - v;	// -NJMP-

                }

            }

            if (Vector3.DotProduct(normal, vmin) > 0.0f) return false;	// -NJMP-

            if (Vector3.DotProduct(normal, vmax) >= 0.0f) return true;	// -NJMP-



            return false;

        }

        static bool AXISTEST_X01(Vector3 v0, Vector3 v2, double a, double b, double fa, double fb, Vector3 boxhalfsize)
        {
            double p0 = a * v0[Y] - b * v0[Z];
            double p2 = a * v2[Y] - b * v2[Z];
            double min, max;

            if (p0 < p2) { min = p0; max = p2; } else { min = p2; max = p0; }

            double rad = fa * boxhalfsize[Y] + fb * boxhalfsize[Z];

            if (min > rad || max < -rad)
                return false;

            return true;
        }

        static bool AXISTEST_X2(Vector3 v0, Vector3 v1, double a, double b, double fa, double fb, Vector3 boxhalfsize)
        {
            double min, max;
            double p0 = a * v0[Y] - b * v0[Z];

            double p1 = a * v1[Y] - b * v1[Z];

            if (p0 < p1) { min = p0; max = p1; } else { min = p1; max = p0; }

            double rad = fa * boxhalfsize[Y] + fb * boxhalfsize[Z];

            if (min > rad || max < -rad) return false;

            return true;
        }

        static bool AXISTEST_Y02(Vector3 v0, Vector3 v2, double a, double b, double fa, double fb, Vector3 boxhalfsize)
        {
            double min, max;
            double p0 = -a * v0[X] + b * v0[Z];
            double p2 = -a * v2[X] + b * v2[Z];

            if (p0 < p2) { min = p0; max = p2; } else { min = p2; max = p0; }

            double rad = fa * boxhalfsize[X] + fb * boxhalfsize[Z];

            if (min > rad || max < -rad) return false;

            return true;
        }


        static bool AXISTEST_Y1(Vector3 v0, Vector3 v1, double a, double b, double fa, double fb, Vector3 boxhalfsize)
        {
            double min, max;
            double p0 = -a * v0[X] + b * v0[Z];

            double p1 = -a * v1[X] + b * v1[Z];

            if (p0 < p1) { min = p0; max = p1; } else { min = p1; max = p0; }

            double rad = fa * boxhalfsize[X] + fb * boxhalfsize[Z];

            if (min > rad || max < -rad) return false;

            return true;
        }




        static bool AXISTEST_Z12(Vector3 v1, Vector3 v2, double a, double b, double fa, double fb, Vector3 boxhalfsize)
        {
            double min, max;
            double p1 = a * v1[X] - b * v1[Y];

            double p2 = a * v2[X] - b * v2[Y];

            if (p2 < p1) { min = p2; max = p1; } else { min = p1; max = p2; }

            double rad = fa * boxhalfsize[X] + fb * boxhalfsize[Y];

            if (min > rad || max < -rad) return false;
            return true;
        }


        static bool AXISTEST_Z0(Vector3 v0, Vector3 v1, double a, double b, double fa, double fb, Vector3 boxhalfsize)
        {
            double min, max;
            double p0 = a * v0[X] - b * v0[Y];

            double p1 = a * v1[X] - b * v1[Y];

            if (p0 < p1) { min = p0; max = p1; } else { min = p1; max = p0; }

            double rad = fa * boxhalfsize[X] + fb * boxhalfsize[Y];

            if (min > rad || max < -rad) return false;
            return true;

        }

        public static bool triBoxOverlap(Point3 boxcenter, Vector3 boxhalfsize, Point3[] triverts)
        {



            /*    use separating axis theorem to test overlap between triangle and box */

            /*    need to test for overlap in these directions: */

            /*    1) the {x,y,z}-directions (actually, since we use the AABB of the triangle */

            /*       we do not even need to test these) */

            /*    2) normal of the triangle */

            /*    3) crossproduct(edge from tri, {x,y,z}-directin) */

            /*       this gives 3x3=9 more tests */

            Vector3 v0;
            Vector3 v1;
            Vector3 v2;

            //   double axis[3];

            double fex, fey, fez;		// -NJMP- "d" local variable removed

            Vector3 normal;
            Vector3 e0;
            Vector3 e1;
            Vector3 e2;

            /* This is the fastest branch on Sun */

            /* move everything so that the boxcenter is in (0,0,0) */

            v0 = triverts[0] - boxcenter;
            v1 = triverts[1] - boxcenter;
            v2 = triverts[2] - boxcenter;

            /* compute triangle edges */
            e0 = v1 - v0;      /* tri edge 0 */
            e1 = v2 - v1;      /* tri edge 1 */
            e2 = v0 - v2;      /* tri edge 2 */

            /* Bullet 3:  */
            /*  test the 9 tests first (this was faster) */
            fex = Math.Abs(e0[X]);
            fey = Math.Abs(e0[Y]);
            fez = Math.Abs(e0[Z]);

            if (!AXISTEST_X01(v0, v2, e0[Z], e0[Y], fez, fey, boxhalfsize))
                return false;
            if (!AXISTEST_Y02(v0, v2, e0[Z], e0[X], fez, fex, boxhalfsize))
                return false;
            if (!AXISTEST_Z12(v1, v2, e0[Y], e0[X], fey, fex, boxhalfsize))
                return false;

            fex = Math.Abs(e1[X]);
            fey = Math.Abs(e1[Y]);
            fez = Math.Abs(e1[Z]);

            if (!AXISTEST_X01(v0, v2, e1[Z], e1[Y], fez, fey, boxhalfsize))
                return false;
            if (!AXISTEST_Y02(v0, v2, e1[Z], e1[X], fez, fex, boxhalfsize))
                return false;
            if (!AXISTEST_Z0(v0, v1, e1[Y], e1[X], fey, fex, boxhalfsize))
                return false;

            fex = Math.Abs(e2[X]);
            fey = Math.Abs(e2[Y]);
            fez = Math.Abs(e2[Z]);

            if (!AXISTEST_X2(v0, v1, e2[Z], e2[Y], fez, fey, boxhalfsize))
                return false;
            if (!AXISTEST_Y1(v0, v1, e2[Z], e2[X], fez, fex, boxhalfsize))
                return false;
            if (!AXISTEST_Z12(v1, v2, e2[Y], e2[X], fey, fex, boxhalfsize))
                return false;


            /* Bullet 1: */
            /*  first test overlap in the {x,y,z}-directions */
            /*  find min, max of the triangle each direction, and test for overlap in */
            /*  that direction -- this is equivalent to testing a minimal AABB around */
            /*  the triangle against the AABB */

            /* test in X-direction */

            double min = 0;
            double max = 0;
            FINDMINMAX(v0[X], v1[X], v2[X], ref min, ref max);

            if (min > boxhalfsize[X] || max < -boxhalfsize[X]) return false;



            /* test in Y-direction */
            min = 0;
            max = 0;
            FINDMINMAX(v0[Y], v1[Y], v2[Y], ref min, ref max);

            if (min > boxhalfsize[Y] || max < -boxhalfsize[Y]) return false;



            /* test in Z-direction */
            min = 0;
            max = 0;
            FINDMINMAX(v0[Z], v1[Z], v2[Z], ref min, ref max);

            if (min > boxhalfsize[Z] || max < -boxhalfsize[Z]) return false;



            /* Bullet 2: */

            /*  test if the box intersects the plane of the triangle */

            /*  compute plane equation of triangle: normal*x+d=0 */

            normal = Vector3.CrossProduct(e0, e1);

            // -NJMP- (line removed here)

            if (!planeBoxOverlap(normal, v0, boxhalfsize)) return false;	// -NJMP-



            return true;   /* box and triangle overlaps */

        }
    }
}


