﻿using System.ComponentModel.Composition;
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
            
            var pos = new Vector();
            pos.X = float.Parse(tokeniser.GetToken(file));
            pos.Y = float.Parse(tokeniser.GetToken(file));
	        pos.Z = float.Parse(tokeniser.GetToken(file));

            var dir = new Vector();
            dir.X = float.Parse(tokeniser.GetToken(file));
            dir.Y = float.Parse(tokeniser.GetToken(file));
            dir.Z = float.Parse(tokeniser.GetToken(file));

            var totalWidth = float.Parse(tokeniser.GetToken(file));
            var fallOffWidth = float.Parse(tokeniser.GetToken(file));

            var col = new Colour();
            col.Red = float.Parse(tokeniser.GetToken(file)) / 255.0f;
            col.Green = float.Parse(tokeniser.GetToken(file)) / 255.0f;
            col.Blue = float.Parse(tokeniser.GetToken(file)) / 255.0f;

            var transform = Transform.CreateTransform(pos, dir);

            var light = new SpotLight(col, totalWidth, fallOffWidth, transform);

            scene.AddLight(light);
        }
    }
}