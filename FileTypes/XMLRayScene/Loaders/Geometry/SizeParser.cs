using System.ComponentModel.Composition;

using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using System.Xml.Linq;
using System;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Geometry
{
    [Export(typeof(XmlRayElementParser))]
    class SizeParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "Dimensions"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            int width = 0;
            int height = 0;

            var widthAttr = this.GetInt(element, "width");
            var heightAttr = this.GetInt(element, "height");

            if (widthAttr.HasValue)
                width = widthAttr.Value;

            if (heightAttr.HasValue)
                height = heightAttr.Value;

            if (!string.IsNullOrEmpty(element.Value))
            {
                var parts = element.Value.Split(',');

                if (parts.Length > 0)
                    width = int.Parse(parts[0]);
                if (parts.Length > 1)
                    height = int.Parse(parts[1]);
            }

            return new Size(width, height);
        }
    }
}
