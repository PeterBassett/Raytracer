using System;
using System.ComponentModel.Composition;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.FileTypes.VBRayScene.Savers
{
    [Export(typeof(IVBRaySceneItemSaver))]
    class ViewpointSaver : IVBRaySceneItemSaver
    {
        public Type SaverForType { get { return typeof(Scene); } }
        public void SaveObject(System.IO.StreamWriter file, object ObjectToSave)
        {
            Scene scene = (Scene)ObjectToSave;

            file.WriteLine("Viewpoint");
            file.WriteLine("(");
            file.WriteLine("\t{0}, {1}, {2}, 'View Position'", scene.EyePosition.X, scene.EyePosition.Y, scene.EyePosition.Z);
            file.WriteLine("\t{0}, {1}, {2}, 'View Angle'", scene.ViewPointRotation.X, scene.ViewPointRotation.Y, scene.ViewPointRotation.Z);
            file.WriteLine("\t{0} 'Field Of View'", scene.FieldOfView);
            file.WriteLine(")");
            file.WriteLine();
        }
    }
}
