using System.ComponentModel.Composition;
using Raytracer.Properties.Annotations;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Geometry
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class FromParser : PointTypeParserBase
    {
        public override string LoaderType { get { return "From"; } }
    }
}
