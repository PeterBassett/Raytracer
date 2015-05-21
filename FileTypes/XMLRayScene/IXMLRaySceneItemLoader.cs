using System.IO;
using Raytracer.Rendering.Core;
using System.Xml.Linq;
using Raytracer.FileTypes.VBRayScene;
using System;

namespace Raytracer.FileTypes.XMLRayScene
{
    interface IXMLRaySceneItemLoader
    {
        string LoaderType { get; }
        void LoadObject(XMLRaySceneLoader loader, XElement element, Scene scene);
    }
}
