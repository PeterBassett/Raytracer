using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Raytracer.Rendering;

namespace Raytracer.Rendering.FileTypes.VBRayScene
{
    interface IVBRaySceneItemLoader
    {
        string LoaderType { get; }
        void LoadObject(StreamReader file, Scene scene);
    }
}
