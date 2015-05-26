using System.ComponentModel.Composition;
using Raytracer.Properties.Annotations;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Geometry
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class ToParser : PointTypeParserBase
    {
        public override string LoaderType { get { return "To"; } }
    }
}
