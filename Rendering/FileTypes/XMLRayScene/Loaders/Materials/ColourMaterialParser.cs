using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Raytracer.Rendering.Lights;
using Raytracer.Rendering.Materials;

namespace Raytracer.Rendering.FileTypes.XMLRayScene.Loaders.Lights
{
    [Export(typeof(XMLRayElementParser))]
    class ColourMaterialParser : XMLRayElementParser
    {
        public override string LoaderType { get { return "ColourMaterial"; } }

        public override dynamic LoadObject(VBRayScene.XMLRaySceneLoader loader, Core.Scene scene, System.Xml.Linq.XElement element, string elementName, Func<dynamic> createDefault)
        {
            var colour = loader.LoadObject<Colour>(scene, element, "Colour", () => new Colour());
            var power = loader.LoadObject<double>(scene, element, "Power", () => 1);

            var mat = new Material();

            // get the name
            mat.Name = loader.LoadObject<string>(scene, element, "Name", () => null);

            mat.Ambient = loader.LoadObject<Colour>(scene, element, "Ambient", () => new Colour(0));
            mat.Diffuse = loader.LoadObject<Colour>(scene, element, "Diffuse", () => new Colour(1));

            // specular
        //    mat.Specularity = loader.LoadObject<double>(scene, element, "Specular", () => 1);
         //   mat.SpecularExponent = float.Parse(oText.GetToken(file));

            mat.Specular = loader.LoadObject<Colour>(scene, element, "Specular", () => new Colour(1));
            mat.Reflective = loader.LoadObject<Colour>(scene, element, "Specular", () => new Colour(1));

            // ignore the two things we dont know about
            mat.Refraction = (float)loader.LoadObject<double>(scene, element, "IOR", () => 1.003);

            mat.Transmitted = loader.LoadObject<Colour>(scene, element, "Transmitted", () => new Colour(0));

            return mat;
        }
    }
}
