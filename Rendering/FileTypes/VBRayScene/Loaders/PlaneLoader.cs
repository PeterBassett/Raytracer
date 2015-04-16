using System;
using System.ComponentModel.Composition;
using System.IO;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Primitives;

namespace Raytracer.Rendering.FileTypes.VBRayScene.Loaders
{
    

    [Export(typeof(IVBRaySceneItemLoader))]
    class PlaneLoader : IVBRaySceneItemLoader
    {
        public string LoaderType { get { return "Plane"; } }
        public void LoadObject(StreamReader file, Scene scene)
        {
            Tokeniser oText = new Tokeniser();
            
            Plane plane = new Plane();

            Vector3 vec = new Vector3();
	        vec.X = float.Parse(oText.GetToken(file));
	        vec.Y = float.Parse(oText.GetToken(file));
	        vec.Z = float.Parse(oText.GetToken(file));
            plane.Pos = vec;
            plane.D = vec.GetLength();

            Vector3 normal = new Vector3();
            normal.X = float.Parse(oText.GetToken(file));
            normal.Y = float.Parse(oText.GetToken(file));
            normal.Z = float.Parse(oText.GetToken(file));
            plane.Normal = normal;
            plane.Normal.Normalize();            

            string strMaterial = oText.GetToken(file);

	        var mat = scene.FindMaterial(strMaterial);

            if(mat == null)
		        throw new Exception("Cannot find material '" + strMaterial + "' for plane.");

	        plane.Material = mat;

            scene.AddObject(plane);
        }
    }
}