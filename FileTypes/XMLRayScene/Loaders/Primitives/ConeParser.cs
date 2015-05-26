using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Primitives;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Primitives
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class ConeParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "Cone"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var transform = loader.LoadObject(components, element, "Transform", Transform.CreateIdentityTransform);
            var radius = loader.LoadObject<double>(components, element, "Radius", () => 1);
            var height = loader.LoadObject<double>(components, element, "Height", () => 2);
            var phiMax = loader.LoadObject<double>(components, element, "PhiMax", () => 1);

            var solidity = (Solidity)Enum.Parse(typeof(Solidity), loader.LoadObject<string>(components, element, "Solidity", () => "Solid"), true);

            var materialName = loader.LoadObject<string>(components, element, "Material", () => null);

            var mat = components.scene.FindMaterial(materialName);

            if (mat == null)
                throw new Exception("Cannot find material '" + materialName + "' for cone.");

            return new Cone(radius, height, phiMax, solidity, transform)
            {
                Material = mat
            };
        }
    }
}
