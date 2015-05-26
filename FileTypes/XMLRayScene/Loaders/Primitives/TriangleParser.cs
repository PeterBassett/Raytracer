using System;
using System.Linq;
using System.ComponentModel.Composition;
using System.Xml.Linq;
using Raytracer.MathTypes;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.Primitives;
using Raytracer.FileTypes.XMLRayScene.Extensions;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Primitives
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class TriangleParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "Triangle"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var obj = new Triangle();

            var points = element.ElementsCaseInsensitive("Point").ToArray();

            for (int i = 0; i < 3; i++)
                obj.Vertices[i] = loader.LoadObject<Point>(components, points[i], () => { throw new ArgumentNullException("Three points are required"); });

            obj.Pos = (obj.Vertices[0] + obj.Vertices[1] + obj.Vertices[2]) / 3.0;

            obj.Normals = null;

            var materialName = loader.LoadObject<string>(components, element, "Material", () => null);

            var mat = components.scene.FindMaterial(materialName);

            if (mat == null)
                throw new Exception("Cannot find material '" + materialName + "' for triangle.");

            obj.Material = mat;

            return obj;	
        }
    }
}