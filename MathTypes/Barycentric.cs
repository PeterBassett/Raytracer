using System;

namespace Raytracer.MathTypes
{
    public class Barycentric
    {
        public static Vector CalculateBarycentricInterpolationVector(Point point, Point[] vertexes)
        {
            var p1 = vertexes[0];
            var p2 = vertexes[1];
            var p3 = vertexes[2];

            // calculate vectors from point f to vertices p1, p2 and p3
            var f1 = p1 - point;
            var f2 = p2 - point;
            var f3 = p3 - point;

            // calculate the areas and factors (order of parameters doesn't matter):
            var a = Vector.CrossProduct(p1 - p2, p1 - p3).Length; // main triangle area a

            var a1 = Vector.CrossProduct(f2, f3).Length / a; // p1's triangle area / a
            var a2 = Vector.CrossProduct(f3, f1).Length / a; // p2's triangle area / a 
            var a3 = Vector.CrossProduct(f1, f2).Length / a; // p3's triangle area / a

            return new Vector(a1, a2, a3);
        }

        public static Vector CalculateSignedBarycentricInterpolationVector(Point pointOfIntersection, Point[] vertexes)
        {
            var p1 = vertexes[0];
            var p2 = vertexes[1];
            var p3 = vertexes[2];

            // calculate vectors from point f to vertices p1, p2 and p3:
            var f1 = p1 - pointOfIntersection;
            var f2 = p2 - pointOfIntersection;
            var f3 = p3 - pointOfIntersection;

            // calculate the areas (parameters order is essential in this case):
            var va = Vector.CrossProduct(p1 - p2, p1 - p3); // main triangle cross product
            var va1 = Vector.CrossProduct(f2, f3); // p1's triangle cross product
            var va2 = Vector.CrossProduct(f3, f1); // p2's triangle cross product
            var va3 = Vector.CrossProduct(f1, f2); // p3's triangle cross product

            var a = va.Length; // main triangle area

            // calculate barycentric coordinates with sign:
            var a1 = va1.Length / a * Math.Sign(Vector.DotProduct(va, va1));
            var a2 = va2.Length / a * Math.Sign(Vector.DotProduct(va, va2));
            var a3 = va3.Length / a * Math.Sign(Vector.DotProduct(va, va3));

            return new Vector(a1, a2, a3);
        }
    }
}
