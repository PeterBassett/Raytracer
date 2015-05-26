using System.ComponentModel.Composition;
using Raytracer.Properties.Annotations;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Cameras
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class ViewpointParser : PinholeCameraParser
    {
        public override string LoaderType { get { return "Viewpoint"; } }
    }
}
