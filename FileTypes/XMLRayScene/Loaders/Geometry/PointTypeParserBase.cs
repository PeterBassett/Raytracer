using System.ComponentModel.Composition;
using Raytracer.FileTypes.VBRayScene;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Geometry
{
    [Export(typeof(XmlRayElementParser))]
    abstract class PointTypeParserBase : XmlRayElementParser
    {
        public override dynamic LoadObject(XmlRaySceneLoader loader, Scene scene, System.Xml.Linq.XElement element, string elementName, System.Func<dynamic> createDefault)
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

            var point = loader.LoadObject<Point?>(scene, element, "Point", () => (Point?)null);

            if (point.HasValue)
                return point;

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

            return new Point(x, y, z);
        }
    }
}
