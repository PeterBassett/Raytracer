using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.Rendering;
using System.IO;
using System.ComponentModel.Composition;

namespace Raytracer.Rendering.FileTypes.VBRayScene.Savers
{
    [Export(typeof(IVBRaySceneItemSaver))]
    class AmbientLightSaver : IVBRaySceneItemSaver
    {
        public Type SaverForType
        {
            get { return typeof(Light); }
        }

        public void SaveObject(StreamWriter file, object ObjectToSave)
        {
            Light light = (Light)ObjectToSave;

            if (light.Ambient.Sum() > 0 && light.Diffuse.Sum() == 0)
            {
                file.WriteLine("AmbientLight({0}, {1}, {2})", 
                    light.Ambient.Red * 255.0f,
                    light.Ambient.Green * 255.0f,
                    light.Ambient.Blue * 255.0f);
                file.WriteLine();
            }
        }
    }
}
