﻿using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.FileTypes.VBRayScene;

namespace Raytracer.Rendering.FileTypes.XMLRayScene.Loaders.Values
{
    [Export(typeof(XMLRayElementParser))]
    class ExponentLoader : SingleDoubleParser
    {
        public override string LoaderType { get { return "Exponent"; } }
    }
}