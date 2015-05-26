using System;
using System.ComponentModel.Composition;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.Materials;
using Raytracer.MathTypes;
using System.Xml.Linq;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Materials
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class NoiseMaterialParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "NoiseMaterial"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var name = loader.LoadObject<string>(components, element, "Name", () => null);

            string strMaterial = loader.LoadObject<string>(components, element, "from-material", () => null);
            var mat1 = components.scene.FindMaterial(strMaterial);

            if (mat1 == null)
                throw new Exception("Cannot find material '" + strMaterial + "' for CheckMaterial.");

            strMaterial = loader.LoadObject<string>(components, element, "to-material", () => null);
            var mat2 = components.scene.FindMaterial(strMaterial);

            if (mat2 == null)
                throw new Exception("Cannot find material '" + strMaterial + "' for CheckMaterial.");

            var seed = loader.LoadObject<int>(components, element, "seed", () => 0);
            var octaves = loader.LoadObject<int>(components, element, "octaves", () => 1);
            var persistence = loader.LoadObject<float>(components, element, "persistence", () => 0.5f);
            var scale = loader.LoadObject<Vector>(components, element, "scale", () => new Vector(1, 1, 1));
            var offset = loader.LoadObject<float>(components, element, "offset", () => 0.0f);
            var size = loader.LoadObject<Vector>(components, element, "size", () => new Vector(1, 1, 1));

            return new MaterialNoise(mat1, mat2, seed, persistence, octaves, (float) scale.X, offset, size)
            {
                Name = name
            };
        }
    }
}
