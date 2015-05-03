using System;
using System.ComponentModel.Composition;
using System.IO;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Primitives;

namespace Raytracer.Rendering.FileTypes.VBRayScene.Loaders
{
    [Export(typeof(IVBRaySceneItemLoader))]
    class ConeLoader : IVBRaySceneItemLoader
    {
        public string LoaderType { get { return "Cone"; } }
        public void LoadObject(StreamReader file, Scene scene)
        {
            var oText = new Tokeniser();
            
            var radius = float.Parse(oText.GetToken(file));
            var height = float.Parse(oText.GetToken(file));
            var phiMax = float.Parse(oText.GetToken(file));

            var solidity = (Solidity)Enum.Parse(typeof(Solidity), oText.GetToken(file), true);

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

            var obj = new Cone(radius, height, phiMax, solidity, transform);

            var strMaterial = oText.GetToken(file);
            
	        var mat = scene.FindMaterial(strMaterial);

	        if(mat == null)
		        throw new Exception("Cannot find material '" + strMaterial + "' for cone.");

            obj.Material = mat;

	        scene.AddObject(obj);	        
        }
    }
}
