using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Xml.Linq;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.RenderingStrategies;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.RenderingStrategies
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class RenderingStrategyParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "RenderingStrategy"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var firstChild = element.Elements().FirstOrDefault();

            if (firstChild == null)
                return createDefault();

            return loader.LoadObject<IRenderingStrategy>(components, firstChild, () => createDefault() );
        }
    }
}
