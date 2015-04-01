using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.MathTypes;
using System.ComponentModel.Composition;

namespace Raytracer.Rendering.FileTypes.VBRayScene.Loaders
{
    using Vector = Vector3;

    [Export(typeof(IVBRaySceneItemLoader))]
    class ViewpointLoader : IVBRaySceneItemLoader
    {
        public string LoaderType { get { return "Viewpoint"; } }
        public void LoadObject(System.IO.StreamReader file, Scene scene)
        {
	        Tokeniser oText = new Tokeniser();

            Vector pos = new Vector();
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
