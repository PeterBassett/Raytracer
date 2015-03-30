using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace Raytracer.Rendering.FileTypes.VBRayScene.Saver
{
    [Export(typeof(IVBRaySceneItemSaver))]
    class RenderSettingsSaver : IVBRaySceneItemSaver
    {
        public Type SaverForType
        {
            get { return typeof(Raytracer.Rendering.Scene); }
        }

        public void SaveObject(System.IO.StreamWriter file, object ObjectToSave)
        {
            Scene scene = (Scene)ObjectToSave;

            file.WriteLine("RenderSettings");
            file.WriteLine("(");
            file.WriteLine("\t{0}, 'Recursion Depth'", scene.RecursionDepth);
            file.WriteLine("\t{0}, 'Trace Shadows'", scene.TraceShadows);
            file.WriteLine("\t{0}, 'Trace Reflections'", scene.TraceReflections);
            file.WriteLine("\t{0}, 'Trace Refractions'", scene.TraceRefractions);
            file.WriteLine(")");
            file.WriteLine();
        }
    }
}
