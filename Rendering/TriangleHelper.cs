using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.Rendering.Primitives;
using Raytracer.Rendering;

namespace Raytracer.Rendering
{
    using Vector = Raytracer.MathTypes.Vector3;
    using Raytracer.Rendering.Materials;

    class TriangleHelper
    {
        public static List<Triangle> CreateTriangles(List<Vector> verticies, Material currentMaterial)
        {
            List<Triangle> triangles = new List<Triangle>();

            Vector v1, v2, v3;

            v1 = verticies[0];

            for (int i = 0; i < verticies.Count - 2; ++i)
            {
                v2 = verticies[i + 1];
                v3 = verticies[i + 2];

                if (v1 == v2 || v1 == v3 || v2 == v3)
                    continue;

                Triangle tri = new Triangle();
                tri.Vertex[0] = v1;
                tri.Vertex[1] = v2;
                tri.Vertex[2] = v3;

                tri.Material = currentMaterial;
                triangles.Add(tri);
            }

            return triangles;
        }
    }
}
