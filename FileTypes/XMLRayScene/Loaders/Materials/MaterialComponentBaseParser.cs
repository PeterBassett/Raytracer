using System;
using Raytracer.Rendering.Core;

using System.Xml.Linq;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Materials
{
    abstract class MaterialComponentBaseParser : XmlRayElementParser
    {
        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            return loader.LoadObject<Colour>(components, element, "Colour", () => createDefault());
        }
    }
}
