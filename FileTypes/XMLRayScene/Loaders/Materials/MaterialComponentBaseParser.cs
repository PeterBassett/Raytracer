using System;
using Raytracer.Rendering.Core;
using Raytracer.FileTypes.VBRayScene;
using System.Xml.Linq;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Materials
{
    abstract class MaterialComponentBaseParser : XmlRayElementParser
    {
        public override dynamic LoadObject(XmlRaySceneLoader loader, Scene scene, XElement element, string elementName, Func<dynamic> createDefault)
        {
            return loader.LoadObject<Colour>(scene, element, "Colour", () => createDefault());
        }
    }
}
