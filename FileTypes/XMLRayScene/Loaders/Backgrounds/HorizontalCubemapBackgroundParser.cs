using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.BackgroundMaterials;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Backgrounds
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class HorizontalCubemapBackgroundParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "HorizontalCubemapBackground"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            return new HorizontalCubemapBackground(element.Value);
        }
    }
}
