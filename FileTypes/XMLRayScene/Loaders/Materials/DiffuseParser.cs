using System.ComponentModel.Composition;
using Raytracer.Properties.Annotations;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Materials
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class DiffuseParser : MaterialComponentBaseParser
    {
        public override string LoaderType { get { return "Diffuse"; } }
    }
}
