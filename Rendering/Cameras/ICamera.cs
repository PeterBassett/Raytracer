using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.MathTypes;

namespace Raytracer.Rendering.Cameras
{
    interface ICamera
    {
        Ray GenerateRayForPixel(Vector2 coordinate);
    }
}
