using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.FileTypes.VBRayScene;

namespace Raytracer.Rendering.FileTypes.XMLRayScene.Loaders.Transforms
{
    [Export(typeof(XMLRayElementParser))]
    class TransformLoader : XMLRayElementParser
    {
        public override string LoaderType { get { return "Transform"; } }
        
        public override dynamic LoadObject(XMLRaySceneLoader loader, Scene scene, System.Xml.Linq.XElement element, string elementName, System.Func<dynamic> createDefault)
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
