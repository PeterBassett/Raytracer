using System;
using System.IO;
using Raytracer.FileTypes.XMLRayScene.Loaders;

namespace Raytracer.FileTypes
{
    interface ISceneLoader : IDisposable
    {
        SystemComponents LoadScene(Stream sceneStream);
    }
}
