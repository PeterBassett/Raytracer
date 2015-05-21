using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Raytracer.Rendering.Lights;
using Raytracer.Rendering.Materials;
using Raytracer.MathTypes;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Materials
{
    [Export(typeof(XMLRayElementParser))]
    class NoiseMaterialParser : XMLRayElementParser
    {
        public override string LoaderType { get { return "NoiseMaterial"; } }

        public override dynamic LoadObject(VBRayScene.XMLRaySceneLoader loader, Rendering.Core.Scene scene, System.Xml.Linq.XElement element, string elementName, Func<dynamic> createDefault)
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

            MaterialNoise mat = new MaterialNoise(mat1, mat2, seed, persistence, octaves, (float)scale.X, offset, size);
            mat.Name = name;

            return mat;
        }
    }
}
