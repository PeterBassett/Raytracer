﻿using System.ComponentModel.Composition;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.Distributions;
using Raytracer.Rendering.PixelSamplers;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.PixelSamplers
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class EdgeDetectionPerComponentPixelSamplerParser : EdgeDetectionPixelSamplerParser
    {
        public override string LoaderType { get { return "EdgeDetectionPerComponentSampler"; } }

        protected override IPixelSampler GetSampler(Distribution distribution, uint samples, bool renderEdges)
        {
            return new EdgeDetectionPerComponentSampler(distribution, samples, renderEdges);
        }
    }
}