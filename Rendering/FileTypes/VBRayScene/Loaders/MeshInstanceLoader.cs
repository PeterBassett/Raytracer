using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.IO;
using Raytracer.MathTypes;
using Raytracer.Rendering.Primitives;

namespace Raytracer.Rendering.FileTypes.VBRayScene.Loaders
{
    using Vector = Vector3;

    [Export(typeof(IVBRaySceneItemLoader))]
    class MeshInstanceLoader : IVBRaySceneItemLoader
    {
        public string LoaderType { get { return "MeshInstance"; } }

        public void LoadObject(StreamReader file, Scene scene)
        {
            Tokeniser oText = new Tokeniser();

            Vector offset = new Vector(
                 float.Parse(oText.GetToken(file)),
                 float.Parse(oText.GetToken(file)),
                 float.Parse(oText.GetToken(file))
            );

            Vector rotation = new Vector(
                float.Parse(oText.GetToken(file)),
                float.Parse(oText.GetToken(file)),
                float.Parse(oText.GetToken(file))
            );

            var meshName = oText.GetToken(file);

            MeshInstance inst = new MeshInstance()
            {
                Mesh = scene.FindMesh(meshName),
                Ori = rotation,
                Pos = offset
            };

            scene.AddObject(inst);
        }
    }
}
