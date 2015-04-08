using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.Rendering.FileTypes;

namespace Raytracer.Rendering
{
    interface IScene
    {
        Colour Trace(Ray ray);
    }
}
