﻿using System.Collections.Generic;
using Raytracer.Rendering.Core;
using Raytracer.MathTypes;

namespace Raytracer.Rendering.Accellerators
{
    interface IAccelerator
    {
        void Build(IEnumerable<Traceable> primitives);
        IEnumerable<Traceable> Intersect(Ray ray);
        IEnumerable<Traceable> Intersect(Point point);
    }
}
