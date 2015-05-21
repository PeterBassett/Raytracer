using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Raytracer.Rendering.Lights;
using Raytracer.Rendering.Core;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Lights
{
    [Export(typeof(XMLRayElementParser))]
    class PointLightParser : XMLRayElementParser
    {
        public override string LoaderType { get { return "PointLight"; } }

        public override dynamic LoadObject(VBRayScene.XMLRaySceneLoader loader, Rendering.Core.Scene scene, System.Xml.Linq.XElement element, string elementName, Func<dynamic> createDefault)
        {
            var transform = loader.LoadObject<Transform>(scene, element, "Transform", () => Transform.CreateIdentityTransform());            
            var colour = loader.LoadObject<Colour>(scene, element, "Colour", () => new Colour());
            var power = loader.LoadObject<double>(scene, element, "Power", () => 1);

            return new PointLight(colour * power, transform);
        }
    }
}
