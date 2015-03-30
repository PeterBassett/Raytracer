using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.MathTypes;
using System.ComponentModel.Composition;

namespace Raytracer.Rendering.FileTypes.VBRayScene.Savers
{
    [Export(typeof(IVBRaySceneItemSaver))]
    class ViewpointSaver : IVBRaySceneItemSaver
    {
        public Type SaverForType { get { return typeof(Raytracer.Rendering.Scene); } }
        public void SaveObject(System.IO.StreamWriter file, object ObjectToSave)
        {
            Scene scene = (Scene)ObjectToSave;

            file.WriteLine("Viewpoint");
            file.WriteLine("(");
            file.WriteLine("\t{0}, {1}, {2}, 'View Position'", scene.Pos.X, scene.Pos.Y, scene.Pos.Z);
            file.WriteLine("\t{0}, {1}, {2}, 'View Angle'", scene.Dir.X, scene.Dir.Y, scene.Dir.Z);
            file.WriteLine("\t{0} 'Field Of View'", scene.FieldOfView);
            file.WriteLine(")");
            file.WriteLine();
        }
    }
}
