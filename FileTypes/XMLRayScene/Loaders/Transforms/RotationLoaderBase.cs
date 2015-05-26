using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using System;
using System.Xml.Linq;
using Raytracer.Properties.Annotations;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Transforms
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    abstract class RotationParserBase : XmlRayElementParser
    {
        protected abstract Matrix CreateRotationMatrix(double rotation);

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            double rotation;

            var degrees = GetDouble(element, "degrees");
            if (degrees.HasValue)
                rotation = MathLib.Deg2Rad(degrees.Value);
            else
            {
                var radians = GetDouble(element, "radians");
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
