using System.ComponentModel.Composition;
using Raytracer.Rendering.Core;
using Raytracer.FileTypes.VBRayScene;
using System.Xml.Linq;
using System;

namespace Raytracer.FileTypes.XMLRayScene.Loaders
{
    [Export(typeof(XmlRayElementParser))]
    abstract class XmlRayElementParser
    {
        public abstract string LoaderType { get; }

        public abstract dynamic LoadObject(XmlRaySceneLoader loader, Scene scene, XElement element, string elementName, Func<dynamic> createDefault);
        
        public double GetDouble(XElement element, string valueName, Func<double> createDefault)
        {
            var value = GetDouble(element, valueName);

            return value.HasValue ? value.Value : createDefault();
        }

        public double? GetDouble(XElement element, string valueName)
        {
            var attribute = element.Attribute(valueName);
            if (attribute != null)
                return double.Parse(attribute.Value);

            return null;
        }
    }
}
