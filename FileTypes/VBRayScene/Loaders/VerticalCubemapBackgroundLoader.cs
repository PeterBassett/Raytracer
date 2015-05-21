using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

namespace Raytracer.FileTypes.VBRayScene.Loaders
{
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
