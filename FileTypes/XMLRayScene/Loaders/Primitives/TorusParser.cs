using System.ComponentModel.Composition;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.Core;

using System;
using System.Xml.Linq;
using Raytracer.Rendering.Primitives;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Primitives
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class TorusParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "Torus"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var transform = loader.LoadObject(components, element, "Transform", Transform.CreateIdentityTransform);
            var innerRadius = loader.LoadObject<double>(components, element, "InnerRadius", () => 1);
            var outerRadius = loader.LoadObject<double>(components, element, "OuterRadius", () => 0.5);
            var materialName = loader.LoadObject<string>(components, element, "Material", () => null);

            var mat = components.scene.FindMaterial(materialName);

	        if(mat == null)
		        throw new Exception("Cannot find material '" + materialName + "' for sphere.");

            return new Torus(transform, innerRadius, outerRadius)
            {
                Material = mat
            };
        }
    }
}