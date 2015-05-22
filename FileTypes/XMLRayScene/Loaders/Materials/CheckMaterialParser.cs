using System;
using System.ComponentModel.Composition;
using Raytracer.Rendering.Materials;
using Raytracer.MathTypes;
using Raytracer.FileTypes.VBRayScene;
using Raytracer.Rendering.Core;
using System.Xml.Linq;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Materials
{
    [Export(typeof(XmlRayElementParser))]
    class CheckMaterialParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "CheckMaterial"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, Scene scene, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var mat = new MaterialCheckerboard();

            // get the name
            mat.Name = loader.LoadObject<string>(scene, element, "Name", () => null);
                                 
            string strMaterial = loader.LoadObject<string>(scene, element, "from-material", () => null);
            var mat1 = scene.FindMaterial(strMaterial);

            if (mat1 == null)
                throw new Exception("Cannot find material '" + strMaterial + "' for CheckMaterial.");

            strMaterial = loader.LoadObject<string>(scene, element, "to-material", () => null);
            var mat2 = scene.FindMaterial(strMaterial);

            if (mat2 == null)
                throw new Exception("Cannot find material '" + strMaterial + "' for CheckMaterial.");

            mat.SubMaterial1 = mat1;
            mat.SubMaterial2 = mat2;

            mat.Size = loader.LoadObject<Vector>(scene, element, "Scale", () => new Vector(4,4,4));

            return mat;
        }
    }
}
