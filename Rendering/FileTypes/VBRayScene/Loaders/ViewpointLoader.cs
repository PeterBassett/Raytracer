using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.FileTypes.VBRayScene.Loaders
{
    

    [Export(typeof(IVBRaySceneItemLoader))]
    class ViewpointLoader : IVBRaySceneItemLoader
    {
        public string LoaderType { get { return "Viewpoint"; } }
        public void LoadObject(System.IO.StreamReader file, Scene scene)
        {
	        Tokeniser oText = new Tokeniser();

            Vector3 pos = new Vector3();
            pos.X = float.Parse(oText.GetToken(file));
            pos.Y = float.Parse(oText.GetToken(file));
            pos.Z = float.Parse(oText.GetToken(file));
            scene.EyePosition = pos;

            Vector3 dir = new Vector3();
            dir.X = float.Parse(oText.GetToken(file));
            dir.Y = float.Parse(oText.GetToken(file));
            dir.Z = float.Parse(oText.GetToken(file));
            scene.ViewPointRotation = dir;
            
            scene.FieldOfView = float.Parse(oText.GetToken(file));
        }
    }
}
