using System;
using System.ComponentModel.Composition;
using Raytracer.Rendering.Core;

using System.Xml.Linq;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Materials
{
    [Export(typeof(XmlRayElementParser))]
    class SpecularParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "Specular"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var colour = loader.LoadObject<Colour>(components, element, "Colour", () => new Colour(1));
            var specularity = loader.LoadObject<double>(components, element, "Specularity", () => 20);
            var exponent = loader.LoadObject<double>(components, element, "Exponent", () => 0.35);
            
            return new
            {
                Colour = colour,
                Specularity = specularity,
                Exponent = exponent
            };
        }
    }
}
