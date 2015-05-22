using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.FileTypes.VBRayScene;
using System;
using System.Xml.Linq;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Transforms
{
    [Export(typeof(XmlRayElementParser))]
    abstract class RotationParserBase : XmlRayElementParser
    {
        public abstract Matrix CreateRotationMatrix(double rotation);

        public override dynamic LoadObject(XmlRaySceneLoader loader, Scene scene, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var rotation = 0.0;

            var degrees = this.GetDouble(element, "degrees");
            if (degrees.HasValue)
                rotation = MathLib.Deg2Rad(degrees.Value);
            else
            {
                var radians = this.GetDouble(element, "radians");
                if (radians.HasValue)
                    rotation = radians.Value;
                else if (!string.IsNullOrEmpty(element.Value))
                {
                    rotation = MathLib.Deg2Rad(double.Parse(element.Value));
                }
                else
                    throw new FormatException();
            }

            return CreateRotationMatrix(rotation);
        }
    }
}
