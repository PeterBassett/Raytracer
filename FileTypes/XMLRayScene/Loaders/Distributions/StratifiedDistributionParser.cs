using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Xml.Linq;
using Raytracer.Properties.Annotations;
using Raytracer.Rendering.Cameras;
using Raytracer.Rendering.Distributions;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Cameras
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class StratifiedDistributionParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "StratifiedDistribution"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            return new StratifiedDistribution();
        }
    }
}
