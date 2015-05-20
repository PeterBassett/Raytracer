using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.FileTypes.VBRayScene;
using System;
using System.Xml.Linq;

namespace Raytracer.Rendering.FileTypes.XMLRayScene.Loaders.Transforms
{
    [Export(typeof(XMLRayElementParser))]
    class RotateParser : XYZParserBase
    {
        public override string LoaderType { get { return "Rotate"; } }

        public override dynamic LoadObject(XMLRaySceneLoader loader, Scene scene, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var vector = this.LoadVector(loader, scene, element, () =>
            {
                var defaultValue = createDefault();
                return new Vector(defaultValue.X, defaultValue.Y, defaultValue.Z);
            });

            var v = vector.Value;

            return Matrix.CreateRotation(MathLib.Deg2Rad(v.X),
                                         MathLib.Deg2Rad(v.Y),       
                                         MathLib.Deg2Rad(v.Z));
        }
    }
}
