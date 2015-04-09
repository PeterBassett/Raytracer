using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.MathTypes;

namespace Raytracer.Rendering.Core
{
    public enum HitResult
    {
        MISS = 0, // Ray missed primitive
        HIT = 1, // Ray hit primitive
        INPRIM = -1 // Ray started inside primitive        
    }
}
