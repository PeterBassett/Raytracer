using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using System;
using System.Xml.Linq;
using Raytracer.Properties.Annotations;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Transforms
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class TranslateParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "Translate"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            double x = 0.0;
            double y = 0.0;
            double z = 0.0;

            var xAttr = this.GetDouble(element, "x");
            var yAttr = this.GetDouble(element, "y");
            var zAttr = this.GetDouble(element, "z");

            if (xAttr.HasValue)
                x = xAttr.Value;

            if (yAttr.HasValue)
                y = yAttr.Value;

            if (zAttr.HasValue)
                z = zAttr.Value;

            if (!string.IsNullOrEmpty(element.Value))
            {
                var parts = element.Value.Split(',');

                if (parts.Length > 0)
                    x = double.Parse(parts[0]);
                if (parts.Length > 1)
                    y = double.Parse(parts[1]);
                if (parts.Length > 2)
                    z = double.Parse(parts[2]);
            }

            return Matrix.CreateTranslation(x, y, z);
        }
    }
}
