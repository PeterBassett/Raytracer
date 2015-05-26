using System;
using System.ComponentModel.Composition;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.Materials;
using System.Xml.Linq;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Materials
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class TextureMaterialParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "TextureMaterial"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var mat = new MaterialTexture();

            mat.Name = loader.LoadObject<string>(components, element, "Name", () => null);

            mat.UScale = 1.0 / loader.LoadObject<double>(components, element, "UScale", () => 1);
            mat.VScale = 1.0 / loader.LoadObject<double>(components, element, "VScale", () => 1);

            mat.LoadDiffuseMap(loader.LoadObject<string>(components, element, "Texture", () => null));
            
            return mat;
        }
    }
}
