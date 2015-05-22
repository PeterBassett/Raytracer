using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.FileTypes.VBRayScene;
using System.Xml.Linq;
using System;

namespace Raytracer.FileTypes.XMLRayScene.Loaders
{
    [Export(typeof(XmlRayElementParser))]
    class ColourParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "Colour"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, Scene scene, XElement element, string elementName, Func<dynamic> createDefault)
        {
            double? r = null;
            double? g = null;
            double? b = null;

            var rAttr = this.GetDouble(element, "r");
            var gAttr = this.GetDouble(element, "g");
            var bAttr = this.GetDouble(element, "b");

            if (rAttr.HasValue)
                r = rAttr.Value;

            if (gAttr.HasValue)
                g = gAttr.Value;

            if (bAttr.HasValue)
                b = bAttr.Value;

            if(!r.HasValue)
                r = loader.LoadObject<double?>(scene, element, "Red", () => null);

            if (!g.HasValue)
                g = loader.LoadObject<double?>(scene, element, "Green", () => null);

            if(!b.HasValue)
                b = loader.LoadObject<double?>(scene, element, "Blue", () => null);

            if (!r.HasValue || !g.HasValue || !b.HasValue)
            {
                if (!string.IsNullOrEmpty(element.Value))
                {
                    var parts = element.Value.Split(',');

                    if (parts.Length > 0)
                        r = double.Parse(parts[0]);
                    if (parts.Length == 1)
                        g = b = r;
                    if (parts.Length > 1)
                        g = double.Parse(parts[1]);
                    if (parts.Length > 2)
                        b = double.Parse(parts[2]);
                }
            }

            if (!r.HasValue)
                r = 0;

            if (!g.HasValue)
                g = 0;

            if (!b.HasValue)
                b = 0;

            return new Colour(r.Value, g.Value, b.Value);
        }
    }
}
