using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Materials;

namespace Raytracer.Rendering.Primitives
{
    abstract class Traceable
    {
        protected Traceable()
        {
            Pos = new Point(0.0f, 0.0f, 0.0f);
            Ori = new Vector(0.0f, 0.0f, 0.0f);
        }
        public abstract IntersectionInfo Intersect(Ray ray);
        public abstract bool Intersect(AABB aabb);
        public abstract bool Contains(Point point);
        // ReSharper disable once InconsistentNaming
        public abstract AABB GetAABB();

        public Point Pos { get; set; }
        public Vector Ori { get; set; }
        public Material Material { get; set; }        
    }
}
