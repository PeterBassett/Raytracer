using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Raytracer.Rendering.Lights;

namespace Raytracer.Rendering.FileTypes.XMLRayScene.Loaders.Materials
{
    abstract class MaterialComponentBaseParser : XMLRayElementParser
    {
        public override dynamic LoadObject(VBRayScene.XMLRaySceneLoader loader, Core.Scene scene, System.Xml.Linq.XElement element, string elementName, Func<dynamic> createDefault)
        {
            return loader.LoadObject<Colour>(scene, element, "Colour", () => createDefault());
        }
    }
}
