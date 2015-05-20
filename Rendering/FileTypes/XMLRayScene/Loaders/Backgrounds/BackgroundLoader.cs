using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.FileTypes.VBRayScene;
using System.Linq;
using System;
using Raytracer.Rendering.BackgroundMaterials;

namespace Raytracer.Rendering.FileTypes.XMLRayScene.Loaders
{
    [Export(typeof(IXMLRaySceneItemLoader))]
    class BackgroundLoader : IXMLRaySceneItemLoader
    {
        public string LoaderType { get { return "Background"; } }
        
        public void LoadObject(XMLRaySceneLoader loader, System.Xml.Linq.XElement element, Scene scene)
        {
            if (element.Elements().Count() > 1)
                throw new ArgumentOutOfRangeException("Only one background ca be used");

            var background = loader.LoadObject<IBackgroundMaterial>(scene, element.Elements().First(), () => null);

            if (background != null)
                scene.BackgroundMaterial = background;
        }
    }
}
