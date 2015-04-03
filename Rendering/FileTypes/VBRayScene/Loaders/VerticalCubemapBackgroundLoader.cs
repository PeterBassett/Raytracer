
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
    class VerticalCubemapBackgroundLoader : IVBRaySceneItemLoader
    {
        public string LoaderType { get { return "VerticalCubemapBackground"; } }
        public void  LoadObject(System.IO.StreamReader file, Scene scene)
        {
            Tokeniser oText = new Tokeniser();

            var filename = oText.GetToken(file);
            var background = new VerticalCubemapBackground(filename);

            scene.BackgroundMaterial = background;
        }
    }
}
