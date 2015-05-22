using System;
using System.ComponentModel.Composition;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Materials;
using Raytracer.FileTypes.VBRayScene;
using System.Xml.Linq;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Materials
{
    [Export(typeof(XmlRayElementParser))]
    class ColourMaterialParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "ColourMaterial"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, Scene scene, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var mat = new Material();

            // get the name
            mat.Name = loader.LoadObject<string>(scene, element, "Name", () => null);

            mat.Ambient = loader.LoadObject<Colour>(scene, element, "Ambient", () => new Colour(0));
            mat.Diffuse = loader.LoadObject<Colour>(scene, element, "Diffuse", () => new Colour(1));

            var specular = loader.LoadObject<dynamic>(scene, element, "Specular", () => new { Specularity = 20.0, Exponent = 0.35, Colour = new Colour(1) });

            mat.Specularity = (float)specular.Specularity;
            mat.SpecularExponent = (float)specular.Exponent;
            mat.Specular = specular.Colour;

            mat.Reflective = loader.LoadObject<Colour>(scene, element, "Reflected", () => new Colour(0));
            mat.Refraction = (float)loader.LoadObject<double>(scene, element, "IOR", () => 1.003);
            mat.Transmitted = loader.LoadObject<Colour>(scene, element, "Transmitted", () => new Colour(0));

            return mat;
        }
    }
}
