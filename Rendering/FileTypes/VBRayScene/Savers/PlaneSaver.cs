using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.Rendering;
using System.IO;
using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Primitives;

namespace Raytracer.Rendering.FileTypes.VBRayScene.Savers
{
    [Export(typeof(IVBRaySceneItemSaver))]
    class PlaneSaver : IVBRaySceneItemSaver
    {        
        public Type SaverForType
        {
            get { return typeof(Raytracer.Rendering.Primitives.Plane); }
        }

        public void SaveObject(StreamWriter file, object ObjectToSave)
        {
            Plane plane = (Plane)ObjectToSave;
            
            file.WriteLine("Plane");
            file.WriteLine("(");
            file.WriteLine("\t{0}, {1}, {2}, 'Point on plane'", plane.Pos.X, plane.Pos.Y, plane.Pos.Z);
            file.WriteLine("\t{0}, {1}, {2}, 'Normal Vector'", plane.Normal.X, plane.Normal.Y, plane.Normal.Z);
            file.WriteLine("\t\"{0}\" 'Material'", plane.Material.Name);
            file.WriteLine(")");
            file.WriteLine();
        }
    }
}