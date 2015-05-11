using System.ComponentModel.Composition;
using System.IO;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Lights;

namespace Raytracer.Rendering.FileTypes.VBRayScene.Loaders
{
    [Export(typeof(IVBRaySceneItemLoader))]
    class SpotLightLoader : IVBRaySceneItemLoader
    {
        public string LoaderType { get { return "SpotLight"; } }
        public void LoadObject(StreamReader file, Scene scene)
        {
            var tokeniser = new Tokeniser();
            
            var from = new Vector();
            from.X = float.Parse(tokeniser.GetToken(file));
            from.Y = float.Parse(tokeniser.GetToken(file));
	        from.Z = float.Parse(tokeniser.GetToken(file));

            var to = new Vector();
            to.X = float.Parse(tokeniser.GetToken(file));
            to.Y = float.Parse(tokeniser.GetToken(file));
            to.Z = float.Parse(tokeniser.GetToken(file));

            var totalWidth = float.Parse(tokeniser.GetToken(file));
            var fallOffWidth = float.Parse(tokeniser.GetToken(file));

            var col = new Colour();
            col.Red = float.Parse(tokeniser.GetToken(file)) / 255.0f;
            col.Green = float.Parse(tokeniser.GetToken(file)) / 255.0f;
            col.Blue = float.Parse(tokeniser.GetToken(file)) / 255.0f;

            Vector dir = (to - from).Normalize();

            Vector du, dv;
            Vector.CoordinateSystem(dir, out du, out dv);

            var lightToWorld = Matrix.CreateLookAt((Point)from, to - from, du);

            var worldToLight = lightToWorld;
            worldToLight.Invert();

            var transform = new Transform(lightToWorld, worldToLight);

            var light = new SpotLight(col, totalWidth, fallOffWidth, transform);

            scene.AddLight(light);
        }
    }
}
