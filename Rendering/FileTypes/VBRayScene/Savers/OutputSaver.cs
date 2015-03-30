using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace Raytracer.Rendering.FileTypes.VBRayScene.Saver
{
    [Export(typeof(IVBRaySceneItemSaver))]
    class OutputSaver : IVBRaySceneItemSaver
    {
        public Type SaverForType
        {
            get { return typeof(Raytracer.Rendering.Scene); }
        }

        public void SaveObject(System.IO.StreamWriter file, object ObjectToSave)
        {
            Scene scene = (Scene)ObjectToSave;

            file.WriteLine("OutputSize({0}, {1})", scene.Width, scene.Height);
            file.WriteLine();
        }
    }
}
