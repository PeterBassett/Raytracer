using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Materials;

namespace Raytracer.Rendering.FileTypes.VBRayScene.Loaders
{

    [Export(typeof(IVBRaySceneItemLoader))]
    class TextureMaterialLoader : IVBRaySceneItemLoader
    {
        public string LoaderType { get { return "TextureMaterial"; } }
        public void  LoadObject(System.IO.StreamReader file, Scene scene)
        {
            Tokeniser oText = new Tokeniser();

            var mat = new MaterialTexture();

            mat.Name = oText.GetToken(file);

            // get the name
            var token = oText.GetToken(file);

            float f;
            if (float.TryParse(token, out f))
            {
                // scale specified.
                mat.UScale = 1.0 / f;
                mat.VScale = 1.0 / float.Parse(oText.GetToken(file));

                mat.LoadDiffuseMap(oText.GetToken(file));
            }
            else
                mat.LoadDiffuseMap(token);            

            scene.AddMaterial(mat, mat.Name);
        }
    }
}
