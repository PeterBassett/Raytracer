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
    using Vector = Vector3D;

    [Export(typeof(IVBRaySceneItemLoader))]
    class SphereLoader : IVBRaySceneItemLoader
    {
        public string LoaderType { get { return "Sphere"; } }
        public void LoadObject(StreamReader file, Scene scene)
        {
            Tokeniser oText = new Tokeniser();
            Sphere obj = new Sphere();
            
            obj.Radius = float.Parse(oText.GetToken(file));

            Vector pos = new Vector();
            pos.X = float.Parse(oText.GetToken(file));
            pos.Y = float.Parse(oText.GetToken(file));
	        pos.Z = float.Parse(oText.GetToken(file));
            obj.Pos = pos;

            Vector ori = new Vector();

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