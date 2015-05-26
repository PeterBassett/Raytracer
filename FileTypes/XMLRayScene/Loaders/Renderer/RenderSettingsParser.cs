using System.ComponentModel.Composition;
using System.Xml.Linq;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.Renderers;
using System;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Renderer
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class RenderSettingsParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "RenderSettings"; } }
        
        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var settings = new RenderSettings();

            settings.PathDepth = loader.LoadObject<int>(components, element, "Depth", () => 1);
            settings.TraceShadows = TrueValue(loader.LoadObject<string>(components, element, "Shadows", () => "true"));
            settings.TraceReflections = TrueValue(loader.LoadObject<string>(components, element, "Reflections", () => "true"));
            settings.TraceRefractions = TrueValue(loader.LoadObject<string>(components, element, "Refractions", () => "true"));
            settings.MultiThreaded = TrueValue(loader.LoadObject<string>(components, element, "MultiThreaded", () => "true"));

            return settings;
        }
    }
}
