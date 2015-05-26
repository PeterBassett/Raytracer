using System.ComponentModel.Composition;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.Materials;
using System.Xml.Linq;
using System.Collections.Generic;
using System;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Materials
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
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
