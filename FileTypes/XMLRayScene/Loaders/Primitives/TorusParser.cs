using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.FileTypes.VBRayScene;
using System;
using System.Xml.Linq;
using Raytracer.Rendering.Primitives;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Primitives
{
    [Export(typeof(XmlRayElementParser))]
    class TorusParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "Torus"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, Scene scene, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var transform = loader.LoadObject<Transform>(scene, element, "Transform", () => Transform.CreateIdentityTransform());
            var innerRadius = loader.LoadObject<double>(scene, element, "InnerRadius", () => 1);
            var outerRadius = loader.LoadObject<double>(scene, element, "OuterRadius", () => 0.5);
            var materialName = loader.LoadObject<string>(scene, element, "Material", () => null);

            var mat = scene.FindMaterial(materialName);

	        if(mat == null)
		        throw new Exception("Cannot find material '" + materialName + "' for sphere.");

            return new Torus(transform, innerRadius, outerRadius)
            {
                Material = mat
            };
        }
    }
}