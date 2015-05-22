using System;
using System.ComponentModel.Composition;
using System.IO;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Primitives;

namespace Raytracer.FileTypes.VBRayScene.Loaders
{
    

    [Export(typeof(IVbRaySceneItemLoader))]
    class SphereLoader : IVbRaySceneItemLoader
    {
        public string LoaderType { get { return "Sphere"; } }
        public void LoadObject(StreamReader file, Scene scene)
        {
            var oText = new Tokeniser();
            var obj = new Sphere
            {
                Radius = float.Parse(oText.GetToken(file))
            };

            var pos = new Point
            {
                X = float.Parse(oText.GetToken(file)),
                Y = float.Parse(oText.GetToken(file)),
                Z = float.Parse(oText.GetToken(file))
            };
            obj.Pos = pos;

            var ori = new Vector();

            string strMaterial;

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