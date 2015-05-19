using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.FileTypes.VBRayScene;
using Raytracer.Rendering.Materials;

namespace Raytracer.Rendering.FileTypes.XMLRayScene.Loaders
{
    [Export(typeof(IXMLRaySceneItemLoader))]
    class MaterialsLoader : IXMLRaySceneItemLoader
    {
        public string LoaderType { get { return "Materials"; } }
        
        public void LoadObject(XMLRaySceneLoader loader, System.Xml.Linq.XElement element, Scene scene)
        {
            foreach (var child in element.Elements())
            {
                var material = loader.LoadObject<Material>(scene, child, () => (Material)null);
                if (material != null)
                    scene.AddMaterial(material, material.Name);
            }
        }
    }
}
