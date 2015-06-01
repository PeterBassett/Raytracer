using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using System;
using System.Xml.Linq;
using Raytracer.Properties.Annotations;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Transforms
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class LookAtParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "LookAt"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var from = loader.LoadObject<Point>(components, element, "From", () => new Point(0,0,-1));
            var to = loader.LoadObject<Point>(components, element, "To", () => Point.Zero);
            var up = loader.LoadObject<Vector>(components, element, "Up", () =>
            {
                return new Vector(0, 1, 0);
                /*
                Vector dir = (to - from).Normalize();
                Vector du, dv;
                Vector.CoordinateSystem(dir, out du, out dv);

                return du;*/
            });

            return Matrix.CreateLookAt(from, to, up);
        }
    }
}
