using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.Core;

using System;
using System.Xml.Linq;
using Raytracer.Rendering.Primitives;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Primitives
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class CubeParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "Cube"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var transform = loader.LoadObject(components, element, "Transform", Transform.CreateIdentityTransform);
            var materialName = loader.LoadObject<string>(components, element, "Material", () => null);

            var mat = components.Scene.FindMaterial(materialName);

            return new Cube(transform)
            {
                Material = mat
            };
        }
    }
}