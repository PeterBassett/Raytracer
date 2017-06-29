using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Cameras;
using Raytracer.Rendering.RenderingStrategies;
using Raytracer.Rendering.Distributions;

namespace Raytracer.Rendering.Renderers
{
    interface IRenderer
    {
        RenderSettings Settings { get; set; }
        IRenderingStrategy RenderingStrategy { get; set; }
        ICamera Camera { get; set; }
        Scene Scene { get; set; }
        void RenderScene(Buffer frameBuffer);
        Colour ComputeSample(Vector2 pixelCoordinate);
        IntersectionInfo FindClosestIntersection(Ray ray);
        Distribution Distribution { get; set; }
    }
}
