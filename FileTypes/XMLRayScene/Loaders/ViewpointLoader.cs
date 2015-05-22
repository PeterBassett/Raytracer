using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.FileTypes.VBRayScene;

namespace Raytracer.FileTypes.XMLRayScene.Loaders
{
    [Export(typeof(IXmlRaySceneItemLoader))]
    class ViewpointLoader : IXmlRaySceneItemLoader
    {
        public string LoaderType { get { return "Viewpoint"; } }
        
        public void LoadObject(XmlRaySceneLoader loader, System.Xml.Linq.XElement element, Scene scene)
        {
	        var transform = loader.LoadObject<Transform>(scene, element, "Transform", () => Transform.CreateIdentityTransform());            

            scene.EyePosition = transform.ToObjectSpace(new Point(0, 0, 0));

            var rotate =  transform.GetObjectSpaceRotation();
            scene.ViewPointRotation = new Vector(MathLib.Rad2Deg(rotate.X),
                                                 MathLib.Rad2Deg(rotate.Y),
                                                 MathLib.Rad2Deg(rotate.Z));
            
            scene.FieldOfView = loader.LoadObject<float>(scene, element, "FOV", () => 90);
        }
    }
}
