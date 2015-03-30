using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.Rendering;
using System.ComponentModel.Composition;
using Raytracer.Rendering.Materials;

namespace Raytracer.Rendering.FileTypes.VBRayScene.Loaders
{
    [Export(typeof(IVBRaySceneItemLoader))]
    class ColourMaterialLoader : IVBRaySceneItemLoader
    {
        public string LoaderType { get { return "ColourMaterial"; } }
        public void  LoadObject(System.IO.StreamReader file, Scene scene)
        {
 	        Tokeniser oText = new Tokeniser();

            Material mat = new Material();
	
	        // get the name
	        mat.Name = oText.GetToken(file);

	        // ambient
            Colour col = new Colour();
            col.Red = float.Parse(oText.GetToken(file));
            col.Green = float.Parse(oText.GetToken(file));
            col.Blue = float.Parse(oText.GetToken(file));
            mat.Ambient = col;

	        // diffuse
            col = new Colour();
            col.Red = float.Parse(oText.GetToken(file));
            col.Green = float.Parse(oText.GetToken(file));
            col.Blue = float.Parse(oText.GetToken(file));
            mat.Diffuse = col;

	        // specular
	        mat.Specularity = float.Parse(oText.GetToken(file));
	        mat.SpecularExponent = float.Parse(oText.GetToken(file));

	        // just default the individualr specular colours to 1.0f
	        mat.Specular.Set(1.0f);

	        // reflected
            col = new Colour();
            col.Red = float.Parse(oText.GetToken(file));
            col.Green = float.Parse(oText.GetToken(file));
            col.Blue = float.Parse(oText.GetToken(file));
            mat.Reflective = col;

	        // ignore the two things we dont know about
	        mat.Refraction = float.Parse(oText.GetToken(file));

            col = new Colour();
            col.Red = float.Parse(oText.GetToken(file));
            col.Green = float.Parse(oText.GetToken(file));
            col.Blue = float.Parse(oText.GetToken(file));
            mat.Transmitted = col;

	        scene.AddMaterial(mat, mat.Name);
        }
    }
}
