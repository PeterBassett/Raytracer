using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

using Raytracer.Rendering.Renderers;
using System.Xml.Linq;
using Raytracer.Rendering.Cameras;
using System.Collections.Generic;
using System.Linq;
using Raytracer.Rendering.Materials;
using Raytracer.Rendering.Primitives;
using System;

namespace Raytracer.FileTypes.XMLRayScene.Loaders
{
    [Export(typeof(XmlRayElementParser))]
    class SceneParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "Scene"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            if (components.scene == null)
                components.scene = new Scene();

            var lights = loader.LoadObject<IEnumerable<Light>>(components, element, "Lights", () => Enumerable.Empty<Light>());
            var materials = loader.LoadObject<IEnumerable<Material>>(components, element, "Materials", () => Enumerable.Empty<Material>());
            var primitives = loader.LoadObject<IEnumerable<Traceable>>(components, element, "Primitives", () => Enumerable.Empty<Traceable>());

            foreach (var light in lights)
                components.scene.AddLight(light);
            /*
            foreach (var material in materials)
                scene.AddMaterial(material, material.Name);
            */
            foreach (var primitive in primitives)
                components.scene.AddObject(primitive);

            return components.scene;
        }
    }
}
