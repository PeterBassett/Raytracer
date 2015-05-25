using System.ComponentModel.Composition;
using Raytracer.Rendering.PixelSamplers;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Primitives
{
    [Export(typeof(XmlRayElementParser))]
    class EdgeDetectionPerComponentPixelSamplerParser : EdgeDetectionPixelSamplerParser
    {
        public override string LoaderType { get { return "EdgeDetectionPerComponentSampler"; } }

        protected override IPixelSampler GetSampler(uint samples, bool renderEdges)
        {
            return new EdgeDetectionPerComponentSampler(samples, renderEdges);
        }
    }
}