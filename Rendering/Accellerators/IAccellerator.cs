using System.Collections.Generic;
using Raytracer.Rendering.Primitives;

namespace Raytracer.Rendering.Accellerators
{
    interface IAccelerator
    {
        void Build(IEnumerable<Traceable> primitives);
        IEnumerable<Traceable> Intersect(Ray ray);
    }
}
