using System.IO;
using Raytracer.Rendering.Core;
using System.Xml.Linq;
using Raytracer.Rendering.FileTypes.VBRayScene;
using System;

namespace Raytracer.Rendering.FileTypes.XMLRayScene
{
    interface IXMLRaySceneItemLoader
    {
        string LoaderType { get; }
        void LoadObject(XMLRaySceneLoader loader, XElement element, Scene scene);
    }
}
