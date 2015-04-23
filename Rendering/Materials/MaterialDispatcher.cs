using System;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Primitives;

namespace Raytracer.Rendering.Materials
{
    class MaterialDispatcher
    {
        public void Solidify(Traceable unused, Material material, IntersectionInfo info, Material output)
        {
            material.SolidifyMaterial(info, output);
        }
        
        public void Solidify(Triangle triangle, MaterialTexture material, IntersectionInfo info, Material output)
        {
            Material.CloneElements(output, material);

            var uv = InterpolateTextureUV(info.ObjectLocalHitPoint, triangle.Vertex, triangle.Texture);

            output.Diffuse = material.Sample(uv.X, uv.Y);
            output.Ambient = output.Diffuse;
        }
        
        public Vector2 InterpolateTextureUV(Point3 pointOfIntersection, Point3[] vertexes, Vector2[] textureUVs)
        {
            var a = Barycentric.CalculateBarycentricInterpolationVector(pointOfIntersection, vertexes);

            // find the uv corresponding to point f (uv1/uv2/uv3 are associated to p1/p2/p3):
            return textureUVs[0] * a.X +
                   textureUVs[1] * a.Y +
                   textureUVs[2] * a.Z;
        }

        public void Solidify(Sphere sphere, MaterialTexture material, IntersectionInfo info, Material output)
        {
            var d = info.HitPoint - sphere.Pos;
            
            d.RotateX(sphere.Ori.X, ref d);
            d.RotateY(sphere.Ori.Y, ref d);
            d.RotateZ(sphere.Ori.Z, ref d);

            d.Normalize();

            var u = 0.5 + Math.Atan2(d._z, d._x) / (2.0 * Math.PI);
            var v = 0.5 - Math.Asin(d._y) / Math.PI;            

            Material.CloneElements(output, material);

            output.Diffuse = material.Sample(u,v);
        }

        public void Solidify(Plane plane, MaterialTexture material, IntersectionInfo info, Material output)
        {
            var uAxis = new Vector3(plane.Normal.Y, plane.Normal.Z, -plane.Normal.X);
            var vAxis = Vector3.CrossProduct(uAxis, plane.Normal);

            Material.CloneElements(output, material);

            var u = Vector3.DotProduct(info.HitPoint, uAxis) * material.UScale;
            var v = Vector3.DotProduct(info.HitPoint, vAxis) * material.VScale;

            if (u < 0)
                u = 1.0 - u;

            if (v < 0)
                v = 1.0 - v;

            u = u - Math.Truncate(u);
            v = v - Math.Truncate(v);

            output.Diffuse = material.Sample(u, v);
        }
    }
}
