using System;
using System.ComponentModel.Composition;
using System.IO;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Primitives;

namespace Raytracer.FileTypes.VBRayScene.Loaders
{
    

    [Export(typeof(IVbRaySceneItemLoader))]
    class TriangleLoader : IVbRaySceneItemLoader
    {
        public string LoaderType { get { return "Triangle"; } }
        public void LoadObject(StreamReader file, Scene scene)
        {
            Tokeniser oText = new Tokeniser();
            Triangle obj = new Triangle();

            for (int i = 0; i < 3; i++)
            {
                var pos = new Point();
                pos.X = float.Parse(oText.GetToken(file));
                pos.Y = float.Parse(oText.GetToken(file));
                pos.Z = float.Parse(oText.GetToken(file));
                obj.Vertices[i] = pos;
            }             

            obj.Pos = (obj.Vertices[0] + obj.Vertices[1] + obj.Vertices[2]) / 3.0;
            
            obj.Normals = null;

            var strMaterial = oText.GetToken(file);

	        var mat = scene.FindMaterial(strMaterial);

	        if(mat == null)
		        throw new Exception("Cannot find material '" + strMaterial + "' for sphere.");

            obj.Material = mat;

	        scene.AddObject(obj);	        
        }
    }
}