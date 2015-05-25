using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

using System;
using System.Xml.Linq;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Transforms
{
    [Export(typeof(XmlRayElementParser))]
    class LookAtParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "LookAt"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var from = loader.LoadObject<Point>(components, element, "From", () => Point.Zero);
            var to = loader.LoadObject<Point>(components, element, "To", () => Point.Zero);
            var up = loader.LoadObject<Vector>(components, element, "Up", () =>
            {
                Vector dir = (to - from).Normalize();
                Vector du, dv;
                Vector.CoordinateSystem(dir, out du, out dv);

                return du;
            });

            return Matrix.CreateLookAt(from, to, up);
        }
    }
}
