using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.FileTypes.VBRayScene;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Transforms
{
    [Export(typeof(XmlRayElementParser))]
    class TransformLoader : XmlRayElementParser
    {
        public override string LoaderType { get { return "Transform"; } }
        
        public override dynamic LoadObject(XmlRaySceneLoader loader, Scene scene, System.Xml.Linq.XElement element, string elementName, System.Func<dynamic> createDefault)
        {
            var matrix = Matrix.Identity;

            foreach (var child in element.Elements())
            {
                matrix = matrix * loader.LoadObject<Matrix>(scene, child, () => Matrix.Identity);            
            }

            var inverse = matrix;
            inverse.Invert();
            return new Transform(matrix, inverse);
        }
    }
}
