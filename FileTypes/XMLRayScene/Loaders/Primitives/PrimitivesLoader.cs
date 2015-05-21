using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.FileTypes.VBRayScene;
using Raytracer.Rendering.Primitives;

namespace Raytracer.FileTypes.XMLRayScene.Loaders
{
    [Export(typeof(IXMLRaySceneItemLoader))]
    class PrimitivesLoader : IXMLRaySceneItemLoader
    {
        public string LoaderType { get { return "Primitives"; } }
        
        public void LoadObject(XMLRaySceneLoader loader, System.Xml.Linq.XElement element, Scene scene)
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
