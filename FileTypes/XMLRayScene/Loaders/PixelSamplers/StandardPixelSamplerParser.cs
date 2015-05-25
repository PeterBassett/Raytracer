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
    class StandardPixelSamplerParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "StandardSampler"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            return new StandardPixelSampler();
        }
    }
}