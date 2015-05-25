using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

using System;
using System.Xml.Linq;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Transforms
{
    [Export(typeof(XmlRayElementParser))]
    class RotateYParser : RotationParserBase
    {
        public override string LoaderType { get { return "RotateY"; } }

        public override Matrix CreateRotationMatrix(double rotation)
        {
            return Matrix.CreateRotationY(rotation);
        }
    }
}
