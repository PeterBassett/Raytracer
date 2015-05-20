using System;
using System.ComponentModel.Composition;
using System.IO;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Primitives;

namespace Raytracer.Rendering.FileTypes.VBRayScene.Loaders
{
    [Export(typeof(IVBRaySceneItemLoader))]
    class TorusLoader : IVBRaySceneItemLoader
    {
        public string LoaderType { get { return "Torus"; } }
        public void LoadObject(StreamReader file, Scene scene)
        {
            Tokeniser oText = new Tokeniser();
            
            var outerRadius = float.Parse(oText.GetToken(file));
            var innerRadius = float.Parse(oText.GetToken(file));

            var pos = new Vector();
            pos.X = float.Parse(oText.GetToken(file));
            pos.Y = float.Parse(oText.GetToken(file));
            pos.Z = float.Parse(oText.GetToken(file));

            var ori = new Vector();
            ori.X = float.Parse(oText.GetToken(file));
            ori.Y = float.Parse(oText.GetToken(file));
            ori.Z = float.Parse(oText.GetToken(file));

            var transform = Transform.CreateTransform(-pos, -ori);

            Torus obj = new Torus(transform, innerRadius, outerRadius);
           
            string strMaterial = oText.GetToken(file);
            
	        var mat = scene.FindMaterial(strMaterial);

	        if(mat == null)
		        throw new Exception("Cannot find material '" + strMaterial + "' for torus.");

            obj.Material = mat;

	        scene.AddObject(obj);	        
        }
    }
}
