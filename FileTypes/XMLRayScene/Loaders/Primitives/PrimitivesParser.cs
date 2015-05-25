using System.ComponentModel.Composition;

using Raytracer.Rendering.Core;
using Raytracer.Rendering.Primitives;
using Raytracer.Rendering.Renderers;
using System.Xml.Linq;
using Raytracer.Rendering.Cameras;
using System.Collections.Generic;
using System;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Primitives
{
    [Export(typeof(XmlRayElementParser))]
    class PrimitivesParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "Primitives"; } }
        
        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var primitives = new List<Traceable>();
            foreach (var child in element.Elements())
            {
                var primitive = loader.LoadObject<Traceable>(components, child, () => (Traceable)null);
                if (primitive != null)
                    primitives.Add(primitive);
            }
            return primitives;        
        }
    }
}
