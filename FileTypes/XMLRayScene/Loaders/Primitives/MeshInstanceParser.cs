using System.Xml.Linq;
using Raytracer.Rendering.Primitives;
using Raytracer.FileTypes.ObjFile;
using System.Collections.Generic;
using Raytracer.Rendering.Materials;
using System.ComponentModel.Composition;
using System;
using Raytracer.Rendering.Core;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Primitives
{
    [Export(typeof(XmlRayElementParser))]
    class MeshInstanceParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "MeshInstance"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var transform = loader.LoadObject<Transform>(components, element, "Transform", () => Transform.CreateIdentityTransform());

            var name = loader.LoadObject<string>(components, element, "MeshName", () => null);

            var inst = new MeshInstance(components.scene.FindMesh(name), transform);

            return inst;
        }
    }
}
