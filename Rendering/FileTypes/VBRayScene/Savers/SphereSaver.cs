using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.Rendering;
using System.IO;
using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Primitives;

namespace Raytracer.Rendering.FileTypes.VBRayScene.Savers
{
    [Export(typeof(IVBRaySceneItemSaver))]
    class SphereSaver : IVBRaySceneItemSaver
    {        
        public Type SaverForType
        {
            get { return typeof(Raytracer.Rendering.Primitives.Sphere); }
        }

        public void SaveObject(StreamWriter file, object ObjectToSave)
        {
            Sphere sphere = (Sphere)ObjectToSave;

            file.WriteLine("Sphere");
            file.WriteLine("(");
            file.WriteLine("\t{0}, 'Radius'", sphere.Radius);
            file.WriteLine("\t{0}, 'X'", sphere.Pos.X);
            file.WriteLine("\t{0}, 'Y'", sphere.Pos.Y);
            file.WriteLine("\t{0}, 'Z'", sphere.Pos.Z);
            file.WriteLine("\t\"{0}\" 'Material'", sphere.Material.Name);
            file.WriteLine(")");
            file.WriteLine();
        }
    }
}