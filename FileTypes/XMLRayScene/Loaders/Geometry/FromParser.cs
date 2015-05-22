using System.ComponentModel.Composition;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Geometry
{
    [Export(typeof(XmlRayElementParser))]
    class FromParser : PointTypeParserBase
    {
        public override string LoaderType { get { return "From"; } }
    }
}
