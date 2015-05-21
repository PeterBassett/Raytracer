using System.ComponentModel.Composition;
using System.IO;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Lights;

namespace Raytracer.FileTypes.VBRayScene.Loaders
{
    [Export(typeof(IVBRaySceneItemLoader))]
    class AmbientLightLoader : IVBRaySceneItemLoader
    {
        public string LoaderType { get { return "AmbientLight"; } }
        public void LoadObject(StreamReader file, Scene scene)
        {
            var tokeniser = new Tokeniser();
            
            var col = new Colour();
            col.Red = float.Parse(tokeniser.GetToken(file)) / 255.0f;
            col.Green = float.Parse(tokeniser.GetToken(file)) / 255.0f;
            col.Blue = float.Parse(tokeniser.GetToken(file)) / 255.0f;
           
            var light = new AmbientLight(col);

            scene.AddLight(light);
        }        
    }
}
