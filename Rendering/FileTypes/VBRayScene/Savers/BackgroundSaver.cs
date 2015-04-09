using System;
using System.ComponentModel.Composition;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.FileTypes.VBRayScene.Savers
{
    [Export(typeof(IVBRaySceneItemSaver))]
    class BackgroundSaver : IVBRaySceneItemSaver
    {
        public Type SaverForType { get { return typeof(Scene); } }
        public void SaveObject(System.IO.StreamWriter file, object ObjectToSave)
        {
            
        }
    }
}
