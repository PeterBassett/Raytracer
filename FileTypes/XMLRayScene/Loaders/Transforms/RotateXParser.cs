using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Properties.Annotations;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Transforms
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class RotateXParser: RotationParserBase
    {
        public override string LoaderType { get { return "RotateX"; } }

        protected override Matrix CreateRotationMatrix(double rotation)
        {
            return Matrix.CreateRotationX(rotation);
        }
    }
}
