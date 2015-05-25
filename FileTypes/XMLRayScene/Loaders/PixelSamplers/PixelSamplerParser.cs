using System.ComponentModel.Composition;

using Raytracer.Rendering.Core;
using Raytracer.Rendering.Renderers;
using System.Xml.Linq;
using Raytracer.Rendering.Cameras;
using System.Collections.Generic;
using System;
using System.Linq;
using Raytracer.Rendering.PixelSamplers;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Lights
{
    [Export(typeof(XmlRayElementParser))]
    class PixelSamplerParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "PixelSampler"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            return loader.LoadObject<IPixelSampler>(components, element.Elements().FirstOrDefault(), () => new StandardPixelSampler());
        }
    }
}
