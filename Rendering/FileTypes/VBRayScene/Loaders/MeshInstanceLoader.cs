using System.ComponentModel.Composition;
using System.IO;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Primitives;

namespace Raytracer.Rendering.FileTypes.VBRayScene.Loaders
{
    [Export(typeof(IVBRaySceneItemLoader))]
    class MeshInstanceLoader : IVBRaySceneItemLoader
    {
        public string LoaderType { get { return "MeshInstance"; } }

        public void LoadObject(StreamReader file, Scene scene)
        {
            Tokeniser oText = new Tokeniser();

            var offset = new Vector(
                 float.Parse(oText.GetToken(file)),
                 float.Parse(oText.GetToken(file)),
                 float.Parse(oText.GetToken(file))
            );

            var rotation = new Vector(
                float.Parse(oText.GetToken(file)),
                float.Parse(oText.GetToken(file)),
                float.Parse(oText.GetToken(file))
            );

            var meshName = oText.GetToken(file);

            //var material = oText.GetToken(file);

            var transform = Transform.CreateTransform(-offset, -rotation);

            var inst = new MeshInstance(scene.FindMesh(meshName), transform);

            //inst.Material = scene.FindMaterial(material);

            scene.AddObject(inst);
        }
    }
}
