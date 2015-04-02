﻿using System;
using System.Collections.Generic;
namespace Raytracer.Rendering.Accellerators
{
    interface IAccelerator
    {
        void Build(IEnumerable<Traceable> primitives);
        IEnumerable<Traceable> Intersect(Ray ray);
    }
}