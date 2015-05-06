using System.Collections.Generic;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Primitives;

namespace Raytracer.Rendering.Accellerators.Partitioners
{
    interface IPrimitivePartitioner
    {
        bool Partition(IList<Traceable> primitives, int depth, ref AABB bounds, ref List<Traceable> leftPrims, ref List<Traceable> rightPrims);
    }
}
