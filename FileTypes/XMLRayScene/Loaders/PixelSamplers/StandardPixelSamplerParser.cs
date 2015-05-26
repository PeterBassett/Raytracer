using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.PixelSamplers;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.PixelSamplers
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class StandardPixelSamplerParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "StandardSampler"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            return new StandardPixelSampler();
        }
    }
}