using System;
using System.ComponentModel.Composition;
using System.IO;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Primitives;

namespace Raytracer.Rendering.FileTypes.XMLRayScene.Loaders
{
    [Export(typeof(IVBRaySceneItemLoader))]
    class CylinderLoader : IVBRaySceneItemLoader
    {
        public string LoaderType { get { return "Cylinder"; } }
        public void LoadObject(StreamReader file, Scene scene)
        {
            var oText = new Tokeniser();
            
            var radius = float.Parse(oText.GetToken(file));
            var height = float.Parse(oText.GetToken(file));

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

            var obj = new Cylinder(radius, height, transform);

            var strMaterial = oText.GetToken(file);
            
	        var mat = scene.FindMaterial(strMaterial);

	        if(mat == null)
		        throw new Exception("Cannot find material '" + strMaterial + "' for cylinder.");

            obj.Material = mat;

	        scene.AddObject(obj);	        
        }
    }
}
