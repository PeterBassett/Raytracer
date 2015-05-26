using System.ComponentModel.Composition;
using System.Xml.Linq;
using System;
using Raytracer.Properties.Annotations;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Values
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
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
