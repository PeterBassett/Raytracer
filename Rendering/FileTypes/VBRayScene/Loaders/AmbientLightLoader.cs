using System.ComponentModel.Composition;
using System.IO;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.FileTypes.VBRayScene.Loaders
{
    [Export(typeof(IVBRaySceneItemLoader))]
    class AmbientLightLoader : IVBRaySceneItemLoader
    {
        public string LoaderType { get { return "AmbientLight"; } }
        public void LoadObject(StreamReader file, Scene scene)
        {
            Tokeniser oText = new Tokeniser();
            Light light = new Light();

            Colour col = new Colour();
            col.Red = float.Parse(oText.GetToken(file)) / 255.0f;
            col.Green = float.Parse(oText.GetToken(file)) / 255.0f;
            col.Blue = float.Parse(oText.GetToken(file)) / 255.0f;
            light.Ambient = col;

            scene.AddLight(light);
        }        
    }
}
