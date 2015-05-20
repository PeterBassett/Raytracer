using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Raytracer.Rendering.Lights;
using Raytracer.Rendering.Materials;
using Raytracer.MathTypes;

namespace Raytracer.Rendering.FileTypes.XMLRayScene.Loaders.Materials
{
    [Export(typeof(XMLRayElementParser))]
    class TextureMaterialParser : XMLRayElementParser
    {
        public override string LoaderType { get { return "TextureMaterial"; } }

        public override dynamic LoadObject(VBRayScene.XMLRaySceneLoader loader, Core.Scene scene, System.Xml.Linq.XElement element, string elementName, Func<dynamic> createDefault)
        {
            var mat = new MaterialTexture();

            mat.Name = loader.LoadObject<string>(scene, element, "Name", () => null);

            mat.UScale = 1.0 / loader.LoadObject<double>(scene, element, "UScale", () => 1);
            mat.VScale = 1.0 / loader.LoadObject<double>(scene, element, "VScale", () => 1);

            mat.LoadDiffuseMap(loader.LoadObject<string>(scene, element, "Texture", () => null));
            
            return mat;
        }
    }
}
