using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.PixelSamplers;
using Raytracer.Rendering.RenderingStrategies;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.RenderingStrategies
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class ContinuousRenderingStrategyParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "ContinuousRenderingStrategy"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var renderingStrategy = loader.LoadObject<IRenderingStrategy>(components, element, "RenderingStrategy", () => { throw new ArgumentNullException(); });

            return new ContinuousRenderingStrategy(renderingStrategy, components.Renderer.Settings.MultiThreaded, components.CancellationTokenSource.Token);
        }
    }
}
