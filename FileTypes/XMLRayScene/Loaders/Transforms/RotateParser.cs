using System.ComponentModel.Composition;
using Raytracer.FileTypes.XMLRayScene.Loaders.Geometry;
using Raytracer.MathTypes;
using System;
using System.Xml.Linq;
using Raytracer.Properties.Annotations;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Transforms
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class RotateParser : XYZParserBase
    {
        public override string LoaderType { get { return "Rotate"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var vector = LoadVector(loader, components, element, () =>
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
