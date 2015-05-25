using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

using Raytracer.Rendering.Renderers;
using System.Xml.Linq;
using Raytracer.Rendering.Cameras;

namespace Raytracer.FileTypes.XMLRayScene.Loaders
{
    [Export(typeof(IXmlRaySceneItemLoader))]
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
