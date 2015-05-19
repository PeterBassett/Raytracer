using System.IO;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.FileTypes
{
    interface ISceneLoader
    {
        bool CanLoadStream(Stream sceneStream);
        Scene LoadScene(Stream sceneStream);
        void SaveScene(StreamWriter output, Scene scene);
    }
}
