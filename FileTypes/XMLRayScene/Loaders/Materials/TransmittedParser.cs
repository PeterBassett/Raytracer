using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Raytracer.Rendering.Lights;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Materials
{
    [Export(typeof(XmlRayElementParser))]
    class TransmittedParser : MaterialComponentBaseParser
    {
        public override string LoaderType { get { return "Transmitted"; } }
    }
}
