using System.Xml.Linq;
using Raytracer.FileTypes.XMLRayScene.Loaders;

namespace Raytracer.FileTypes.XMLRayScene
{
    interface IXmlRaySceneItemLoader
    {
        string LoaderType { get; }
        void LoadObject(XmlRaySceneLoader loader, XElement element, SystemComponents components);
    }
}
