using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.FileTypes.VBRayScene;
using System;
using System.Xml.Linq;

namespace Raytracer.Rendering.FileTypes.XMLRayScene.Loaders.Transforms
{
    [Export(typeof(XMLRayElementParser))]
    abstract class RotationParserBase : XMLRayElementParser
    {
        public abstract Matrix CreateRotationMatrix(double rotation);

        public override dynamic LoadObject(XMLRaySceneLoader loader, Scene scene, XElement element, string elementName, Func<dynamic> createDefault)
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
                    rotation = double.Parse(element.Value);
                }
                else
                    throw new FormatException();
            }

            return CreateRotationMatrix(rotation);
        }
    }
}
