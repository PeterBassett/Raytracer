using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;
using Raytracer.MathTypes;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.Cameras;
using Raytracer.Rendering.Core;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Cameras
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class DOFCameraParsers : XmlRayElementParser
    {
        public override string LoaderType { get { return "DOF"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var transform = loader.LoadObject(components, element, "Transform", Transform.CreateIdentityTransform);            

            var fieldOfView = loader.LoadObject<float>(components, element, "FOV", () => 90);
            var focallength = loader.LoadObject<float>(components, element, "FocalLength", () => 5);
            var aperture = loader.LoadObject<float>(components, element, "Aperture", () => 0.1f);

            transform = transform.Invert(transform);
            
            return new Camera(transform, new Size(100, 100), focallength, aperture, fieldOfView);
        }
    }
}
