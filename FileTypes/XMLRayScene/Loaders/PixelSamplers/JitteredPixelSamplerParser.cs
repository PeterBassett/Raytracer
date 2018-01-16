using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.Distributions;
using Raytracer.Rendering.PixelSamplers;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.PixelSamplers
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class JitteredPixelSamplerParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "JitteredSampler"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var samples = loader.LoadObject<uint>(components, element, "Samples", () => 4);
            var distribution = loader.LoadObject<Distribution>(components, element, "Distribution", () => new RandomDistribution());
            return new JitteredPixelSampler(distribution, samples);
        }
    }
}