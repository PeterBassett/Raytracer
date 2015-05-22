using System.IO;
using Raytracer.Rendering.Core;

namespace Raytracer.FileTypes.VBRayScene
{
    interface IVbRaySceneItemLoader
    {
        string LoaderType { get; }
        void LoadObject(StreamReader file, Scene scene);
    }
}
