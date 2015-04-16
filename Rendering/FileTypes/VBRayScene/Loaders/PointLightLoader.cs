using System.ComponentModel.Composition;
using System.IO;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.FileTypes.VBRayScene.Loaders
{
    

    [Export(typeof(IVBRaySceneItemLoader))]
    class DiffuseLightLoader : IVBRaySceneItemLoader
    {
        public string LoaderType { get { return "LightSource"; } }
        public void LoadObject(StreamReader file, Scene scene)
        {
            Tokeniser oText = new Tokeniser();
            Light light = new Light();

            Vector3 pos = new Vector3();
            pos.X = float.Parse(oText.GetToken(file));
            pos.Y = float.Parse(oText.GetToken(file));
	        pos.Z = float.Parse(oText.GetToken(file));
            light.Pos = pos;

            Colour col = new Colour();
            col.Red = float.Parse(oText.GetToken(file)) / 255.0f;
            col.Green = float.Parse(oText.GetToken(file)) / 255.0f;
            col.Blue = float.Parse(oText.GetToken(file)) / 255.0f;
            light.Diffuse = col;

            scene.AddLight(light);
        }
    }
}
