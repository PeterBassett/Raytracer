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
    class PinholeCameraParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "Pinhole"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var transform = loader.LoadObject(components, element, "Transform", Transform.CreateIdentityTransform);            

            var eyePosition = transform.ToObjectSpace(new Point(0, 0, 0));

            var rotate =  transform.GetObjectSpaceRotation();
            var viewPointRotation = new Vector(MathLib.Rad2Deg(rotate.X),
                                                 MathLib.Rad2Deg(rotate.Y),
                                                 MathLib.Rad2Deg(rotate.Z));

            var fieldOfView = loader.LoadObject<float>(components, element, "FOV", () => 90);

            return new PinholeCamera(eyePosition, -viewPointRotation, new Size(100, 100), fieldOfView);
        }
    }
}
