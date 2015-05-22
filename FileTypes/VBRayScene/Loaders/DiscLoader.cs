using System;
using System.ComponentModel.Composition;
using System.IO;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Primitives;

namespace Raytracer.FileTypes.VBRayScene.Loaders
{
    [Export(typeof(IVbRaySceneItemLoader))]
    class DiscLoader : IVbRaySceneItemLoader
    {
        public string LoaderType { get { return "Disc"; } }
        public void LoadObject(StreamReader file, Scene scene)
        {
            var oText = new Tokeniser();
            
            var outerRadius = float.Parse(oText.GetToken(file));
            var innerRadius = float.Parse(oText.GetToken(file));

            var pos = new Vector
            {
                X = float.Parse(oText.GetToken(file)),
                Y = float.Parse(oText.GetToken(file)),
                Z = float.Parse(oText.GetToken(file))
            };

            var ori = new Vector
            {
                X = float.Parse(oText.GetToken(file)),
                Y = float.Parse(oText.GetToken(file)),
                Z = float.Parse(oText.GetToken(file))
            };

            var transform = Transform.CreateTransform(-pos, -ori);

            var obj = new Disc(outerRadius, innerRadius, transform);

            string strMaterial = oText.GetToken(file);
            
	        var mat = scene.FindMaterial(strMaterial);

	        if(mat == null)
		        throw new Exception("Cannot find material '" + strMaterial + "' for disc.");

            obj.Material = mat;

	        scene.AddObject(obj);	        
        }
    }
}
