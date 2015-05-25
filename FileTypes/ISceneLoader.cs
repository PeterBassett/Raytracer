using System;
using System.IO;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Renderers;
using Raytracer.Rendering.Cameras;
using Raytracer.FileTypes.XMLRayScene.Loaders;

namespace Raytracer.FileTypes
{
    interface ISceneLoader : IDisposable
    {
        bool CanLoadStream(Stream sceneStream);
        void LoadScene(Stream sceneStream, ref SystemComponents components);
    }
}
