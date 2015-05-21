using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.FileTypes.VBRayScene;
using System.Linq;
using System;
using Raytracer.Rendering.BackgroundMaterials;

namespace Raytracer.FileTypes.XMLRayScene.Loaders
{
    [Export(typeof(XMLRayElementParser))]
    class VerticalCubemapBackgroundParser : XMLRayElementParser
    {
        public override string LoaderType { get { return "VerticalCubemapBackground"; } }

        public override dynamic LoadObject(VBRayScene.XMLRaySceneLoader loader, Rendering.Core.Scene scene, System.Xml.Linq.XElement element, string elementName, Func<dynamic> createDefault)
        {
            return new VerticalCubemapBackground(element.Value);
        }
    }
}
