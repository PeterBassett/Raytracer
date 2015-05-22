using System.ComponentModel.Composition;
using Raytracer.FileTypes.VBRayScene;
using Raytracer.Rendering.Core;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Lights
{
    [Export(typeof(IXmlRaySceneItemLoader))]
    class LightsLoader : IXmlRaySceneItemLoader
    {
        public string LoaderType { get { return "Lights"; } }
        
        public void LoadObject(XmlRaySceneLoader loader, System.Xml.Linq.XElement element, Scene scene)
        {
            foreach (var child in element.Elements())
            {
                var light = loader.LoadObject<Light>(scene, child, () => (Light)null);
                if (light != null)
                    scene.AddLight(light);
            }
        }
    }
}
