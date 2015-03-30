using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace Raytracer.Rendering.FileTypes.VBRayScene.Loaders
{
    [Export(typeof(IVBRaySceneItemLoader))]
    class OutputLoader : IVBRaySceneItemLoader
    {
        public string LoaderType { get { return "OutputSize"; } }
        public void  LoadObject(System.IO.StreamReader file, Scene scene)
        {
 	        Tokeniser oText = new Tokeniser();
	        	
	        scene.Width = int.Parse(oText.GetToken(file));
            scene.Height = int.Parse(oText.GetToken(file));            
        }
    }
}
