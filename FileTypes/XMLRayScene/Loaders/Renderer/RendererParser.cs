﻿using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Xml.Linq;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.PixelSamplers;
using Raytracer.Rendering.Renderers;
using Raytracer.Rendering.RenderingStrategies;
using Raytracer.Rendering.Distributions;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Renderer
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
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

            components.Renderer = loader.LoadObject<IRenderer>(components, 
                                            element.Elements().First(),
                                            () => new RayTracingRenderer(1));

            components.Renderer.Settings = settings;

            var renderingStrategy = loader.LoadObject<IRenderingStrategy>(components, element, "RenderingStrategy", () =>
            {
                return new ProgressiveRenderingStrategy(new StandardPixelSampler(),
                                                         64,
                                                         true,
                                                         components.CancellationTokenSource.Token);
            });

            components.Renderer.RenderingStrategy = renderingStrategy;

            var distribution = loader.LoadObject<Distribution>(components, element, "Distribution", () => new RandomDistribution() );

            components.Renderer.Distribution = distribution;

            return components.Renderer;
        }
    }
}
