using System.ComponentModel.Composition;
using System.IO;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Lights;

namespace Raytracer.Rendering.FileTypes.VBRayScene.Loaders
{
    [Export(typeof(IVBRaySceneItemLoader))]
    class DistantLightLoader : IVBRaySceneItemLoader
    {
        public string LoaderType { get { return "DistantLight"; } }
        public void LoadObject(StreamReader file, Scene scene)
        {
            var tokeniser = new Tokeniser();
            
            var dir = new Vector();
            dir.X = float.Parse(tokeniser.GetToken(file));
            dir.Y = float.Parse(tokeniser.GetToken(file));
	        dir.Z = float.Parse(tokeniser.GetToken(file));

            var transform = Transform.CreateTransform(Vector.Zero, dir);

            var col = new Colour();
            col.Red = float.Parse(tokeniser.GetToken(file)) / 255.0f;
            col.Green = float.Parse(tokeniser.GetToken(file)) / 255.0f;
            col.Blue = float.Parse(tokeniser.GetToken(file)) / 255.0f;

            var light = new DistantLight(col, 1, transform);

            scene.AddLight(light);
        }
    }
}
