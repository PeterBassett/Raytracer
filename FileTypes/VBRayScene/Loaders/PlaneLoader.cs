using System;
using System.ComponentModel.Composition;
using System.IO;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Primitives;

namespace Raytracer.FileTypes.VBRayScene.Loaders
{
    

    [Export(typeof(IVbRaySceneItemLoader))]
    class PlaneLoader : IVbRaySceneItemLoader
    {
        public string LoaderType { get { return "Plane"; } }
        public void LoadObject(StreamReader file, Scene scene)
        {
            Tokeniser oText = new Tokeniser();
            
            Plane plane = new Plane();

            var vec = new Point();
	        vec.X = float.Parse(oText.GetToken(file));
	        vec.Y = float.Parse(oText.GetToken(file));
	        vec.Z = float.Parse(oText.GetToken(file));
            plane.Pos = vec;
            plane.D = vec.Length;

            var normal = new Normal();
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