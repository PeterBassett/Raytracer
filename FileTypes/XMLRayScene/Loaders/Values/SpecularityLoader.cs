using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;


namespace Raytracer.FileTypes.XMLRayScene.Loaders.Values
{
    [Export(typeof(XmlRayElementParser))]
    class SpecularityLoader : SingleDoubleParser
    {
        public override string LoaderType { get { return "Specularity"; } }
    }
}
