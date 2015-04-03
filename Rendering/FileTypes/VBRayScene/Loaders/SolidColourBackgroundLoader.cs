using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.Rendering;
using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Materials;

namespace Raytracer.Rendering.FileTypes.VBRayScene.Loaders
{
    using Vector = Vector3;
    using Raytracer.Rendering.BackgroundMaterials;

    [Export(typeof(IVBRaySceneItemLoader))]
    class SolidColourBackgroundLoader : IVBRaySceneItemLoader
    {
        public string LoaderType { get { return "SolidColourBackground"; } }
        public void  LoadObject(System.IO.StreamReader file, Scene scene)
        {
            Tokeniser oText = new Tokeniser();

            Colour colour = new Colour();
            colour.Red = float.Parse(oText.GetToken(file));
            colour.Green = float.Parse(oText.GetToken(file));
            colour.Blue = float.Parse(oText.GetToken(file));

            scene.BackgroundMaterial = new SolidColourBackground(colour);
        }
    }
}
