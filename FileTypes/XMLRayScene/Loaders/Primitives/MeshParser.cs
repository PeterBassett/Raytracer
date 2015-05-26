using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.Core;

using System;
using System.Xml.Linq;
using Raytracer.Rendering.Primitives;
using Raytracer.FileTypes.ObjFile;
using System.Collections.Generic;
using Raytracer.Rendering.Materials;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Primitives
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class MeshParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "Mesh"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {            
            var name = loader.LoadObject<string>(components, element, "Name", () => null);
            var meshfile = loader.LoadObject<string>(components, element, "File", () => null);

            var scale = new Vector(1,1,1);

            ReadObjMesh(name, meshfile, scale, components.Scene);

            return null;
        }

        private void ReadObjMesh(string meshName, string meshfile, Vector scale, Scene scene)
        {            
            var verticies = new List<Vector>();
            var triangles = new List<Triangle>();
            var materials = new List<Material>();
            
            var fileLoader = new ObjFileLoader();
            fileLoader.LoadFile( meshfile, triangles, materials);

            foreach (var mat in materials)
            {
                scene.AddMaterial(mat, mat.Name);
            }

            if(triangles.Count > 0)
                scene.AddMeshes(new Mesh(triangles), meshName);
        }
    }
}
