using System.ComponentModel.Composition;
using System.Xml.Linq;
using System;
using Raytracer.Properties.Annotations;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Geometry
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class SolidityParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "Solidity"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            if (!string.IsNullOrEmpty(element.Value))
                return element.Value;

            return createDefault();
        }
    }
}
