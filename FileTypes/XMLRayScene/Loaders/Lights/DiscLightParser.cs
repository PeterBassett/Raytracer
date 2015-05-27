using System;
using System.ComponentModel.Composition;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.Lights;
using Raytracer.Rendering.Core;
using Raytracer.MathTypes;

using System.Xml.Linq;
using Raytracer.Rendering.Lights.AreaLights;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Lights
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class DiscLightParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "DiscLight"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var transform = loader.LoadObject<Transform>(components, element, "Transform", () =>
            {
                return Transform.CreateLookAtTransform(new Point(0, 10, 0), new Point(0, 0, 0), new Vector(0, 0, 1));
            });

            var colour = loader.LoadObject<Colour>(components, element, "Colour", () => new Colour(1));
            var power = loader.LoadObject<double>(components, element, "Power", () => 1000);
            var samples = loader.LoadObject<int>(components, element, "Samples", () => 64);
            var radius = loader.LoadObject<double>(components, element, "Radius", () => 1);

            return new DiscLight(colour, (float)power, transform, samples, radius);
        }
    }
}
