using System.ComponentModel.Composition;
using System.Xml.Linq;
using System.Linq;
using Raytracer.Rendering.Cameras;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Renderers;
using System;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Camera
{
    [Export(typeof(XmlRayElementParser))]
    class CameraParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "Camera"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            return loader.LoadObject<ICamera>(components, 
                element.Elements().First(),
                () => { throw new ArgumentNullException("Camera is required"); });

        }
    }
}
