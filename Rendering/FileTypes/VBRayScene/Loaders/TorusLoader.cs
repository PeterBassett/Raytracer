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
    class TorusLoader : IVBRaySceneItemLoader
    {
        public string LoaderType { get { return "Torus"; } }
        public void LoadObject(StreamReader file, Scene scene)
        {
            Tokeniser oText = new Tokeniser();
            Torus obj = new Torus();
            
            obj.OuterRadius = float.Parse(oText.GetToken(file));
            obj.InnerRadius = float.Parse(oText.GetToken(file));

            Vector pos = new Vector();
            pos.X = float.Parse(oText.GetToken(file));
            pos.Y = float.Parse(oText.GetToken(file));
	        pos.Z = float.Parse(oText.GetToken(file));
            obj.Pos = pos;

            string strMaterial = oText.GetToken(file);
            
	        var mat = scene.FindMaterial(strMaterial);

	        if(mat == null)
		        throw new Exception("Cannot find material '" + strMaterial + "' for torus.");

            obj.Material = mat;

	        scene.AddObject(obj);	        
        }
    }
}