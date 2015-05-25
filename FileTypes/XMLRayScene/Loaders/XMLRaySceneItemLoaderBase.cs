using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Xml.Linq;

namespace Raytracer.FileTypes.XMLRayScene.Loaders
{
    [Export(typeof(XmlRayElementParser))]
    abstract class XmlRayElementParser
    {
        public abstract string LoaderType { get; }

        public abstract dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault);
        
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

        public int? GetInt(XElement element, string valueName)
        {
            var attribute = element.Attribute(valueName);
            if (attribute != null)
                return int.Parse(attribute.Value);

            return null;
        }

        protected bool TrueValue(string value)
        {
            var truthyValues = new[] { "true", "1", "on" };

            return truthyValues.Contains(value.ToLowerInvariant());
        }
    }
}
