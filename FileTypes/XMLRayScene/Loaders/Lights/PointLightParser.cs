using System;
using System.ComponentModel.Composition;
using Raytracer.Rendering.Lights;
using Raytracer.Rendering.Core;

using System.Xml.Linq;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Lights
{
    [Export(typeof(XmlRayElementParser))]
    class PointLightParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "PointLight"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var transform = loader.LoadObject<Transform>(components, element, "Transform", () => Transform.CreateIdentityTransform());
            var colour = loader.LoadObject<Colour>(components, element, "Colour", () => new Colour());
            var power = loader.LoadObject<double>(components, element, "Power", () => 1);

            return new PointLight(colour * power, transform);
        }
    }
}
