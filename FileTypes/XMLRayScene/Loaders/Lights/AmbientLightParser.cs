using System;
using System.ComponentModel.Composition;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Lights;

using System.Xml.Linq;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Lights
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class AmbientLightParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "AmbientLight"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var colour = loader.LoadObject<Colour>(components, element, "Colour", () => new Colour());
            var power = loader.LoadObject<double>(components, element, "Power", () => 1);

            return new AmbientLight(colour * power);
        }
    }
}
