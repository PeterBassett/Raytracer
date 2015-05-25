using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using System.Xml.Linq;
using System;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Transforms
{
    [Export(typeof(XmlRayElementParser))]
    class TransformLoader : XmlRayElementParser
    {
        public override string LoaderType { get { return "Transform"; } }
        
        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var matrix = Matrix.Identity;

            foreach (var child in element.Elements())
            {
                matrix = matrix * loader.LoadObject<Matrix>(components, child, () => Matrix.Identity);            
            }

            var inverse = matrix;
            inverse.Invert();
            return new Transform(matrix, inverse);
        }
    }
}
