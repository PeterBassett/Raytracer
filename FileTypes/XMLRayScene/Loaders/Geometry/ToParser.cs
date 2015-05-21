using System.ComponentModel.Composition;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Transforms
{
    [Export(typeof(XMLRayElementParser))]
    class ToParser : PointTypeParserBase
    {
        public override string LoaderType { get { return "To"; } }
    }
}
