using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Xml.Linq;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.BackgroundMaterials;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Backgrounds
{
    [Export(typeof(IXmlRaySceneItemLoader)), UsedImplicitly]
    class BackgroundLoader : IXmlRaySceneItemLoader
    {
        public string LoaderType { get { return "Background"; } }
        
        public void LoadObject(XmlRaySceneLoader loader, XElement element, SystemComponents components)
        {
            if (element.Elements().Count() > 1)
                throw new ArgumentOutOfRangeException("loader", "Only one background ca be used");

            var background = loader.LoadObject<IBackgroundMaterial>(components, element.Elements().First(), () => null);

            if (background != null)
                components.Scene.BackgroundMaterial = background;
        }
    }
}
