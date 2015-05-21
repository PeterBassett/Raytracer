using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.FileTypes.VBRayScene;
using System;
using System.Xml.Linq;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Transforms
{
    [Export(typeof(XMLRayElementParser))]
    class LookAtParser : XMLRayElementParser
    {
        public override string LoaderType { get { return "LookAt"; } }

        public override dynamic LoadObject(XMLRaySceneLoader loader, Scene scene, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var from = loader.LoadObject<Point>(scene, element, "From", () => Point.Zero);
            var to = loader.LoadObject<Point>(scene, element, "To", () => Point.Zero);
            var up = loader.LoadObject<Vector>(scene, element, "Up", () =>
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
