using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.PixelSamplers;
using Raytracer.Rendering.RenderingStrategies;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.RenderingStrategies
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class ProgressiveRenderingStrategyParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "ProgressiveRenderingStrategy"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var sampler = loader.LoadObject<IPixelSampler>(components, element, "PixelSampler", () => 
            {
                return new StandardPixelSampler();
            });

            return new ProgressiveRenderingStrategy(sampler, 64, components.Renderer.Settings.MultiThreaded, components.CancellationTokenSource.Token);
        }
    }
}
