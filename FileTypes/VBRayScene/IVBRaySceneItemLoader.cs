using System.IO;
using Raytracer.Rendering.Core;

namespace Raytracer.FileTypes.VBRayScene
{
    interface IVBRaySceneItemLoader
    {
        string LoaderType { get; }
        void LoadObject(StreamReader file, Scene scene);
    }
}
