using System;
using System.ComponentModel.Composition;
using System.IO;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Primitives;

namespace Raytracer.Rendering.FileTypes.VBRayScene.Loaders
{
    

    [Export(typeof(IVBRaySceneItemLoader))]
    class SphereLoader : IVBRaySceneItemLoader
    {
        public string LoaderType { get { return "Sphere"; } }
        public void LoadObject(StreamReader file, Scene scene)
        {
            Tokeniser oText = new Tokeniser();
            Sphere obj = new Sphere();
            
            obj.Radius = float.Parse(oText.GetToken(file));

            var pos = new Point3();
            pos.X = float.Parse(oText.GetToken(file));
            pos.Y = float.Parse(oText.GetToken(file));
	        pos.Z = float.Parse(oText.GetToken(file));
            obj.Pos = pos;

            Vector3 ori = new Vector3();

            string strMaterial = null;

            float f;
            var token = oText.GetToken(file);
            if (float.TryParse(token, out f))
            {
                ori.X = f;
                ori.Y = float.Parse(oText.GetToken(file));
                ori.Z = float.Parse(oText.GetToken(file));

                strMaterial = oText.GetToken(file);
            }
            else
                strMaterial = token;

            obj.Ori = ori;

	        var mat = scene.FindMaterial(strMaterial);

	        if(mat == null)
		        throw new Exception("Cannot find material '" + strMaterial + "' for sphere.");

            obj.Material = mat;

	        scene.AddObject(obj);	        
        }
    }
}