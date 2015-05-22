using System.IO;
using Raytracer.FileTypes.VBRayScene;
using Raytracer.FileTypes.XMLRayScene;
using Raytracer.Rendering.Core;

namespace Raytracer.FileTypes
{
    class RaySceneLoader : ISceneLoader
    {
        public Scene LoadScene(Stream sceneStream)
        {
            using (var loader = GetLoader(sceneStream))
            {
                sceneStream.Seek(0, SeekOrigin.Begin);

                return loader.LoadScene(sceneStream);
            }
        }

        private ISceneLoader GetLoader(Stream scene)
        {
            var loader = new XmlRaySceneLoader();

            if (loader.CanLoadStream(scene))
                return loader;

            loader.Dispose();

            return new VbRaySceneLoader();
        }

        public bool CanLoadStream(Stream sceneStream)
        {
            return true;
        }

        public void Dispose()
        {
            // do nothing. we don't hold references in this implementation.
        }
    }
}
