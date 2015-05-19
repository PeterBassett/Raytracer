using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using Raytracer.Rendering.Core;
using System.Xml.Linq;
using Raytracer.Rendering.FileTypes.XMLRayScene;
using Raytracer.Rendering.FileTypes.XMLRayScene.Loaders;

namespace Raytracer.Rendering.FileTypes.VBRayScene
{
    class RaySceneLoader : ISceneLoader
    {
        public Scene LoadScene(Stream sceneStream)
        {
            var loader = GetLoader(sceneStream);
            return loader.LoadScene(sceneStream);
        }

        public void SaveScene(StreamWriter output, Scene scene)
        {
            throw new NotImplementedException();
        }

        public ISceneLoader GetLoader(Stream scene)
        {
            var bufferedStream = new BufferedStream(scene, 4096);
            ISceneLoader loader = null;

            loader = new XMLRaySceneLoader();

            if (loader.CanLoadStream(scene))
            {
                scene.Seek(0, SeekOrigin.Begin);
                return loader;
            }
            else
                return new VBRaySceneLoader();
        }

        public bool CanLoadStream(Stream sceneStream)
        {
            return true;
        }
    }
}
