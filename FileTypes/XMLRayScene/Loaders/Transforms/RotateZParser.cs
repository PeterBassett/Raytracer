using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.FileTypes.VBRayScene;
using System;
using System.Xml.Linq;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Transforms
{
    [Export(typeof(XMLRayElementParser))]
    class RotateZParser : RotationParserBase
    {
        public override string LoaderType { get { return "RotateZ"; } }

        public override Matrix CreateRotationMatrix(double rotation)
        {
            return Matrix.CreateRotationZ(rotation);
        }
    }
}
