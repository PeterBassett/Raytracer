using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.FileTypes.VBRayScene;
using System.Linq;
using System.Globalization;

namespace Raytracer.FileTypes.XMLRayScene.Loaders
{
    [Export(typeof(IXMLRaySceneItemLoader))]
    class ConfigurationLoader : IXMLRaySceneItemLoader
    {
        public string LoaderType { get { return "RenderSettings"; } }
        
        public void LoadObject(XMLRaySceneLoader loader, System.Xml.Linq.XElement element, Scene scene)
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
