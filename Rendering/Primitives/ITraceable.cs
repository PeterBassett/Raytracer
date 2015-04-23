using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Materials;

namespace Raytracer.Rendering.Primitives
{
    abstract class Traceable
    {
        protected Traceable()
        {
            Pos = new Point3(0.0f, 0.0f, 0.0f);
            Ori = new Vector3(0.0f, 0.0f, 0.0f);
        }
        public abstract IntersectionInfo Intersect(Ray ray);
        public abstract bool Intersect(AABB aabb);
        public abstract bool Contains(Point3 point);
        // ReSharper disable once InconsistentNaming
        public abstract AABB GetAABB();

        public Point3 Pos { get; set; }
        public Vector3 Ori { get; set; }
        public Material Material { get; set; }        
    }
}
