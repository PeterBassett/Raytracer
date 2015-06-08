using Raytracer.Rendering.Cameras;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Renderers;
using Raytracer.Rendering.Synchronisation;
using Raytracer.Rendering.Primitives;

namespace Raytracer.FileTypes.XMLRayScene.Loaders
{
    class SystemComponents
    {
        public SystemComponents()
        {
            CancellationTokenSource = new CancellationTokenSource();
        }

        public Scene Scene;
        public IRenderer Renderer;
        public ICamera Camera;
        public readonly CancellationTokenSource CancellationTokenSource;
        public Scene ExistingScene;

        internal bool HasMeshFile(string meshName)
        {
            if (ExistingScene == null)
                return false;

            return ExistingScene.FindMesh(meshName) != null;
        }

        internal Mesh GetMesh(string meshName)
        {
            return ExistingScene.FindMesh(meshName);
        }
    }
}
