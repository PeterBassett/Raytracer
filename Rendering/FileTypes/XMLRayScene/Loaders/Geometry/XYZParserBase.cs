using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.FileTypes.VBRayScene;
using System;
using System.Xml.Linq;

namespace Raytracer.Rendering.FileTypes.XMLRayScene.Loaders.Transforms
{
    abstract class XYZParserBase : XMLRayElementParser
    {
        public virtual Vector? LoadVector(XMLRaySceneLoader loader, Scene scene, XElement element, Func<Vector?> createDefault)
        {
            double? x = null;
            double? y = null;
            double? z = null;

            var xAttr = this.GetDouble(element, "x");
            var yAttr = this.GetDouble(element, "y");
            var zAttr = this.GetDouble(element, "z");

            if (xAttr.HasValue)
                x = xAttr.Value;

            if (yAttr.HasValue)
                y = yAttr.Value;

            if (zAttr.HasValue)
                z = zAttr.Value;

            if (!x.HasValue)
                x = loader.LoadObject<double?>(scene, element, "X", () => null);

            if (!y.HasValue)
                y = loader.LoadObject<double?>(scene, element, "Y", () => null);

            if (!z.HasValue)
                z = loader.LoadObject<double?>(scene, element, "Z", () => null);

            if (!x.HasValue || !y.HasValue || !z.HasValue)
            {
                if (!string.IsNullOrEmpty(element.Value))
                {
                    var parts = element.Value.Split(',');

                    if (parts.Length > 0)
                        x = double.Parse(parts[0]);
                    if (parts.Length == 1)
                        y = z = x;
                    if (parts.Length > 1)
                        y = double.Parse(parts[1]);
                    if (parts.Length > 2)
                        z = double.Parse(parts[2]);
                }
            }

            if (!x.HasValue || !y.HasValue || !z.HasValue)
            {
                return createDefault();
            }

            return new Vector(x.Value, y.Value, z.Value);
        }
    }
}
