using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.PixelSamplers;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.PixelSamplers
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class VariancePixelSamplerParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "VarianceSampler"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var minimumSamples = loader.LoadObject<uint>(components, element, "MinimumSamples", () => 2);
            var fireflySamples = loader.LoadObject<uint>(components, element, "FireflySamples", () => 256);
            var adaptiveSamples = loader.LoadObject<uint>(components, element, "AdaptiveSamples", () => 16);

            return new VariancePixelSampler(minimumSamples, fireflySamples, adaptiveSamples);
        }
    }
}