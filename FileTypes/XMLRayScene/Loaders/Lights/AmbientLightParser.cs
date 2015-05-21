using System;
using System.ComponentModel.Composition;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Lights;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Lights
{
    [Export(typeof(XMLRayElementParser))]
    class AmbientLightParser : XMLRayElementParser
    {
        public override string LoaderType { get { return "AmbientLight"; } }

        public override dynamic LoadObject(VBRayScene.XMLRaySceneLoader loader, Rendering.Core.Scene scene, System.Xml.Linq.XElement element, string elementName, Func<dynamic> createDefault)
        {
            var colour = loader.LoadObject<Colour>(scene, element, "Colour", () => new Colour());
            var power = loader.LoadObject<double>(scene, element, "Power", () => 1);

            return new AmbientLight(colour * power);
        }
    }
}
