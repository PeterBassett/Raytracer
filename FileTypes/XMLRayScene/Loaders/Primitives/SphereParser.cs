using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

using System;
using System.Xml.Linq;
using Raytracer.Rendering.Primitives;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Primitives
{
    [Export(typeof(XmlRayElementParser))]
    class SphereParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "Sphere"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var transform = loader.LoadObject<Transform>(components, element, "Transform", () => Transform.CreateIdentityTransform());
            var radius = loader.LoadObject<double>(components, element, "Radius", () => 1);
            var materialName = loader.LoadObject<string>(components, element, "Material", () => null);

            var mat = components.scene.FindMaterial(materialName);

            var rotate = transform.GetObjectSpaceRotation();
            var rotation = new Vector(MathLib.Rad2Deg(rotate.X),
                                      MathLib.Rad2Deg(rotate.Y),
                                      MathLib.Rad2Deg(rotate.Z));

            return new Sphere()
            {
                Pos = transform.ToObjectSpace(new Point(0, 0, 0)),
                Ori = rotation,
                Radius = radius,
                Material = mat
            };
        }
    }
}