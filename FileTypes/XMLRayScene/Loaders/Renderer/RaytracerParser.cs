﻿using System.ComponentModel.Composition;
using System.Xml.Linq;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.Renderers;
using System;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Renderer
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class RaytracerParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "Raytracer"; } }
        
        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            return new RayTracingRenderer();
        }
    }
}