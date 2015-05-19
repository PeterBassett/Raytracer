using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.FileTypes.VBRayScene;
using System.Xml.Linq;
using System;

namespace Raytracer.Rendering.FileTypes.XMLRayScene.Loaders
{
    [Export(typeof(XMLRayElementParser))]
    abstract class XMLRayElementParser
    {
        public abstract string LoaderType { get; }

        public abstract dynamic LoadObject(XMLRaySceneLoader loader, Scene scene, XElement element, string elementName, Func<dynamic> createDefault);
        
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
