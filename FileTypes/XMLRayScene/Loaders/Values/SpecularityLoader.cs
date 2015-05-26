using System.ComponentModel.Composition;


namespace Raytracer.FileTypes.XMLRayScene.Loaders.Values
{
    [Export(typeof(XmlRayElementParser)), 
]
    class SpecularityLoader : SingleDoubleParser
    {
        public override string LoaderType { get { return "Specularity"; } }
    }
}
