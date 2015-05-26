using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;
using Raytracer.MathTypes;
using Raytracer.Properties.Annotations;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Geometry
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class ScaleParser : XYZParserBase
    {
        public override string LoaderType { get { return "Scale"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            return (Vector)LoadVector(loader, components, element, () =>
            {
                var defaultValue = createDefault();
                return new Vector(defaultValue.X, defaultValue.Y, defaultValue.Z);
            });
        }
    }
}
