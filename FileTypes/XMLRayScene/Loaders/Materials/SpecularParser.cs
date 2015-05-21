using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Raytracer.Rendering.Lights;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Materials
{
    [Export(typeof(XMLRayElementParser))]
    class SpecularParser : XMLRayElementParser
    {
        public override string LoaderType { get { return "Specular"; } }

        public override dynamic LoadObject(VBRayScene.XMLRaySceneLoader loader, Rendering.Core.Scene scene, System.Xml.Linq.XElement element, string elementName, Func<dynamic> createDefault)
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
