using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

using Raytracer.Rendering.Materials;
using Raytracer.Rendering.Renderers;
using System.Xml.Linq;
using Raytracer.Rendering.Cameras;
using System.Collections.Generic;
using System;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Materials
{
    [Export(typeof(XmlRayElementParser))]
    class MaterialsParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "Materials"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var materials = new List<Material>();
            foreach (var child in element.Elements())
            {
                var material = loader.LoadObject<Material>(components, child, () => (Material)null);
                if (material != null)
                {
                    components.scene.AddMaterial(material, material.Name);
                    materials.Add(material);
                }
            }

            return materials;
        }
    }
}
