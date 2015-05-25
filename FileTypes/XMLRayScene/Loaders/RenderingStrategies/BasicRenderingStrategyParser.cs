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
    class BasicRenderingStrategyParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "BasicRenderingStrategy"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var sampler = loader.LoadObject<IPixelSampler>(components, element, "PixelSampler", () => 
            {
                return new StandardPixelSampler();
            });

            if(components.cancellationTokenSource == null)
                components.cancellationTokenSource = new System.Threading.CancellationTokenSource();

            return new BasicRenderingStrategy(sampler, components.renderer.Settings.MultiThreaded, components.cancellationTokenSource.Token);
        }
    }
}
