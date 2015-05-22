using System;
using System.ComponentModel.Composition;
using System.Linq;
using Raytracer.FileTypes.VBRayScene;
using Raytracer.Rendering.BackgroundMaterials;
using Raytracer.Rendering.Core;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Backgrounds
{
    [Export(typeof(IXmlRaySceneItemLoader))]
    class BackgroundLoader : IXmlRaySceneItemLoader
    {
        public string LoaderType { get { return "Background"; } }
        
        public void LoadObject(XmlRaySceneLoader loader, System.Xml.Linq.XElement element, Scene scene)
        {
            if (element.Elements().Count() > 1)
                throw new ArgumentOutOfRangeException("loader", "Only one background ca be used");

            var background = loader.LoadObject<IBackgroundMaterial>(scene, element.Elements().First(), () => null);

            if (background != null)
                scene.BackgroundMaterial = background;
        }
    }
}
