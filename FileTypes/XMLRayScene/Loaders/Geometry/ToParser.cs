using System.ComponentModel.Composition;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Geometry
{
    [Export(typeof(XmlRayElementParser))]
    class ToParser : PointTypeParserBase
    {
        public override string LoaderType { get { return "To"; } }
    }
}
