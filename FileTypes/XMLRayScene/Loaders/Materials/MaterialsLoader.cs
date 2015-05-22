using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.FileTypes.VBRayScene;
using Raytracer.Rendering.Materials;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Materials
{
    [Export(typeof(IXmlRaySceneItemLoader))]
    class MaterialsLoader : IXmlRaySceneItemLoader
    {
        public string LoaderType { get { return "Materials"; } }
        
        public void LoadObject(XmlRaySceneLoader loader, System.Xml.Linq.XElement element, Scene scene)
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
