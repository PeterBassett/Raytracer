using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.FileTypes.VBRayScene;

namespace Raytracer.FileTypes.XMLRayScene.Loaders
{
    [Export(typeof(IXMLRaySceneItemLoader))]
    class DefaultElementLoader : IXMLRaySceneItemLoader
    {
        public string LoaderType { get { return ""; } }
        
        public void LoadObject(XMLRaySceneLoader loader, System.Xml.Linq.XElement element, Scene scene)
        {
            foreach (var child in element.Elements())
	        {
                loader.LoadElement(scene, child);
	        }
        }
    }
}
