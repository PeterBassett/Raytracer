using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Xml.Linq;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.BackgroundMaterials;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Backgrounds
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class BackgroundParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "Background"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            if (element.Elements().Count() > 1)
                throw new ArgumentOutOfRangeException("loader", "Only one background ca be used");

            return loader.LoadObject<IBackgroundMaterial>(components, element.Elements().First(), () => null);
        }
    }
}
