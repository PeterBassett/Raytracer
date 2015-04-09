using System.IO;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.FileTypes.VBRayScene
{
    interface IVBRaySceneItemLoader
    {
        string LoaderType { get; }
        void LoadObject(StreamReader file, Scene scene);
    }
}
