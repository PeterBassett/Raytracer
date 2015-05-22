using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

namespace Raytracer.FileTypes.VBRayScene.Loaders
{
    

    [Export(typeof(IVbRaySceneItemLoader))]
    class ViewpointLoader : IVbRaySceneItemLoader
    {
        public string LoaderType { get { return "Viewpoint"; } }
        public void LoadObject(System.IO.StreamReader file, Scene scene)
        {
	        Tokeniser oText = new Tokeniser();

            var pos = new Point();
            pos.X = float.Parse(oText.GetToken(file));
            pos.Y = float.Parse(oText.GetToken(file));
            pos.Z = float.Parse(oText.GetToken(file));
            scene.EyePosition = pos;

            Vector dir = new Vector();
            dir.X = float.Parse(oText.GetToken(file));
            dir.Y = float.Parse(oText.GetToken(file));
            dir.Z = float.Parse(oText.GetToken(file));
            scene.ViewPointRotation = dir;
            
            scene.FieldOfView = float.Parse(oText.GetToken(file));
        }
    }
}
