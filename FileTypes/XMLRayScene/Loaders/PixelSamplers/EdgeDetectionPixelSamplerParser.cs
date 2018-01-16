using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.Distributions;
using Raytracer.Rendering.PixelSamplers;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.PixelSamplers
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class EdgeDetectionPixelSamplerParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "EdgeDetectionSampler"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var samples = loader.LoadObject<uint>(components, element, "Samples", () => 4);
            var renderEdges = TrueValue(loader.LoadObject<string>(components, element, "RenderEdges", () => "false"));
            var distribution = loader.LoadObject<Distribution>(components, element, "Distribution", () => new RandomDistribution());

            return GetSampler(distribution, samples, renderEdges);
        }

        protected virtual IPixelSampler GetSampler(Distribution distribution, uint samples, bool renderEdges)
        {
            return new EdgeDetectionSampler(distribution, samples, renderEdges);
        }
    }
}