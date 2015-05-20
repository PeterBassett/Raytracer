using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.FileTypes.VBRayScene;
using Raytracer.Rendering.Core;
using System.Xml.Linq;
using System;

namespace Raytracer.Rendering.FileTypes.XMLRayScene.Loaders.Transforms
{
    [Export(typeof(XMLRayElementParser))]
    class ScaleParser : XYZParserBase
    {
        public override string LoaderType { get { return "Scale"; } }

        public override dynamic LoadObject(XMLRaySceneLoader loader, Scene scene, XElement element, string elementName, Func<dynamic> createDefault)
        {
            return (Vector)this.LoadVector(loader, scene, element, () =>
            {
                var defaultValue = createDefault();
                return new Vector(defaultValue.X, defaultValue.Y, defaultValue.Z);
            });
        }
    }
}
