using Raytracer.Rendering.Cameras;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Renderers;
using Raytracer.Rendering.Synchronisation;

namespace Raytracer.FileTypes.XMLRayScene.Loaders
{
    class SystemComponents
    {
        public SystemComponents()
        {
            CancellationTokenSource = new CancellationTokenSource();
        }

        public Scene scene;
        public IRenderer renderer;
        public ICamera camera;
        public readonly CancellationTokenSource CancellationTokenSource;
    }
}
