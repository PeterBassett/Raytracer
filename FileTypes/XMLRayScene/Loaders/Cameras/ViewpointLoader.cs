using System.ComponentModel.Composition;
using System.Xml.Linq;
using Raytracer.MathTypes;
using Raytracer.Rendering.Cameras;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Renderers;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Camera
{
    [Export(typeof(XmlRayElementParser))]
    class ViewpointParser : PinholeCameraParser
    {
        public override string LoaderType { get { return "Viewpoint"; } }
    }
}
