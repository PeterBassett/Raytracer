using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.Rendering.FileTypes;

namespace Raytracer.Rendering.BackgroundMaterials
{
    interface IBackgroundMaterial
    {
        Colour Shade(Ray ray);
    }
}
