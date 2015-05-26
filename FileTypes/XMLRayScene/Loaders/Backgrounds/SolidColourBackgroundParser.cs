using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.BackgroundMaterials;
using Raytracer.Rendering.Core;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Backgrounds
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class SolidColourBackgroundParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "SolidColourBackground"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            if (components.scene == null) 
                throw new ArgumentNullException("scene");

            var colour = loader.LoadObject<Colour>(components, element, "Colour", () => null);

            if (colour == null)
            {
                var parser = new ColourParser();
                colour = parser.LoadObject(loader, components, element, element.Name.LocalName, () => null);
            }

            if (colour == null)
                colour = new Colour(0);

            return new SolidColourBackground(colour);
        }
    }
}
