using Raytracer.Rendering.Core;
using Raytracer.Rendering.FileTypes;

namespace Raytracer.Rendering.BackgroundMaterials
{
    interface IBackgroundMaterial
    {
        Colour Shade(Ray ray);
    }
}
