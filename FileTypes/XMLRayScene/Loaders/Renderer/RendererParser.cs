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
    class RendererParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "Renderer"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var settings = loader.LoadObject<RenderSettings>(components, element, "RenderSettings", () => new RenderSettings()
            {
                PathDepth = 2,
                TraceReflections = true,
                TraceShadows = true,
                TraceRefractions = true,
                MultiThreaded = true
            });

            components.renderer = loader.LoadObject<IRenderer>(components, 
                                            element.Elements().First(),
                                            () => new RayTracingRenderer());

            components.renderer.Settings = settings;

            var renderingStrategy = loader.LoadObject<IRenderingStrategy>(components, element, "RenderingStrategy", () =>
            {
                return new ProgressiveRenderingStrategy(new StandardPixelSampler(),
                                                         64,
                                                         true,
                                                         System.Threading.CancellationToken.None);
            });

            components.renderer.RenderingStrategy = renderingStrategy;

            return components.renderer;
        }
    }
}
