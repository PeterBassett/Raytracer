using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.MathTypes;

namespace Raytracer.Rendering
{
    using Vector = Vector3;
    using Real = System.Double;
    using Raytracer.Rendering.Materials;

    abstract class Traceable
    {
        public Traceable()
        {
            Pos = new Vector(0.0f, 0.0f, 0.0f);
            Ori = new Vector(0.0f, 0.0f, 0.0f);
        }
        public abstract IntersectionInfo Intersect(Ray ray);
        public abstract bool Intersect(AABB aabb);
        public abstract Vector GetNormal(Vector vPoint);
        public abstract AABB GetAABB();

        public Vector Pos { get; set; }
        public Vector Ori { get; set; }
        public Material Material { get; set; }
        
    }

}
