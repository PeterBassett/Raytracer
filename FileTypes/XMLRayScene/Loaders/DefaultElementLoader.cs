using System.ComponentModel.Composition;
using System.Xml.Linq;
using Raytracer.Properties.Annotations;

namespace Raytracer.FileTypes.XMLRayScene.Loaders
{
    [Export(typeof(IXmlRaySceneItemLoader)), UsedImplicitly]
    class DefaultElementLoader : IXmlRaySceneItemLoader
    {
        public string LoaderType { get { return ""; } }
        
        public void LoadObject(XmlRaySceneLoader loader, XElement element, SystemComponents components)
        {
            foreach (var child in element.Elements())
	        {
                loader.LoadElement(components, child);
	        }
        }
    }
}
