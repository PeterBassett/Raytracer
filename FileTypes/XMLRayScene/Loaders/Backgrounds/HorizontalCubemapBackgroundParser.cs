using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;
using Raytracer.FileTypes.VBRayScene;
using Raytracer.Rendering.BackgroundMaterials;
using Raytracer.Rendering.Core;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Backgrounds
{
    [Export(typeof(XmlRayElementParser))]
    class HorizontalCubemapBackgroundParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "HorizontalCubemapBackground"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, Scene scene, XElement element, string elementName, Func<dynamic> createDefault)
        {
            return new HorizontalCubemapBackground(element.Value);
        }
    }
}
