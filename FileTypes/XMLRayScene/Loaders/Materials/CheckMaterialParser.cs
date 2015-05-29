using System;
using System.ComponentModel.Composition;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.Materials;
using Raytracer.MathTypes;
using System.Xml.Linq;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Materials
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class CheckMaterialParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "CheckMaterial"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var mat = new MaterialCheckerboard();

            // get the name
            mat.Name = loader.LoadObject<string>(components, element, "Name", () => null);
                                 
            string strMaterial = loader.LoadObject<string>(components, element, "from-material", () => null);
            var mat1 = components.Scene.FindMaterial(strMaterial);

            if (mat1 == null)
                throw new Exception("Cannot find material '" + strMaterial + "' for CheckMaterial.");

            strMaterial = loader.LoadObject<string>(components, element, "to-material", () => null);
            var mat2 = components.Scene.FindMaterial(strMaterial);

            if (mat2 == null)
                throw new Exception("Cannot find material '" + strMaterial + "' for CheckMaterial.");

            mat.SubMaterial1 = mat1;
            mat.SubMaterial2 = mat2;

            mat.Size = loader.LoadObject<Matrix>(components, element, "Size", () => Matrix.CreateScale(4)).Transform(new Vector(1.0, 1.0, 1.0));

            return mat;
        }
    }
}
