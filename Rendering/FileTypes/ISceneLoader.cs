using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.Rendering;
using System.IO;

namespace Raytracer.Rendering.FileTypes
{
    interface ISceneLoader
    {
        Scene LoadScene(Stream sceneStream);
        void SaveScene(StreamWriter output, Scene scene);
    }
}
