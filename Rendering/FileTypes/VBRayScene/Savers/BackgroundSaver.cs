using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.MathTypes;
using System.ComponentModel.Composition;

namespace Raytracer.Rendering.FileTypes.VBRayScene.Savers
{
    [Export(typeof(IVBRaySceneItemSaver))]
    class BackgroundSaver : IVBRaySceneItemSaver
    {
        public Type SaverForType { get { return typeof(Raytracer.Rendering.Scene); } }
        public void SaveObject(System.IO.StreamWriter file, object ObjectToSave)
        {
            
        }
    }
}
