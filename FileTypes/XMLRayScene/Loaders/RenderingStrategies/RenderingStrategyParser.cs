using System.ComponentModel.Composition;
using System.Xml.Linq;
using System.Linq;
using Raytracer.Rendering.Cameras;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Renderers;
using System;
using Raytracer.Rendering.RenderingStrategies;
using Raytracer.Rendering.PixelSamplers;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Camera
{
    [Export(typeof(XmlRayElementParser))]
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
