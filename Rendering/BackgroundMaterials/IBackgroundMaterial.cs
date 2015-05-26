using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.BackgroundMaterials
{
    interface IBackgroundMaterial
    {
        Colour Shade(Ray ray);
    }
}
