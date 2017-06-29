using System;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Xml.Linq;
using Raytracer.FileTypes.XMLRayScene.Extensions;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.Core;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Output
{
    [Export(typeof(IXmlRaySceneItemLoader)), UsedImplicitly]
    class OutputElementLoader : IXmlRaySceneItemLoader
    {
        public string LoaderType { get { return "Output"; } }
        
        public void LoadObject(XmlRaySceneLoader loader, XElement element, SystemComponents components)
        {
            var dimensions = loader.LoadObject<Raytracer.MathTypes.Size>(components, element, "Dimensions", () => new Raytracer.MathTypes.Size(800, 600));

            var file = element.ElementCaseInsensitive("File");
            if (file == null)
                throw new ArgumentNullException("File");

            var overwriteAttr = file.AttributeCaseInsensitive("overwrite");
            var fileName = file.Value;
            bool overwrite = overwriteAttr != null && overwriteAttr.Value == "true";

            //var bmp = new Bmp(dimensions.Width, dimensions.Height);
            var buffer = new Raytracer.Rendering.Core.Buffer(dimensions.Width, dimensions.Height);
            components.Renderer.Camera.OutputDimensions = dimensions;
            components.Renderer.RenderScene(buffer);

            using(var bitmap = new Bitmap(dimensions.Width, dimensions.Height))
            {
                for (int x = 0; x < dimensions.Width; x++)
                {
                    for (int y = 0; y < dimensions.Height; y++)
                    {
                        bitmap.SetPixel(x, y, buffer.Colour(x, dimensions.Height - y - 1).ToColor());
                    }
                }
                bitmap.Save(fileName);
            }
        }
    }
}
