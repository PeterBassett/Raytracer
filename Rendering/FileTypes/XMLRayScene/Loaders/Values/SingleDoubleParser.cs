using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.FileTypes.VBRayScene;

namespace Raytracer.Rendering.FileTypes.XMLRayScene.Loaders.Values
{
    [Export(typeof(XMLRayElementParser))]
    abstract class SingleDoubleParser : XMLRayElementParser
    {
        public override dynamic LoadObject(XMLRaySceneLoader loader, Scene scene, System.Xml.Linq.XElement element, string elementName, System.Func<dynamic> createDefault)
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
