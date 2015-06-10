using System.Xml.Linq;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.Primitives;
using System.ComponentModel.Composition;
using System;
using Raytracer.Rendering.Core;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Primitives
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class MeshInstanceParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "MeshInstance"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var transform = loader.LoadObject(components, element, "Transform", Transform.CreateIdentityTransform);

            var name = loader.LoadObject<string>(components, element, "MeshName", () => null);

            string strMaterial = loader.LoadObject<string>(components, element, "Material", () => null);
            var mat = components.Scene.FindMaterial(strMaterial);

            var inst = new MeshInstance(components.Scene.FindMesh(name), transform, mat);

            return inst;
        }
    }
}
