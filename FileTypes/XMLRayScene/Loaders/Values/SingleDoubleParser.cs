using System.ComponentModel.Composition;
using System.Xml.Linq;
using System;
using Raytracer.Properties.Annotations;


namespace Raytracer.FileTypes.XMLRayScene.Loaders.Values
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
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
