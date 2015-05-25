using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

using Raytracer.Rendering.Renderers;
using System.Xml.Linq;
using Raytracer.Rendering.Cameras;
using Raytracer.FileTypes.XMLRayScene.Extensions;

namespace Raytracer.FileTypes.XMLRayScene.Loaders
{
    [Export(typeof(IXmlRaySceneItemLoader))]
    class ConfigElementLoader : IXmlRaySceneItemLoader
    {
        public string LoaderType { get { return "Config"; } }
        
        public void LoadObject(XmlRaySceneLoader loader, XElement element, SystemComponents components)
        {
            components.scene = loader.LoadObject<Scene>(components, element, "Scene", () => null);
            components.camera = loader.LoadObject<ICamera>(components, element, "Camera", () => null);
            components.renderer = loader.LoadObject<IRenderer>(components, element, "Renderer", () => null);

            components.renderer.Scene = components.scene;
            components.renderer.Camera = components.camera;

            var output = element.ElementCaseInsensitive("Output");
            if(output != null)
                loader.LoadElement(components, output);
        }
    }
}
