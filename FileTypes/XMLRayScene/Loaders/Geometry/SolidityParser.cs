using System.ComponentModel.Composition;

using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using System.Xml.Linq;
using System;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Geometry
{
    [Export(typeof(XmlRayElementParser))]
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
