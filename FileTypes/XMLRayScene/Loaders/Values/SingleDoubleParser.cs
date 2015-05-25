using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using System.Xml.Linq;
using System;


namespace Raytracer.FileTypes.XMLRayScene.Loaders.Values
{
    [Export(typeof(XmlRayElementParser))]
    abstract class SingleDoubleParser : XmlRayElementParser
    {
        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var attr = GetDouble(element, LoaderType);

            if(attr.HasValue)
                return attr.Value;

            if (!string.IsNullOrEmpty(element.Value))
                return double.Parse(element.Value);

            return createDefault();
        }
    }
}
