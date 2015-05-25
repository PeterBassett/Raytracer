using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

using Raytracer.Rendering.Renderers;
using System.Xml.Linq;
using Raytracer.Rendering.Cameras;
using Raytracer.FileTypes.XMLRayScene.Extensions;
using System;
using System.Drawing;

namespace Raytracer.FileTypes.XMLRayScene.Loaders
{
    [Export(typeof(IXmlRaySceneItemLoader))]
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

            var bmp = new Bmp(dimensions.Width, dimensions.Height);
            components.renderer.Camera.OutputDimensions = dimensions;
            components.renderer.RenderScene(bmp);

            using(var bitmap = new Bitmap(dimensions.Width, dimensions.Height))
            {
                for (int x = 0; x < dimensions.Width; x++)
                {
                    for (int y = 0; y < dimensions.Height; y++)
                    {
                        bitmap.SetPixel(x, y, bmp.GetPixel(x, dimensions.Height - y - 1).ToColor());
                    }
                }
                bitmap.Save(fileName);
            }
        }
    }
}
