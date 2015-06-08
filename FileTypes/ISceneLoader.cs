using System;
using System.IO;
using Raytracer.FileTypes.XMLRayScene.Loaders;
using Raytracer.Rendering.Core;

namespace Raytracer.FileTypes
{
    interface ISceneLoader : IDisposable
    {
        SystemComponents LoadScene(Stream sceneStream, Scene existingScene);
    }
}
