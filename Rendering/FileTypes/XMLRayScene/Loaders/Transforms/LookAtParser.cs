using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.FileTypes.VBRayScene;
using System;
using System.Xml.Linq;

namespace Raytracer.Rendering.FileTypes.XMLRayScene.Loaders.Transforms
{
    [Export(typeof(XMLRayElementParser))]
    class LookAtParser : XMLRayElementParser
    {
        public override string LoaderType { get { return "LookAt"; } }

        public override dynamic LoadObject(XMLRaySceneLoader loader, Scene scene, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var from = loader.LoadObject<Point>(scene, element, "From", () => Point.Zero);
            var to = loader.LoadObject<Point>(scene, element, "To", () => Point.Zero);
            var up = loader.LoadObject<Point>(scene, element, "Up", () => Point.Zero);

            return Matrix.CreateLookAt(from, to, (Vector)up);
        }
    }
}
