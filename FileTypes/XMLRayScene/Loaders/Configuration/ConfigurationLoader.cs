using System.ComponentModel.Composition;
using System.Linq;
using System.Xml.Linq;
using Raytracer.FileTypes.VBRayScene;
using Raytracer.Rendering.Core;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Configuration
{
    [Export(typeof(IXmlRaySceneItemLoader))]
    class ConfigurationLoader : IXmlRaySceneItemLoader
    {
        public string LoaderType { get { return "RenderSettings"; } }
        
        public void LoadObject(XmlRaySceneLoader loader, XElement element, Scene scene)
        {
            scene.RecursionDepth = loader.LoadObject<int>(scene, element, "Depth", () => 1);

            scene.TraceShadows =     TrueValue(loader.LoadObject<string>(scene, element, "Shadows", () => "true"));
            scene.TraceReflections = TrueValue(loader.LoadObject<string>(scene, element, "Reflections", () => "true"));
            scene.TraceRefractions = TrueValue(loader.LoadObject<string>(scene, element, "Refractions", () => "true"));
        }

        private bool TrueValue(string value)
        {
            var truthyValues = new [] { "true", "1" };

            return truthyValues.Contains(value.ToLowerInvariant());
        }
    }
}
