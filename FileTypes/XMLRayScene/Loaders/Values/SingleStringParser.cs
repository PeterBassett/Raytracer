using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.FileTypes.VBRayScene;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Values
{
    [Export(typeof(XMLRayElementParser))]
    abstract class SingleStringParser : XMLRayElementParser
    {
        public override dynamic LoadObject(XMLRaySceneLoader loader, Scene scene, System.Xml.Linq.XElement element, string elementName, System.Func<dynamic> createDefault)
        {
            var attribute = element.Attribute(elementName);
            if (attribute != null)
                return attribute.Value;

            if (!string.IsNullOrEmpty(element.Value))
                return element.Value;

            return createDefault();
        }
    }
}
