using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.FileTypes.VBRayScene;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Values
{
    [Export(typeof(XMLRayElementParser))]
    class SpecularityLoader : SingleDoubleParser
    {
        public override string LoaderType { get { return "Specularity"; } }
    }
}
