using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;
using Raytracer.Properties.Annotations;
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
            
            return GetSampler(samples, renderEdges);
        }

        protected virtual IPixelSampler GetSampler(uint samples, bool renderEdges)
        {
            return new EdgeDetectionSampler(samples, renderEdges);
        }
    }
}