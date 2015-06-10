using Raytracer.MathTypes;
using Raytracer.Rendering.Materials;

namespace Raytracer.Rendering.Core
{
    abstract class Traceable
    {
        protected Traceable()
        {
            Pos = new Point(0.0f, 0.0f, 0.0f);
            Ori = new Vector(0.0f, 0.0f, 0.0f);
        }
        public abstract IntersectionInfo Intersect(Ray ray);
        public abstract bool Contains(Point point);
        // ReSharper disable once InconsistentNaming
        public abstract AABB GetAABB();

        public Point Pos { get; set; }
        public Vector Ori { get; set; }
        public virtual Material Material { get; set; }        
    }
}
