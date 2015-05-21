using System.ComponentModel.Composition;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Transforms
{
    [Export(typeof(XMLRayElementParser))]
    class UpParser : PointTypeParserBase
    {
        public override string LoaderType { get { return "Up"; } }
    }
}
