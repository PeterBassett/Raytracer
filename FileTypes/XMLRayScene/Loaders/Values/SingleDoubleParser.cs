using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.FileTypes.VBRayScene;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Values
{
    [Export(typeof(XmlRayElementParser))]
    abstract class SingleDoubleParser : XmlRayElementParser
    {
        public override dynamic LoadObject(XmlRaySceneLoader loader, Scene scene, System.Xml.Linq.XElement element, string elementName, System.Func<dynamic> createDefault)
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
