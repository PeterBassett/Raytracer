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
    class PlaneParser : XMLRayElementParser
    {
        public override string LoaderType { get { return "Plane"; } }

        public override dynamic LoadObject(XMLRaySceneLoader loader, Scene scene, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var plane = new Plane();

            plane.Pos = loader.LoadObject<Point>(scene, element, "Point", () => new Point(0,0,0));
            plane.D = plane.Pos.Length;

            plane.Normal = loader.LoadObject<Normal>(scene, element, "Normal", () => new Normal(0, 1, 0));
            plane.Normal.Normalize();

            string strMaterial = loader.LoadObject<string>(scene, element, "Material", () => null);
            
	        var mat = scene.FindMaterial(strMaterial);

            if(mat == null)
		        throw new Exception("Cannot find material '" + strMaterial + "' for plane.");

	        plane.Material = mat;

            return plane;
        }
    }
}