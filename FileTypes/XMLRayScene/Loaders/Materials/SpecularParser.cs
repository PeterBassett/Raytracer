using System;
using System.ComponentModel.Composition;
using Raytracer.Rendering.Core;
using Raytracer.FileTypes.VBRayScene;
using System.Xml.Linq;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Materials
{
    [Export(typeof(XmlRayElementParser))]
    class SpecularParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "Specular"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, Scene scene, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var colour = loader.LoadObject<Colour>(scene, element, "Colour", () => new Colour(1));
            var specularity = loader.LoadObject<double>(scene, element, "Specularity", () => 20);
            var exponent = loader.LoadObject<double>(scene, element, "Exponent", () => 0.35);
            
            return new
            {
                Colour = colour,
                Specularity = specularity,
                Exponent = exponent
            };
        }
    }
}
