using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using System;
using System.Xml.Linq;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.Primitives;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Primitives
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class PlaneParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "Plane"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var plane = new Plane();

            plane.Pos = loader.LoadObject<Point>(components, element, "Point", () => new Point(0, 0, 0));
            plane.D = plane.Pos.Length;

            plane.Normal = loader.LoadObject<Normal>(components, element, "Normal", () => new Normal(0, 1, 0));
            plane.Normal.Normalize();

            string strMaterial = loader.LoadObject<string>(components, element, "Material", () => null);

            var mat = components.Scene.FindMaterial(strMaterial);

	        plane.Material = mat;

            return plane;
        }
    }
}