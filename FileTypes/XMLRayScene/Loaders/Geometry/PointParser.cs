using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.FileTypes.VBRayScene;
using System;
using System.Xml.Linq;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Transforms
{
    [Export(typeof(XMLRayElementParser))]
    class PointParser : XYZParserBase
    {
        public override string LoaderType { get { return "Point"; } }

        public override dynamic LoadObject(XMLRaySceneLoader loader, Scene scene, XElement element, string elementName, Func<dynamic> createDefault)
        {
            return (Point)this.LoadVector(loader, scene, element, () =>
            {
                var defaultValue = createDefault();
                return new Vector(defaultValue.X, defaultValue.Y, defaultValue.Z);
            });
        }
    }
}
