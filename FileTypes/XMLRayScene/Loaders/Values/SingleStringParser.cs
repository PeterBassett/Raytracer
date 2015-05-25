using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using System.Xml.Linq;
using System;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Values
{
    [Export(typeof(XmlRayElementParser))]
    abstract class SingleStringParser : XmlRayElementParser
    {
        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
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
