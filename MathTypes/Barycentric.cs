using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raytracer.MathTypes
{
    public class Barycentric
    {
        public static Vector3 CalculateBarycentricInterpolationVector(Point3 point, Point3[] vertexes)
        {
            var p1 = vertexes[0];
            var p2 = vertexes[1];
            var p3 = vertexes[2];

            // calculate vectors from point f to vertices p1, p2 and p3
            var f1 = p1 - point;
            var f2 = p2 - point;
            var f3 = p3 - point;

            // calculate the areas and factors (order of parameters doesn't matter):
            var a = Vector3.CrossProduct(p1 - p2, p1 - p3).GetLength(); // main triangle area a

            var a1 = Vector3.CrossProduct(f2, f3).GetLength() / a; // p1's triangle area / a
            var a2 = Vector3.CrossProduct(f3, f1).GetLength() / a; // p2's triangle area / a 
            var a3 = Vector3.CrossProduct(f1, f2).GetLength() / a; // p3's triangle area / a

            return new Vector3(a1, a2, a3);
        }

        public static Vector3 CalculateSignedBarycentricInterpolationVector(Point3 pointOfIntersection, Point3[] vertexes)
        {
            var p1 = vertexes[0];
            var p2 = vertexes[1];
            var p3 = vertexes[2];

            // calculate vectors from point f to vertices p1, p2 and p3:
            var f1 = p1 - pointOfIntersection;
            var f2 = p2 - pointOfIntersection;
            var f3 = p3 - pointOfIntersection;

            // calculate the areas (parameters order is essential in this case):
            var va = Vector3.CrossProduct(p1 - p2, p1 - p3); // main triangle cross product
            var va1 = Vector3.CrossProduct(f2, f3); // p1's triangle cross product
            var va2 = Vector3.CrossProduct(f3, f1); // p2's triangle cross product
            var va3 = Vector3.CrossProduct(f1, f2); // p3's triangle cross product

            var a = va.GetLength(); // main triangle area

            // calculate barycentric coordinates with sign:
            var a1 = va1.GetLength() / a * Math.Sign(Vector3.DotProduct(va, va1));
            var a2 = va2.GetLength() / a * Math.Sign(Vector3.DotProduct(va, va2));
            var a3 = va3.GetLength() / a * Math.Sign(Vector3.DotProduct(va, va3));

            return new Vector3(a1, a2, a3);
        }
    }
}
