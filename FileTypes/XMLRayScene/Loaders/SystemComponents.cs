using System.Threading;
using Raytracer.Rendering.Cameras;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Renderers;

namespace Raytracer.FileTypes.XMLRayScene.Loaders
{
    class SystemComponents
    {
        public Scene scene;
        public IRenderer renderer;
        public ICamera camera;
        public CancellationTokenSource cancellationTokenSource;
    }
}
