using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Primitives;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Primitives
{
    [Export(typeof(XmlRayElementParser))]
    class DiscParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "Disc"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var transform = loader.LoadObject<Transform>(components, element, "Transform", () => Transform.CreateIdentityTransform());
            var innerRadius = loader.LoadObject<double>(components, element, "InnerRadius", () => 0);
            var outerRadius = loader.LoadObject<double>(components, element, "InnerHeight", () => 1);

            var materialName = loader.LoadObject<string>(components, element, "Material", () => null);

            var mat = components.scene.FindMaterial(materialName);

            if (mat == null)
                throw new Exception("Cannot find material '" + materialName + "' for disc.");

            return new Disc(outerRadius, innerRadius, transform)
            {
                Material = mat
            };
        }
    }
}
