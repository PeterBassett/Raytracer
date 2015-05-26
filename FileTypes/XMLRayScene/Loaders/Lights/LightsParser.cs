using System.ComponentModel.Composition;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.Core;
using System.Xml.Linq;
using System.Collections.Generic;
using System;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Lights
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class LightsParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "Lights"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var lights = new List<Light>();
            foreach (var child in element.Elements())
            {
                var light = loader.LoadObject<Light>(components, child);
                if (light != null)
                    lights.Add(light);
            }
            return lights;
        }
    }
}
