using System;
using System.ComponentModel.Composition;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Materials;

using System.Xml.Linq;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Materials
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class ColourMaterialParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "ColourMaterial"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var mat = new Material();

            // get the name
            mat.Name = loader.LoadObject<string>(components, element, "Name", () => null);

            mat.Ambient = loader.LoadObject<Colour>(components, element, "Ambient", () => new Colour(0));
            mat.Diffuse = loader.LoadObject<Colour>(components, element, "Diffuse", () => new Colour(1));

            var specular = loader.LoadObject<dynamic>(components, element, "Specular", () => new { Specularity = 20.0, Exponent = 0.35, Colour = new Colour(1) });

            mat.Specularity = (float)specular.Specularity;
            mat.SpecularExponent = (float)specular.Exponent;
            mat.Specular = specular.Colour;

            mat.Reflective = loader.LoadObject<Colour>(components, element, "Reflected", () => new Colour(0));
            mat.Refraction = (float)loader.LoadObject<double>(components, element, "IOR", () => 1.003);
            mat.Transmitted = loader.LoadObject<Colour>(components, element, "Transmitted", () => new Colour(0));

            mat.Density = loader.LoadObject<float>(components, element, "Density", () => 0);

            return mat;
        }
    }
}
