using System;
using System.ComponentModel.Composition;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.Lights;
using Raytracer.Rendering.Core;
using Raytracer.MathTypes;

using System.Xml.Linq;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Lights
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class DistantLightParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "DistantLight"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var transform = loader.LoadObject(components, element, "Transform", () => Transform.CreateLookAtTransform(new Point(0, 10, 0), new Point(0, 0, 0), new Vector(0, 0, 1)));

            var colour = loader.LoadObject(components, element, "Colour", () => new Colour(1));
            var power = loader.LoadObject<double>(components, element, "Power", () => 1000);

            return new DistantLight(colour, power, transform);
        }
    }
}
