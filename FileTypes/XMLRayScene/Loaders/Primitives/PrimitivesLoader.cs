using System.ComponentModel.Composition;
using Raytracer.FileTypes.VBRayScene;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Primitives;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Primitives
{
    [Export(typeof(IXmlRaySceneItemLoader))]
    class PrimitivesLoader : IXmlRaySceneItemLoader
    {
        public string LoaderType { get { return "Primitives"; } }
        
        public void LoadObject(XmlRaySceneLoader loader, System.Xml.Linq.XElement element, Scene scene)
        {
            foreach (var child in element.Elements())
            {
                var primitive = loader.LoadObject<Traceable>(scene, child, () => (Traceable)null);
                if (primitive != null)
                    scene.AddObject(primitive);
            }
        }
    }
}
