using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.FileTypes.ObjFile;
using Raytracer.Rendering.Primitives;

namespace Raytracer.Rendering.FileTypes.VBRayScene.Loaders
{
    using Raytracer.Rendering.Materials;
    using Vector = Vector3;

    [Export(typeof(IVBRaySceneItemLoader))]
    class MeshLoader : IVBRaySceneItemLoader
    {
        public string LoaderType { get { return "Mesh"; } }

        public void LoadObject(StreamReader file, Scene scene)
        {
            Tokeniser oText = new Tokeniser();

            var meshName = oText.GetToken(file);
            var meshfile = oText.GetToken(file);

            Vector scale = new Vector(1,1,1);

            ReadObjMesh(meshName, meshfile, scale, scene);            
        }

        private void ReadObjMesh(string meshName, string meshfile, Vector scale, Scene scene)
        {            
            List<Vector> verticies = new List<Vector>();
            List<Triangle> triangles = new List<Triangle>();
            List<Material> materials = new List<Material>();
            
            ObjFileLoader fileLoader = new ObjFileLoader();
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
