using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.Rendering;
using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Materials;

namespace Raytracer.Rendering.FileTypes.VBRayScene.Loaders
{
    using Vector = Vector3;

    [Export(typeof(IVBRaySceneItemLoader))]
    class CheckerMaterialLoader : IVBRaySceneItemLoader
    {
        public string LoaderType { get { return "CheckMaterial"; } }
        public void  LoadObject(System.IO.StreamReader file, Scene scene)
        {
            Tokeniser oText = new Tokeniser();

            MaterialCheckerboard mat = new MaterialCheckerboard();

            Material mat1 = null;
            Material mat2 = null;

            // get the name
            mat.Name = oText.GetToken(file);

            scene.AddMaterial(mat, mat.Name);

            string strMaterial = oText.GetToken(file);
            mat1 = scene.FindMaterial(strMaterial);

            if (mat1 == null)
                throw new Exception("Cannot find material '" + strMaterial + "' for CheckMaterial.");

            strMaterial = oText.GetToken(file);
            mat2 = scene.FindMaterial(strMaterial);

            if (mat2 == null)
                throw new Exception("Cannot find material '" + strMaterial + "' for CheckMaterial.");

            mat.SubMaterial1 = mat1;
            mat.SubMaterial2 = mat2;

            Vector size = new Vector();
            size.X = float.Parse(oText.GetToken(file));
            size.Y = float.Parse(oText.GetToken(file));
            size.Z = float.Parse(oText.GetToken(file));
            mat.Size = size;
        }
    }
}
