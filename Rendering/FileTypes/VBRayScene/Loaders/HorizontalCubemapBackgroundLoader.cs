using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.FileTypes.VBRayScene.Loaders
{
    using Raytracer.Rendering.BackgroundMaterials;

    [Export(typeof(IVBRaySceneItemLoader))]
    class HorizontalCubemapBackgroundLoader : IVBRaySceneItemLoader
    {
        public string LoaderType { get { return "HorizontalCubemapBackground"; } }
        public void  LoadObject(System.IO.StreamReader file, Scene scene)
        {
            Tokeniser oText = new Tokeniser();

            var filename = oText.GetToken(file);
            var background = new HorizontalCubemapBackground(filename);

            scene.BackgroundMaterial = background;
        }
    }
}
