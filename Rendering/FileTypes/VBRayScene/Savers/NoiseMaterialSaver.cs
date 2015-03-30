using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.Rendering;
using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Materials;

namespace Raytracer.Rendering.FileTypes.VBRayScene.Loaders
{
    [Export(typeof(IVBRaySceneItemSaver))]
    class NoiseMaterialSaver : IVBRaySceneItemSaver
    {     
        public Type SaverForType
        {
            get { return typeof(MaterialNoise); }
        }

        public void SaveObject(System.IO.StreamWriter file, object ObjectToSave)
        {
            MaterialNoise mat = (MaterialNoise)ObjectToSave;

            file.WriteLine("NoiseMaterial");
            file.WriteLine("(");
            file.WriteLine("\t{0},", mat.Name);

            file.WriteLine("\t\"{0}\", 'Submaterial 1'", mat.SubMaterial1.Name);
            file.WriteLine("\t{\"0}\", 'Submaterial 2'", mat.SubMaterial2.Name);

            file.WriteLine("\t{0}, 'Seed'", mat.Seed);
            file.WriteLine("\t{0}, 'Persistence'", mat.Persistence);

            file.WriteLine("\t{0}, 'Octaves'", mat.Octaves);
            file.WriteLine("\t{0}, 'Scale'", mat.Scale);
            file.WriteLine("\t{0}, 'Offset'", mat.Offset);

            file.WriteLine("\t{0}, {1}, {2} 'Size'", mat.Size.X, mat.Size.Y, mat.Size.Z);

            file.WriteLine(")");
            file.WriteLine();
        }
    }
}
