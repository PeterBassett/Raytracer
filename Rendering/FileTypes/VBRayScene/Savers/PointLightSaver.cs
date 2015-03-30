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
    class PointLightSaver : IVBRaySceneItemSaver
    {
        public Type SaverForType
        {
            get { return typeof(Light); }
        }

        public void SaveObject(StreamWriter file, object ObjectToSave)
        {
            Light light = (Light)ObjectToSave;

            if (light.Ambient.Sum() == 0 && light.Diffuse.Sum() > 0)
            {
                file.Write("LightSource(");
                file.Write("{0}, {1}, {2},", light.Pos.X, light.Pos.Y, light.Pos.Z);
                file.Write("{0}, {1}, {2}",
                    light.Diffuse.Red * 255.0f,
                    light.Diffuse.Green * 255.0f,
                    light.Diffuse.Blue * 255.0f);
                file.Write(")");
                file.WriteLine();
            }
        }
    }
}
