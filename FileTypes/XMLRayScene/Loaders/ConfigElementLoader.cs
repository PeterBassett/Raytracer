using System.ComponentModel.Composition;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.Core;

using Raytracer.Rendering.Renderers;
using System.Xml.Linq;
using Raytracer.Rendering.Cameras;
using Raytracer.FileTypes.XMLRayScene.Extensions;

namespace Raytracer.FileTypes.XMLRayScene.Loaders
{
    [Export(typeof(IXmlRaySceneItemLoader)), UsedImplicitly]
    class ConfigElementLoader : IXmlRaySceneItemLoader
    {
        public string LoaderType { get { return "Config"; } }
        
        public void LoadObject(XmlRaySceneLoader loader, XElement element, SystemComponents components)
        {
            components.Scene = loader.LoadObject<Scene>(components, element, "Scene", () => null);
            components.Camera = loader.LoadObject<ICamera>(components, element, "Camera", () => null);
            components.Renderer = loader.LoadObject<IRenderer>(components, element, "Renderer", () => null);

            components.Renderer.Scene = components.Scene;
            components.Renderer.Camera = components.Camera;

            var output = element.ElementCaseInsensitive("Output");
            if(output != null)
                loader.LoadElement(components, output);
        }
    }
}
