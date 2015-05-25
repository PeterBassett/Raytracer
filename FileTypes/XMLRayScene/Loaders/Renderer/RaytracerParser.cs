using System.ComponentModel.Composition;
using System.Linq;
using System.Xml.Linq;
using Raytracer.Rendering.Cameras;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Renderers;
using Raytracer.Rendering.RenderingStrategies;
using Raytracer.Rendering.PixelSamplers;
using System;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Renderer
{
    [Export(typeof(XmlRayElementParser))]
    class RaytracerParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "Raytracer"; } }
        
        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            return new RayTracingRenderer();
        }
    }
}
