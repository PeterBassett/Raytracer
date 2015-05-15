using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.Rendering;
using System.ComponentModel.Composition;
using Raytracer.Rendering.Materials;

namespace Raytracer.Rendering.FileTypes.VBRayScene.Savers
{
    [Export(typeof(IVBRaySceneItemSaver))]
    class ColourMaterialSaver : IVBRaySceneItemSaver
    {        
        public Type SaverForType
        {
            get { return typeof(Material); }
        }

        public void SaveObject(System.IO.StreamWriter file, object ObjectToSave)
        {
            Material mat = (Material)ObjectToSave;

            file.WriteLine("ColourMaterial");
            file.WriteLine("(");
            file.WriteLine("\t\"{0}\",", mat.Name);
            
            SaveColourComponent(file, mat.Ambient);
-           file.WriteLine(", 'Ambient'");

            SaveColourComponent(file, mat.Diffuse);
            file.WriteLine(", 'Diffuse'");

            file.WriteLine("\t{0}, 'Specularity'", mat.Specularity);
            file.WriteLine("\t{0}, 'Specular Exponent'", mat.SpecularExponent);

            SaveColourComponent(file, mat.Reflective);
            file.WriteLine(", 'Reflected'");

            file.WriteLine("\t{0}, 'Refraction'", mat.Refraction);

            SaveColourComponent(file, mat.Transmitted);
            file.WriteLine(", 'Transmitted'");

            file.WriteLine(")");
            file.WriteLine();
        }

        private void SaveColourComponent(System.IO.StreamWriter file,Colour colour)
        {
 	        file.Write("\t{0},{1},{2}", colour.Red, colour.Green, colour.Blue);
        }
    }
}
