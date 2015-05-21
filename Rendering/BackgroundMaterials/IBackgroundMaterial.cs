using Raytracer.Rendering.Core;
using Raytracer.FileTypes;

namespace Raytracer.Rendering.BackgroundMaterials
{
    interface IBackgroundMaterial
    {
        Colour Shade(Ray ray);
    }
}
