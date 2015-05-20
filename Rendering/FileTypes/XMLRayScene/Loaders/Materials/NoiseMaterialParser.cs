using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Raytracer.Rendering.Lights;
using Raytracer.Rendering.Materials;

namespace Raytracer.Rendering.FileTypes.XMLRayScene.Loaders.Materials
{
    [Export(typeof(XMLRayElementParser))]
    class NoiseMaterialParser : XMLRayElementParser
    {
        public override string LoaderType { get { return "NoiseMaterial"; } }

        public override dynamic LoadObject(VBRayScene.XMLRaySceneLoader loader, Core.Scene scene, System.Xml.Linq.XElement element, string elementName, Func<dynamic> createDefault)
        {
            var mat = new MaterialNoise();


            return mat;
        }
    }
}
