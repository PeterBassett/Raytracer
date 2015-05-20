using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.FileTypes.VBRayScene;
using System;
using System.Xml.Linq;
using Raytracer.Rendering.Primitives;

namespace Raytracer.Rendering.FileTypes.XMLRayScene.Loaders.Primitives
{
    [Export(typeof(XMLRayElementParser))]
    class SphereParser : XMLRayElementParser
    {
        public override string LoaderType { get { return "Sphere"; } }

        public override dynamic LoadObject(XMLRaySceneLoader loader, Scene scene, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var transform = loader.LoadObject<Transform>(scene, element, "Transform", () => Transform.CreateIdentityTransform());
            var radius = loader.LoadObject<double>(scene, element, "Radius", () => 1);
            var materialName = loader.LoadObject<string>(scene, element, "Material", () => null);

            var mat = scene.FindMaterial(materialName);

	        if(mat == null)
		        throw new Exception("Cannot find material '" + materialName + "' for sphere.");

            return new Sphere()
            {
                Pos = transform.ToObjectSpace(new Point(0, 0, 0)),
                Ori = transform.ToObjectSpace(new Vector(0, 1, 0)),
                Radius = radius,
                Material = mat
            };
        }
    }
}