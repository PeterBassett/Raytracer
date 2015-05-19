using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.FileTypes.VBRayScene;

namespace Raytracer.Rendering.FileTypes.XMLRayScene.Loaders
{
    [Export(typeof(IXMLRaySceneItemLoader))]
    class ViewpointLoader : IXMLRaySceneItemLoader
    {
        public string LoaderType { get { return "Viewpoint"; } }
        
        public void LoadObject(XMLRaySceneLoader loader, System.Xml.Linq.XElement element, Scene scene)
        {
	        var transform = loader.LoadObject<Transform>(scene, element, "Transform", () => Transform.CreateIdentityTransform());            

            scene.EyePosition = transform.ToWorldSpace(new Point(0, 0, 0));
            scene.ViewPointRotation = transform.ToWorldSpace(new Vector(0, 0, 1));

            scene.FieldOfView = loader.LoadObject<float>(scene, element, "FOV", () => 90);
        }
    }
}
