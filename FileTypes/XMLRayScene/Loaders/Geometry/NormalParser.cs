using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;

using Raytracer.FileTypes.XMLRayScene.Loaders.Transforms;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Geometry
{
    [Export(typeof(XmlRayElementParser))]
    class NormalParser : XYZParserBase
    {
        public override string LoaderType { get { return "Normal"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            return (Normal)this.LoadVector(loader, components, element, () =>
            {
                var defaultValue = createDefault();
                return new Vector(defaultValue.X, defaultValue.Y, defaultValue.Z);
            });
        }
    }
}
