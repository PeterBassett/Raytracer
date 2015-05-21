using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Raytracer.Rendering.Lights;
using Raytracer.Rendering.Core;
using Raytracer.MathTypes;
using Raytracer.Rendering.BackgroundMaterials;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Lights
{
    [Export(typeof(XMLRayElementParser))]
    class SolidColourBackgroundParser : XMLRayElementParser
    {
        public override string LoaderType { get { return "SolidColourBackground"; } }

        public override dynamic LoadObject(VBRayScene.XMLRaySceneLoader loader, Rendering.Core.Scene scene, System.Xml.Linq.XElement element, string elementName, Func<dynamic> createDefault)
        {
            var colour = loader.LoadObject<Colour>(scene, element, "Colour", () => null);

            if (colour == null)
            {
                var parser = new ColourParser();
                colour = parser.LoadObject(loader, scene, element, element.Name.LocalName, () => null);
            }

            if (colour == null)
                colour = new Colour(0);

            return new SolidColourBackground(colour);
        }
    }
}
