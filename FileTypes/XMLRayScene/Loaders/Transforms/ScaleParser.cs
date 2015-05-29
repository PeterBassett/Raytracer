using System.ComponentModel.Composition;
using Raytracer.FileTypes.XMLRayScene.Loaders.Geometry;
using Raytracer.MathTypes;
using System;
using System.Xml.Linq;
using Raytracer.Properties.Annotations;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Transforms
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class ScaleParser : XYZParserBase
    {
        public override string LoaderType { get { return "Scale"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var vector = LoadVector(loader, components, element, () =>
            {
                var defaultValue = createDefault();
                return new Vector(defaultValue.X, defaultValue.Y, defaultValue.Z);
            });

            return Matrix.CreateScale(1/vector.Value);
        }
    }
}
