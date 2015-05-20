using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Raytracer.Rendering.Lights;
using Raytracer.Rendering.Core;
using Raytracer.MathTypes;

namespace Raytracer.Rendering.FileTypes.XMLRayScene.Loaders.Lights
{
    [Export(typeof(XMLRayElementParser))]
    class ProjectionLightParser : XMLRayElementParser
    {
        public override string LoaderType { get { return "ProjectionLight"; } }

        public override dynamic LoadObject(VBRayScene.XMLRaySceneLoader loader, Core.Scene scene, System.Xml.Linq.XElement element, string elementName, Func<dynamic> createDefault)
        {
            var transform = loader.LoadObject<Transform>(scene, element, "Transform", () =>
            {
                return Transform.CreateLookAtTransform(new Point(0, 10, 0), new Point(0, 0, 0), new Vector(0, 0, 1));
            });

            var colour = loader.LoadObject<Colour>(scene, element, "Colour", () => new Colour(1));
            var power = loader.LoadObject<double>(scene, element, "Power", () => 1000);
            var totalWidthInDegrees = loader.LoadObject<double>(scene, element, "Width", () => 45);
            var fallOffWidthInDegrees = loader.LoadObject<double>(scene, element, "FallOffWidth", () => 5);
            var texture = loader.LoadObject<string>(scene, element, "Texture", () => { throw new ArgumentOutOfRangeException("Texture is required"); });

            return new ProjectionLight(colour, ImageReader.Read(texture), (float)power, (float)totalWidthInDegrees, transform);
        }
    }
}
