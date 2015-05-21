using System;
using Raytracer.Rendering.Core;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Materials
{
    abstract class MaterialComponentBaseParser : XMLRayElementParser
    {
        public override dynamic LoadObject(VBRayScene.XMLRaySceneLoader loader, Rendering.Core.Scene scene, System.Xml.Linq.XElement element, string elementName, Func<dynamic> createDefault)
        {
            return loader.LoadObject<Colour>(scene, element, "Colour", () => createDefault());
        }
    }
}
