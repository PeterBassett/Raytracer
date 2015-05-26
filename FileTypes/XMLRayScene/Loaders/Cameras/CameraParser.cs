using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Xml.Linq;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.Cameras;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Cameras
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
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
