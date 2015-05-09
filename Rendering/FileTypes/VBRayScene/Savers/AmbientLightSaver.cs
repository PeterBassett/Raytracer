using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.Rendering;
using System.IO;
using System.ComponentModel.Composition;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Lights;

namespace Raytracer.Rendering.FileTypes.VBRayScene.Savers
{
    [Export(typeof(IVBRaySceneItemSaver))]
    class AmbientLightSaver : IVBRaySceneItemSaver
    {
        public Type SaverForType
        {
            get { return typeof(AmbientLight); }
        }

        public void SaveObject(StreamWriter file, object ObjectToSave)
        {
            Light light = (Light)ObjectToSave;

            if (light.Intensity.Sum() == 0)
                return;

            file.WriteLine("AmbientLight({0}, {1}, {2})", 
                light.Intensity.Red * 255.0f,
                light.Intensity.Green * 255.0f,
                light.Intensity.Blue * 255.0f);
            file.WriteLine();
        }
    }
}
