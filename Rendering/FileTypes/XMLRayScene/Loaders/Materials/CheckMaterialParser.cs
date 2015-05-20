using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Raytracer.Rendering.Lights;
using Raytracer.Rendering.Materials;
using Raytracer.MathTypes;

namespace Raytracer.Rendering.FileTypes.XMLRayScene.Loaders.Materials
{
    [Export(typeof(XMLRayElementParser))]
    class CheckMaterialParser : XMLRayElementParser
    {
        public override string LoaderType { get { return "CheckMaterial"; } }

        public override dynamic LoadObject(VBRayScene.XMLRaySceneLoader loader, Core.Scene scene, System.Xml.Linq.XElement element, string elementName, Func<dynamic> createDefault)
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
