﻿using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Xml.Linq;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.PixelSamplers;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.PixelSamplers
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class PixelSamplerParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "PixelSampler"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            return loader.LoadObject<IPixelSampler>(components, element.Elements().FirstOrDefault(), () => new StandardPixelSampler());
        }
    }
}
