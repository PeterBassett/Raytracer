using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.FileTypes.VBRayScene;

namespace Raytracer.FileTypes.XMLRayScene.Loaders
{
    [Export(typeof(IXmlRaySceneItemLoader))]
    class DefaultElementLoader : IXmlRaySceneItemLoader
    {
        public string LoaderType { get { return ""; } }
        
        public void LoadObject(XmlRaySceneLoader loader, System.Xml.Linq.XElement element, Scene scene)
        {
            foreach (var child in element.Elements())
	        {
                loader.LoadElement(scene, child);
	        }
        }
    }
}
