using System.Xml.Linq;
using Raytracer.Rendering.Cameras;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Renderers;
using Raytracer.FileTypes.XMLRayScene.Loaders;

namespace Raytracer.FileTypes.XMLRayScene
{
    interface IXmlRaySceneItemLoader
    {
        string LoaderType { get; }
        void LoadObject(XmlRaySceneLoader loader, XElement element, SystemComponents components);
    }
}
