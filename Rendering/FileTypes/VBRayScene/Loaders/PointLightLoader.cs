using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.Rendering;
using System.IO;
using System.ComponentModel.Composition;
using Raytracer.MathTypes;

namespace Raytracer.Rendering.FileTypes.VBRayScene.Loaders
{
    using Vector = Vector3;

    [Export(typeof(IVBRaySceneItemLoader))]
    class DiffuseLightLoader : IVBRaySceneItemLoader
    {
        public string LoaderType { get { return "LightSource"; } }
        public void LoadObject(StreamReader file, Scene scene)
        {
            Tokeniser oText = new Tokeniser();
            Light light = new Light();

            Vector pos = new Vector();
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
