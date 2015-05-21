using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.FileTypes.VBRayScene;

namespace Raytracer.FileTypes.XMLRayScene.Loaders
{
    [Export(typeof(IXMLRaySceneItemLoader))]
    class LightsLoader : IXMLRaySceneItemLoader
    {
        public string LoaderType { get { return "Lights"; } }
        
        public void LoadObject(XMLRaySceneLoader loader, System.Xml.Linq.XElement element, Scene scene)
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
