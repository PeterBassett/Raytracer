using System;
using System.ComponentModel.Composition;
using Raytracer.Rendering.Core;

namespace Raytracer.FileTypes.VBRayScene.Loaders
{
    [Export(typeof(IVBRaySceneItemLoader))]
    class RenderSettingsLoader : IVBRaySceneItemLoader
    {
        public string LoaderType { get { return "RenderSettings"; } }
        public void  LoadObject(System.IO.StreamReader file, Scene scene)
        {
 	        Tokeniser oText = new Tokeniser();
            
	        scene.RecursionDepth = int.Parse(oText.GetToken(file));

            scene.TraceShadows = ParseBoolOrIntToBool(oText.GetToken(file));
            scene.TraceReflections = ParseBoolOrIntToBool(oText.GetToken(file));
            scene.TraceRefractions = ParseBoolOrIntToBool(oText.GetToken(file));
        }

        private bool ParseBoolOrIntToBool(string value)
        {
            bool bln = false;
            if (bool.TryParse(value, out bln))
                return bln;

            int val = 0;
            if (int.TryParse(value, out val))
                return val == 1;

            throw new ArgumentException("Invalid rendersetting value " + value);
        }
    }
}
