using System.ComponentModel.Composition;
using System.IO;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Lights;

namespace Raytracer.FileTypes.VBRayScene.Loaders
{
    [Export(typeof(IVbRaySceneItemLoader))]
    class PointLightLoader : IVbRaySceneItemLoader
    {
        public string LoaderType { get { return "PointLight"; } }
        public void LoadObject(StreamReader file, Scene scene)
        {
            var tokeniser = new Tokeniser();
            
            var pos = new Vector();
            pos.X = float.Parse(tokeniser.GetToken(file));
            pos.Y = float.Parse(tokeniser.GetToken(file));
	        pos.Z = float.Parse(tokeniser.GetToken(file));

            var col = new Colour();
            col.Red = float.Parse(tokeniser.GetToken(file)) / 255.0f;
            col.Green = float.Parse(tokeniser.GetToken(file)) / 255.0f;
            col.Blue = float.Parse(tokeniser.GetToken(file)) / 255.0f;

            var transform = Transform.CreateTransform(pos, Vector.Zero);

            var light = new PointLight(col, transform);

            scene.AddLight(light);
        }
    }
}
