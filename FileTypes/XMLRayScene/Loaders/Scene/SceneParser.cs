using System.ComponentModel.Composition;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.BackgroundMaterials;
using Raytracer.Rendering.Core;
using System.Xml.Linq;
using System.Linq;
using Raytracer.Rendering.Materials;
using System;

// ReSharper disable once CheckNamespace
namespace Raytracer.FileTypes.XMLRayScene.Loaders.SceneLoader /* note not Scene namespace as this would conlict with the Scene class */
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class SceneParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "Scene"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            if (components.Scene == null)
                components.Scene = new Scene();

            var background = loader.LoadObject<IBackgroundMaterial>(components, element, "Background", null);

            if (background != null)
                components.Scene.BackgroundMaterial = background;

            var lights = loader.LoadObject(components, element, "Lights", Enumerable.Empty<Light>);
            loader.LoadObject(components, element, "Materials", Enumerable.Empty<Material>);
            var primitives = loader.LoadObject(components, element, "Primitives", Enumerable.Empty<Traceable>);

            foreach (var light in lights)
                components.Scene.AddLight(light);
            /*
            foreach (var material in materials)
                scene.AddMaterial(material, material.Name);
            */
            foreach (var primitive in primitives)
                components.Scene.AddObject(primitive);

            return components.Scene;
        }
    }
}
