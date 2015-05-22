using System.IO;
using Raytracer.Rendering.Core;
using System.Xml.Linq;
using Raytracer.FileTypes.VBRayScene;
using System;

namespace Raytracer.FileTypes.XMLRayScene
{
    interface IXmlRaySceneItemLoader
    {
        string LoaderType { get; }
        void LoadObject(XmlRaySceneLoader loader, XElement element, Scene scene);
    }
}
