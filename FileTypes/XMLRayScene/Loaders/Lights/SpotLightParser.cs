using System;
using System.ComponentModel.Composition;
using Raytracer.Rendering.Lights;
using Raytracer.Rendering.Core;
using Raytracer.MathTypes;
using Raytracer.FileTypes.VBRayScene;
using System.Xml.Linq;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Lights
{
    [Export(typeof(XmlRayElementParser))]
    class SpotLightParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "SpotLight"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, Scene scene, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var transform = loader.LoadObject<Transform>(scene, element, "Transform", () =>
            {
                return Transform.CreateLookAtTransform(new Point(0, 10, 0), new Point(0, 0, 0), new Vector(0, 0, 1));
            });

            var colour = loader.LoadObject<Colour>(scene, element, "Colour", () => new Colour(1));
            var power = loader.LoadObject<double>(scene, element, "Power", () => 1000);
            var totalWidthInDegrees = loader.LoadObject<double>(scene, element, "Width", () => 45);
            var fallOffWidthInDegrees = loader.LoadObject<double>(scene, element, "FallOffWidth", () => 5);

            return new SpotLight(colour, (float)power, (float)totalWidthInDegrees, (float)totalWidthInDegrees - (float)fallOffWidthInDegrees, transform);
        }
    }
}
