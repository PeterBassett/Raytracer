using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

using System;
using System.Xml.Linq;
using Raytracer.Rendering.Primitives;
using Raytracer.Rendering.PixelSamplers;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Primitives
{
    [Export(typeof(XmlRayElementParser))]
    class JitteredPixelSamplerParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "JitteredSampler"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var samples = loader.LoadObject<uint>(components, element, "Samples", () => 4);
            return new JitteredPixelSampler(samples);
        }
    }
}