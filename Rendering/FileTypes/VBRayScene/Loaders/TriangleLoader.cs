using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.Rendering;
using System.IO;
using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Primitives;

namespace Raytracer.Rendering.FileTypes.VBRayScene.Loaders
{
    using Vector = Vector3;

    [Export(typeof(IVBRaySceneItemLoader))]
    class TriangleLoader : IVBRaySceneItemLoader
    {
        public string LoaderType { get { return "Triangle"; } }
        public void LoadObject(StreamReader file, Scene scene)
        {
            Tokeniser oText = new Tokeniser();
            Triangle obj = new Triangle();

            for (int i = 0; i < 3; i++)
            {
                Vector pos = new Vector();
                pos.X = float.Parse(oText.GetToken(file));
                pos.Y = float.Parse(oText.GetToken(file));
                pos.Z = float.Parse(oText.GetToken(file));
                obj.Vertex[i] = pos;
            }             

            obj.Pos = (obj.Vertex[0] + obj.Vertex[1] + obj.Vertex[2]) / 3.0;

            string strMaterial = oText.GetToken(file);

	        var mat = scene.FindMaterial(strMaterial);

	        if(mat == null)
		        throw new Exception("Cannot find material '" + strMaterial + "' for sphere.");

            obj.Material = mat;

	        scene.AddObject(obj);	        
        }
    }
}