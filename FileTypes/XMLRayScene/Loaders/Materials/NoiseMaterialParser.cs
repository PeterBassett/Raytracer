using System;
using System.ComponentModel.Composition;
using Raytracer.Rendering.Materials;
using Raytracer.MathTypes;
using Raytracer.FileTypes.VBRayScene;
using Raytracer.Rendering.Core;
using System.Xml.Linq;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Materials
{
    [Export(typeof(XmlRayElementParser))]
    class NoiseMaterialParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "NoiseMaterial"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, Scene scene, XElement element, string elementName, Func<dynamic> createDefault)
        {            
            var name = loader.LoadObject<string>(scene, element, "Name", () => null);
                     
            string strMaterial = loader.LoadObject<string>(scene, element, "from-material", () => null);
            var mat1 = scene.FindMaterial(strMaterial);

            if (mat1 == null)
                throw new Exception("Cannot find material '" + strMaterial + "' for CheckMaterial.");

            strMaterial = loader.LoadObject<string>(scene, element, "to-material", () => null);
            var mat2 = scene.FindMaterial(strMaterial);

            if (mat2 == null)
                throw new Exception("Cannot find material '" + strMaterial + "' for CheckMaterial.");

            var seed = loader.LoadObject<int>(scene, element, "seed", () => 0);
            var octaves = loader.LoadObject<int>(scene, element, "octaves", () => 1);
            var persistence = loader.LoadObject<float>(scene, element, "persistence", () => 0.5f);
            var scale = loader.LoadObject<Vector>(scene, element, "scale", () => new Vector(1, 1, 1));
            var offset = loader.LoadObject<float>(scene, element, "offset", () => 0.0f);
            var size = loader.LoadObject<Vector>(scene, element, "size", () => new Vector(1,1,1));

            return new MaterialNoise(mat1, mat2, seed, persistence, octaves, (float) scale.X, offset, size)
            {
                Name = name
            };
        }
    }
}
