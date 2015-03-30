using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.Rendering;
using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Materials;

namespace Raytracer.Rendering.FileTypes.VBRayScene.Savers
{
    [Export(typeof(IVBRaySceneItemSaver))]
    class CheckerMaterialSaver : IVBRaySceneItemSaver
    {
        public Type SaverForType
        {
            get { return typeof(MaterialCheckerboard); }
        }

        public void SaveObject(System.IO.StreamWriter file, object ObjectToSave)
        {
            MaterialCheckerboard mat = (MaterialCheckerboard)ObjectToSave;

            file.WriteLine("CheckMaterial");
            file.WriteLine("(");
            file.WriteLine("\t{0},", mat.Name);

            file.WriteLine("\t\"{0}\", 'Submaterial 1'", mat.SubMaterial1.Name);
            file.WriteLine("\t\"{0}\", 'Submaterial 2'", mat.SubMaterial2.Name);

            file.WriteLine("\t{0}, {1}, {2} 'Size'", mat.Size.X, mat.Size.Y, mat.Size.Z);

            file.WriteLine(")");
            file.WriteLine();
        }
    }
}
