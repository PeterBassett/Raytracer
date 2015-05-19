using System.ComponentModel.Composition;

namespace Raytracer.Rendering.FileTypes.XMLRayScene.Loaders.Transforms
{
    [Export(typeof(XMLRayElementParser))]
    class FromParser : PointTypeParserBase
    {
        public override string LoaderType { get { return "From"; } }
    }
}
