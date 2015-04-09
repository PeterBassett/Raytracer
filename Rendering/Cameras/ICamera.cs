using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.Cameras
{
    interface ICamera
    {
        Ray GenerateRayForPixel(Vector2 coordinate);
    }
}
